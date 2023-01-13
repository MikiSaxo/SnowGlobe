using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Manager : MonoBehaviour
{
    public static Manager Instance;


    [Header("Player")] [SerializeField] private GameObject _player;
    [SerializeField] private Transform _spawnPointPlayer;

    [Header("Objects To Get")] [SerializeField]
    private GameObject[] _objectsUI;

    [SerializeField] private GameObject[] _objects;

    [Header("Chrono")] [SerializeField] private TextMeshProUGUI _chronoText;
    [SerializeField] private int _firstChrono;
    [SerializeField] private int _chronoReducePercent;

    [Header("Random Spawn Obj")] [SerializeField]
    private GameObject[] _randomObj;

    [Header("Stage")] [SerializeField] private TextMeshProUGUI _stageRightCornerText;

    [Header("Snow")] [SerializeField] private GameObject _bigSnow;
    [SerializeField] private Material _groundSnow;
    [SerializeField] private float _snowHeight;

    private float _actualChrono;
    private float _chronoReduced;
    private int _countObj;
    private int _actualLevel;

    private bool _hasLost;
    private bool _hasWin;
    private bool _blockChrono;
    private bool _isFirstChrono;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _isFirstChrono = true;
        _chronoReduced = _firstChrono;
        StartScene();

        _groundSnow.SetFloat("_SnowHeight", _snowHeight);
    }

    public void StartScene()
    {
        _bigSnow.SetActive(false);
        if (_isFirstChrono)
        {
            _actualChrono = _firstChrono;
            _isFirstChrono = false;
        }
        else
            _chronoReduced *= 1 - _chronoReducePercent * .01f;

        _actualChrono = _chronoReduced;

        foreach (var obj in _randomObj)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        int random = Random.Range(0, _randomObj.Length);
        if (_randomObj[random] != null)
            _randomObj[random].SetActive(true);

        _hasLost = false;
        _hasWin = false;

        _stageRightCornerText.text = $"Stage : {_actualLevel + 1}";
        _countObj = 0;
        _chronoText.color = Color.white;
        UpdateChrono();
        _player.transform.position = _spawnPointPlayer.position;

        foreach (var obj in _objects)
        {
            obj.GetComponent<RandomPosSpawn>().ChoosePosition();
        }
    }

    private void ChangeNextLevel()
    {
        _hasWin = true;
        _actualLevel++;
        BlockOrNotChrono(false);

        TransiManager.Instance.LaunchCongrats();
    }

    private void UpdateChrono()
    {
        TimeSpan time = TimeSpan.FromMinutes(_actualChrono);
        var format = time.ToString(@"hh\:mm\:ss");
        format = format.Substring(3, 5);
        _chronoText.text = format;

        if (_actualChrono < 5)
            _chronoText.color = Color.red;
    }

    private void Update()
    {
        if (_actualChrono > 0)
        {
            if (!_blockChrono) return;

            _actualChrono -= Time.deltaTime;
            UpdateChrono();
        }
        else
        {
            if (_hasLost || _hasWin) return;

            _hasLost = true;
            if (ShakeObj.Instance != null)
                ShakeObj.Instance.StartShakingCam(0);

            _stageRightCornerText.DOFade(0, 1);
            _bigSnow.SetActive(true);
            StartCoroutine(IncreaseSnowHeight());
            TransiManager.Instance.LaunchFail();
        }
    }

    IEnumerator IncreaseSnowHeight()
    {
        yield return new WaitForSeconds(2f);

        var bigSnowHeigt = _snowHeight;
        for (float i = _snowHeight; i < .8f; i += .01f)
        {
            bigSnowHeigt += .01f;
            _groundSnow.SetFloat("_SnowHeight", bigSnowHeigt);
            yield return new WaitForSeconds(.05f);
        }
    }

    public void ObjectGet(int index)
    {
        _objectsUI[index].GetComponent<Image>().DOFade(1, 0);
        _objectsUI[index].SetActive(true);
        _countObj++;

        if (_countObj >= 3 && !_hasLost)
            ChangeNextLevel();
    }

    public int GetActualLevel()
    {
        return _actualLevel;
    }

    public void BlockOrNotChrono(bool yesOrNot)
    {
        _blockChrono = yesOrNot;
    }
}