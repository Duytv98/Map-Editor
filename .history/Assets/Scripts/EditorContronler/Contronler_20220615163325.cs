using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contronler : MonoBehaviour
{
    public static Contronler Instance;
    [SerializeField] private GameObject randomBoard = null;
    [SerializeField] private GameObject customBoard = null;
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
        randomBoard.SetActive(true);
    }
    public void OnChangeRandomBoard()
    {
        randomBoard.SetActive(true);
        customBoard.SetActive(false);
    }
    public void OnChangeCustomBoard()
    {
        randomBoard.SetActive(false);
        customBoard.SetActive(true);
    }
}
