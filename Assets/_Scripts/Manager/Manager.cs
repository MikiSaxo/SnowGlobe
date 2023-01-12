using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Collections;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [Header("Player")] [SerializeField] private GameObject _player;
    [SerializeField] private Transform _spawnPointPlayer;
    
    [Header("Objects To Get")] 
    [SerializeField] private GameObject[] _objectsUI;
    [SerializeField] private GameObject[] _objects;

    [Header("Chrono")] [SerializeField] private TextMeshProUGUI _chronoText;
    [Tooltip("In seconds")] [SerializeField] private float[] _chrono;

    private float _actualChrono;
    private int _countObj;
    private int _actualLevel;

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

        _countObj = 0;
        _actualChrono = _chrono[_actualLevel];
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

    private void ChangeNextLevel()
    {
        _actualLevel++;

        if (_actualLevel >= _chrono.Length)
            SceneManager.LoadScene(0);
        else
            StartScene();
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
            if(ShakeObj.Instance != null)
                ShakeObj.Instance.StartShakingCam(0);
            
            StartScene();
        }
    }

    public void ObjectGet(int index)
    {
        _objectsUI[index].SetActive(true);
        _countObj++;
        
        if(_countObj >= 3)
            ChangeNextLevel();
    }
}