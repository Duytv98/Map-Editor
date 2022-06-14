using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private List<InfoWord> wordInfoDic = null;

    public InfoWord InfoWord { get => infoWord; set => infoWord = value; }
    public bool ClearWord { get => clearWord; set => clearWord = value; }
    public DifficultyInfo DifficultyInfo { get => difficultyInfo; set => difficultyInfo = value; }
    public Dictionary<string, Word> WordDic { get => wordDic; set => wordDic = value; }
    public List<InfoWord> WordInfoDic { get => wordInfoDic; set => wordInfoDic = value; }

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
        WordInfoDic = new List<InfoWord>();
        DifficultyInfo = GameDefine.DIFFICULTYINFOS[0];
        CreateBoard();
        btnClear.color = ClearWord ? Color.white : Color.gray;
        GenerateWordInfor();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnChangeDifficulty(int val)
    {
        if (val >= 0 && val <= 2) DifficultyInfo = GameDefine.DIFFICULTYINFOS[val];
        Debug.Log(DifficultyInfo.Log());
        CreateBoard();
    }
    private void CreateBoard()
    {

        ClearBoardGridLayout();
        var cols = DifficultyInfo.boardColumnSize;
        var rows = DifficultyInfo.boardRowSize;
        int column = 0;
        int row = 0;
        Debug.Log("cols * rows: " + cols * rows);
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
        }
        Debug.Log(DifficultyInfo.Log());
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
        Debug.Log(parentTransform.childCount);
        if (parentTransform.childCount <= 0) return;
        foreach (Transform item in parentTransform)
        {
            // GameObject word = item.GetComponent<GameObject>();
            Destroy(item.gameObject);
        }
        WordDic.Clear();
    }

    public void SetIndexListWord(Position position, string inText)
    {
        if (InfoWord == null) return;
        if (string.IsNullOrEmpty(InfoWord.Word))
            Debug.Log(position.Log());
        InfoWord.SetPositionWord(position, inText);
    }

    public void OnClickClear()
    {
        ClearWord = !ClearWord;
        btnClear.color = ClearWord ? Color.white : Color.gray;
    }

}
