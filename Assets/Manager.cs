using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [Header("Objects To Get")] [SerializeField]
    private GameObject[] _objects;

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

    private void Update()
    {
        if (!(_chrono > 0)) return;
        
        _chrono -= Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(_chrono);
        var format = time.ToString(@"g");
        format = format.Substring(5, 5);
        _chronoText.text = format;
    }

    public void ObjectGet(int index)
    {
        _objects[index].SetActive(true);
    }
}