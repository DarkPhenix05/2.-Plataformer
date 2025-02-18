using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Number: MonoBehaviour
{
    //public static int keyNumber = 0;
    public int _keyNumber = 0;

    public static Number Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        //keyNumber = 0
        _keyNumber = 0;
    }
}
