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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
