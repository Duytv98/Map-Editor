using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contronler : MonoBehaviour
{
    public static Contronler Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
