using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPosSpawn : MonoBehaviour
{
    [SerializeField] private Transform[] _tpPoints;

    private int _randomNumber;
    void Awake()
    {
        _randomNumber = Random.Range(0, _tpPoints.Length);    
    }

    private void Start()
    {
        transform.position = _tpPoints[_randomNumber].position;
    }
}
