using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    [Header("Objects To Get")]
    [SerializeField] private GameObject[] _objects;

    [Header("Chrono")] [SerializeField] private TextMeshProUGUI _chronoText;
    [SerializeField] private float _chrono;

    private void Start()
    {
        foreach (var obj in _objects)
        {
            obj.SetActive(false);
        }
    }
}