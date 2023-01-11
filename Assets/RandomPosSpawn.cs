using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPosSpawn : MonoBehaviour
{
    [SerializeField] private Transform[] _tpPoints;

    private int _randomNumber;

    public void ChoosePosition()
    {
        gameObject.SetActive(true);
        _randomNumber = Random.Range(0, _tpPoints.Length);    
        transform.position = _tpPoints[_randomNumber].position;
    }
}
