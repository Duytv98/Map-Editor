using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    public string CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    [SerializeField] private InputField outText = null;
    [SerializeField] private GameObject chooseIndex = null;

    [SerializeField] private Image bg = null;

    private string inText = null;

    public int column = 0;
    public int row = 0;

    public string InText { get => inText; set => inText = value; }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnValueChanged(string str)
    {
        Debug.Log("11111111");
        Debug.Log("changed");
        var upperText = str.ToUpper();
        Debug.Log(upperText);
        Debug.Log(CHARACTERS.Contains(upperText));

        if (CHARACTERS.Contains(upperText))
        {
            if (upperText != outText.text)
            {
                outText.text = upperText;
                InText = outText.text;
            }
        }
        else
        {
            outText.text = "";
            InText = null;
        }
        if (!string.IsNullOrEmpty(outText.text))
        {
            chooseIndex.SetActive(true);
        }
    }
    public void SetIndex(int column, int row)
    {
        this.column = column;
        this.row = row;
    }
    public void ConvertIndex()
    {
        if (!CustomBoard.Instance.ClearWord)
        {
            CustomBoard.Instance.SetIndexListWord(new Position(row, column), InText);
        }
        else
        {
            chooseIndex.SetActive(false);
            outText.text = "";
        }

    }
    public void SetChoid(Color color)
    {
        bg.color = color;
    }
    public void SetWord(string str)
    {
        var upperText = str.ToUpper();
        outText.text = upperText;
        InText = upperText;
        chooseIndex.SetActive(true);
    }
}
