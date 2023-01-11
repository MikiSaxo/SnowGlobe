using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    
    [Header("Objects To Get")]
    [SerializeField] private GameObject[] _objects;

    [Header("Chrono")] [SerializeField] private TextMeshProUGUI _chronoText;
    [SerializeField] private float _chrono;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var obj in _objects)
        {
            obj.SetActive(false);
        }
    }

    public void ObjectGet(int index)
    {
        _objects[index].SetActive(true);
    }
}