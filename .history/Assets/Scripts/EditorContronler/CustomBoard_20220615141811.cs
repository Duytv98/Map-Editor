using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SFB;
using SimpleJSON;
public class CustomBoard : MonoBehaviour
{

    public static CustomBoard Instance;
    public string CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    [SerializeField] GridLayoutGroup boardGridLayout = null;
    [SerializeField] RectTransform transformBoard = null;
    [SerializeField] Transform wordPrefab = null;

    [SerializeField] Transform contentWordsAdd = null;
    [SerializeField] Transform wordAdd = null;


    [SerializeField] Image btnClear = null;
    private bool clearWord = false;

    private InfoWord infoWord = null;


    private DifficultyInfo difficultyInfo = null;
    private float sizeWord = 70f;
    Dictionary<string, Word> wordDic = null;
    private List<InfoWord> listWordInfo = null;

    public InfoWord InfoWord { get => infoWord; set => infoWord = value; }
    public bool ClearWord { get => clearWord; set => clearWord = value; }
    public DifficultyInfo DifficultyInfo { get => difficultyInfo; set => difficultyInfo = value; }
    public Dictionary<string, Word> WordDic { get => wordDic; set => wordDic = value; }
    public List<InfoWord> ListWordInfo { get => listWordInfo; set => listWordInfo = value; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        WordDic = new Dictionary<string, Word>();
        ListWordInfo = new List<InfoWord>();
        DifficultyInfo = GameDefine.DIFFICULTYINFOS[0];
        CreateBoard();
        btnClear.color = ClearWord ? Color.white : Color.gray;
        GenerateWordInfor();
    }
    public void OnChangeDifficulty(int val)
    {
        if (val >= 0 && val <= 2) DifficultyInfo = GameDefine.DIFFICULTYINFOS[val];
        // Debug.Log(DifficultyInfo.Log());
        CreateBoard();
    }
    private void CreateBoard()
    {

        ClearCustomBoard();
        var cols = DifficultyInfo.boardColumnSize;
        var rows = DifficultyInfo.boardRowSize;
        int column = 0;
        int row = 0;
        // Debug.Log("cols * rows: " + cols * rows);
        boardGridLayout.cellSize = new Vector2(sizeWord, sizeWord);
        transformBoard.sizeDelta = new Vector2(sizeWord * cols, sizeWord * rows);
        for (int i = 0; i < cols * rows; i++)
        {
            Transform transformWord = Instantiate(wordPrefab, Vector3.zero, Quaternion.identity, transformBoard);

            Word _scriptWord = transformWord.GetComponent<Word>();
            if (column == cols)
            {
                column = 0;
                row++;
            }
            _scriptWord.SetIndex(column, row);
            WordDic.Add(GetKeyWord(row, column), _scriptWord);
            column++;
        }
    }
    private void GenerateWordInfor()
    {
        var maxWord = DifficultyInfo.maxWords;
        for (int i = 0; i < maxWord; i++)
        {
            Transform transformWordInfor = Instantiate(wordAdd, Vector3.zero, Quaternion.identity, contentWordsAdd);

            InfoWord _scriptWord = transformWordInfor.GetComponent<InfoWord>();
            ListWordInfo.Add(_scriptWord);
        }
    }
    public void RandomWord()
    {
        foreach (var item in WordDic)
        {
            Word word = item.Value;
            var _char = CHARACTERS[UnityEngine.Random.Range(0, CHARACTERS.Length)].ToString();
            if (string.IsNullOrEmpty(word.InText))
            {
                word.SetWord(_char);
            }
        }
    }
    public string GetKeyWord(int row, int col)
    {
        return string.Format("{0}x{1}", row, col);
    }
    public string IndexToChar(int row, int col)
    {
        return WordDic[GetKeyWord(row, col)].InText;
    }
    public Word GetWordChoose(int row, int col)
    {
        return WordDic[GetKeyWord(row, col)];
    }
    private void ClearBoardGridLayout()
    {
        Transform parentTransform = boardGridLayout.transform;
        if (parentTransform.childCount <= 0) return;
        foreach (Transform item in parentTransform)
        {
            // GameObject word = item.GetComponent<GameObject>();
            Destroy(item.gameObject);
        }
        WordDic.Clear();
    }
    private void ClearWordInfo()
    {
        if (contentWordsAdd.childCount <= 0) return;
        foreach (Transform item in contentWordsAdd)
        {
            // GameObject word = item.GetComponent<GameObject>();
            Destroy(item.gameObject);
        }
        ListWordInfo.Clear();
        for (int i = 0; i < ListWordInfo.Count; i++)
        {
            var wordInfo = ListWordInfo[i];
            if (!wordInfo.IsComplate)
            {
                Destroy(wordInfo.gameObject);
            }
        }
    }
    public void SetIndexListWord(Position position, string inText)
    {
        if (InfoWord == null) return;
        // if (string.IsNullOrEmpty(InfoWord.Word))
        InfoWord.SetPositionWord(position, inText);
        // Debug.Log(position.Log());
    }

    public void OnClickClear()
    {
        ClearWord = !ClearWord;
        btnClear.color = ClearWord ? Color.white : Color.gray;
    }
    public void PlusWordInfo()
    {
        Transform transformWordInfor = Instantiate(wordAdd, Vector3.zero, Quaternion.identity, contentWordsAdd);

        InfoWord _scriptWord = transformWordInfor.GetComponent<InfoWord>();
        ListWordInfo.Add(_scriptWord);
    }
    public void CreateLevelBoard()
    {
        foreach (var item in WordDic)
        {
            if (string.IsNullOrEmpty(item.Value.InText)) return;
        }
        Board boardGenerate = new Board();
        List<string> words = new List<string>();
        List<List<char>> boardCharacters = GetBoardCharacters();
        List<Board.WordPlacement> wordPlacements = new List<Board.WordPlacement>();
        foreach (var wordInfo in ListWordInfo)
        {
            if (!wordInfo.IsComplate)
            {
                Destroy(wordInfo.gameObject);
            }
            else
            {
                wordPlacements.Add(wordInfo.WordPlacement);
                words.Add(wordInfo.Word);
            }
        }
        boardGenerate.rows = difficultyInfo.boardRowSize;
        boardGenerate.cols = difficultyInfo.boardColumnSize;
        boardGenerate.wordPlacements = wordPlacements;
        boardGenerate.boardCharacters = boardCharacters;
        boardGenerate.words = words;
        // Debug.Log(boardGenerate.ToString());
        Debug.Log(Utilities.ConvertToJsonString(boardGenerate.ToJson()));
        Debug.Log(boardGenerate.rows);
        Debug.Log(boardGenerate.boardCharacters.Count);

    }
    private List<List<char>> GetBoardCharacters()
    {
        List<List<char>> listChar = new List<List<char>>();
        int maxCol = difficultyInfo.boardColumnSize;
        int col = 0;
        int row = 0;
        listChar.Add(new List<char>());
        List<char> listCharacters = new List<char>();
        foreach (var item in WordDic)
        {
            listCharacters.Add(Char.Parse(item.Value.InText));
        }
        for (int i = 0; i < listCharacters.Count; i++)
        {
            if (col < maxCol)
            {
                listChar[row].Add(listCharacters[i]);
            }
            else if (col == maxCol)
            {
                listChar.Add(new List<char>());
                row++;
                listChar[row].Add(listCharacters[i]);
                col = 0;
            }
            col++;
        }
        return listChar;
    }

    private void ClearCustomBoard()
    {
        ClearBoardGridLayout();
        // if (contentWordsAdd.childCount <= 0)

    }

}
