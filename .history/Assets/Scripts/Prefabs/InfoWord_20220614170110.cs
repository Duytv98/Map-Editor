using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InfoWord : MonoBehaviour
{
    [SerializeField] private Image bg = null;

    [SerializeField] private InputField inputWord = null;


    private string word;
    private int maxWord = 0;
    private Position strartWord;
    private Position endWord;
    private bool chooseStart = false;
    private bool chooseEnd = false;

    private bool getWordBoard = false;


    public int MaxWord
    { get => maxWord; set => maxWord = value; }
    public string Word { get => word; set => word = value; }
    private Dictionary<string, Word> wordUseDic = null;

    [SerializeField] Text bgTxtStartWord = null;
    [SerializeField] Text txtStartWord = null;
    [SerializeField] Image bgStartWord = null;
    [SerializeField] Text bgTxtEndWord = null;
    [SerializeField] Text txtEndWord = null;
    [SerializeField] Image bgEndWord = null;


    [SerializeField] private Color[] bgColors = null;



    private void Start()
    {
        wordUseDic = new Dictionary<string, Word>();
    }

    public void OnValueChanged(string str)
    {
        var upperText = str.ToUpper();
        if (string.IsNullOrWhiteSpace(str))
        {
            inputWord.text = "";
            Word = null;
        }
        else if (upperText != inputWord.text)
        {
            inputWord.text = upperText;
            Word = upperText;
        }
        Debug.Log("word change: " + Word);
    }


    public void SetPositionWord(Position position, string inText)
    {
        if (chooseStart) SetStartWord(position);
        else if (chooseEnd) SetEndWord(position);
    }

    public void SetStartWord(Position position)
    {
        Debug.Log("SetStartWord");

        var wordChoose = CustomBoard.Instance.IndexToChar(position.row, position.col);
        Debug.Log("Word: " + Word);
        if (string.IsNullOrEmpty(Word))
        {
            Debug.Log("null");
            Word = wordChoose;
            inputWord.text = wordChoose.ToUpper();
            getWordBoard = true;
        }
        else if (!Word.Equals(wordChoose)) return;

        strartWord = position;
        bgTxtStartWord.text = string.Format("{0}x{1}", position.col, position.row);
        txtStartWord.text = string.Format("{0}x{1}", position.col, position.row);
        ChooseStartWord();
    }
    public void SetEndWord(Position position)
    {
        wordUseDic.Clear();
        Debug.Log("SetEndWord");
        Tuple<bool, string> status = CheckLine(strartWord, position);
        Debug.Log("status: " + status.Item1 + "word: " + status.Item2);
        if (status.Item1 && getWordBoard)
        {
            Debug.Log("status.Item1 && getWordBoard");
            getWordBoard = false;
            inputWord.text = status.Item2.ToUpper();
        }
        else if (!status.Item1 || !Word.Equals(status.Item2))
        {

            Debug.Log("!status.Item1 || !Word.Equals(status.Item2)");
            return;
        }
        Debug.Log("di den cuoi");
        endWord = position;
        bgTxtEndWord.text = string.Format("{0}x{1}", position.col, position.row);
        txtEndWord.text = string.Format("{0}x{1}", position.col, position.row);
        ChooseEndWord();
    }

    private Tuple<bool, string> CheckLine(Position startPos, Position endPos)
    {
        Dictionary<string, Word> dictionary = new Dictionary<string, Word>();
        int h = 0; // ngang (row)
        int v = 0; // doc (col)
        v = endPos.row - startPos.row >= 0 ? endPos.row - startPos.row == 0 ? 0 : 1 : -1;
        h = endPos.col - startPos.col >= 0 ? endPos.col - startPos.col == 0 ? 0 : 1 : -1;

        int maxRow = CustomBoard.Instance.DifficultyInfo.boardRowSize;
        int maxCol = CustomBoard.Instance.DifficultyInfo.boardColumnSize;

        Position tempPos = new Position(startPos.row, startPos.col);
        var word = CustomBoard.Instance.GetWordChoose(tempPos.row, tempPos.col);
        dictionary.Add(CustomBoard.Instance.GetKeyWord(tempPos.row, tempPos.col), word);
        string wordChoose = word.InText;
        bool status = false;
        while (tempPos.row >= 0 && tempPos.row < maxRow && tempPos.col >= 0 && tempPos.col < maxCol && !status)
        {
            tempPos.NextPosition(v, h);
            if (tempPos.row >= 0 && tempPos.row < maxRow && tempPos.col >= 0 && tempPos.col < maxCol)
            {
                word = CustomBoard.Instance.GetWordChoose(tempPos.row, tempPos.col);
                dictionary.Add(CustomBoard.Instance.GetKeyWord(tempPos.row, tempPos.col), word);
                wordChoose += word.InText;
            }
            if (tempPos.Equals(endPos)) status = true;
        }
        if (status) wordUseDic = dictionary;
        return Tuple.Create(status, wordChoose);
    }

    public void ChooseStartWord()
    {
        chooseStart = !chooseStart;
        bgStartWord.color = chooseStart ? Color.green : Color.grey;
        SetInfoWord(chooseStart);
    }
    public void ChooseEndWord()
    {
        chooseEnd = !chooseEnd;
        bgEndWord.color = chooseEnd ? Color.green : Color.grey;
        SetInfoWord(chooseEnd);
    }
    public void SetInfoWord(bool status)
    {
        CustomBoard.Instance.InfoWord = status ? this : null;
    }
    public void OnAddWord()
    {
        Color color = bgColors[UnityEngine.Random.Range(0, bgColors.Length)];
        bg.color = new Color(color.r, color.g, color.b, 0.7f);
        foreach (var item in wordUseDic)
        {
            item.Value.SetChoid(color);
        }
    }
}
