using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    [SerializeField] private Text outText = null;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnEndEdit(string str)
    {
        var upperText = outText.text.ToUpper();
        Debug.Log("outText.text: " + outText.text +  "   upperText: " + upperText);
        Debug.Log(upperText != outText.text);
        if (upperText != outText.text) outText.text = upperText;
    }
}