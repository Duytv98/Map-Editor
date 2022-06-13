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

    [SerializeField] GridLayoutGroup boardGridLayout = null;
    [SerializeField] RectTransform transformBoard = null;
    [SerializeField] Transform wordPrefab = null;


    private DifficultyInfo difficultyInfo = null;

    public void OnChangeDifficulty(int val)
    {
        if (val >= 0 && val <= 2) difficultyInfo = GameDefine.DIFFICULTYINFOS[val];
        Debug.Log(difficultyInfo.Log());
        CreateBoard();

    }
    private void CreateBoard()
    {
        ClearBoardGridLayout();
        var cols = difficultyInfo.boardColumnSize;
        var rows = difficultyInfo.boardRowSize;
        int column = 0;
        int row = 0;
        Debug.Log("cols * rows: " + cols * rows);
        boardGridLayout.cellSize = new Vector2(sizeWord, sizeWord);
        transformBoard.sizeDelta = new Vector2(sizeWord * cols, sizeWord * rows);
        // Transform tran = boardGridLayout.GetComponent<Transform>();
        // tran.localPosition = new Vector3(0, 0, 0);
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
            column++;
        }
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
    }
    private void Start()
    {
        difficultyInfo = GameDefine.DIFFICULTYINFOS[0];
        CreateBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
