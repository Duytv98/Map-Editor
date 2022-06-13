using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    [SerializeField] private InputField outText = null;
    private string inText = null;

    public int column = 0;
    public int row = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnValueChanged(string str)
    {
        var upperText = outText.text.ToUpper();
        if (upperText != outText.text)
        {
            outText.text = upperText;
            inText = outText.text;
        }
    }
    public void SetIndex(int column, int row)
    {
        this.column = column;
        this.row = row;
    }
}
