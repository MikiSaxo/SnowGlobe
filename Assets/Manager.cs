using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [Header("Player")] [SerializeField] private GameObject _player;
    [SerializeField] private Transform _spawnPointPlayer;
    
    [Header("Objects To Get")] 
    [SerializeField] private GameObject[] _objectsUI;
    [SerializeField] private GameObject[] _objects;

    [Header("Chrono")] [SerializeField] private TextMeshProUGUI _chronoText;
    [SerializeField] private float _chrono;

    private float _actualChrono;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartScene();
    }

    private void StartScene()
    {
        print("startScene");

        // ShakeCam.Instance.StartShakingCam(0);
        
        _actualChrono = _chrono;

        _player.transform.position = _spawnPointPlayer.position;

        foreach (var obj in _objectsUI)
        {
            obj.SetActive(false);
        }
        foreach (var obj in _objects)
        {
            obj.GetComponent<RandomPosSpawn>().ChoosePosition();
        }
    }

    private void Update()
    {
        if (_actualChrono > 0)
        {
            _actualChrono -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(_actualChrono);
            var format = time.ToString(@"g");
            format = format.Substring(5, 5);
            _chronoText.text = format;
        }
        else
        {
            StartScene();
        }
    }

    public void ObjectGet(int index)
    {
        _objectsUI[index].SetActive(true);
    }
}