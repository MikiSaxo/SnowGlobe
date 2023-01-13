using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class TransiManager : MonoBehaviour
{
    public static TransiManager Instance;

    [Header("Fade")] [SerializeField] private Image _fade;
    [SerializeField] private float _timeFadeOn;
    [SerializeField] private float _timeFadeOff;
    [Header("Transi")] [SerializeField] private TextMeshProUGUI _instructionsText;
    [SerializeField] private GameObject[] _iconsObj;
    [SerializeField] private GameObject[] _iconsObjBG;
    [SerializeField] private TextMeshProUGUI _goTxt;
    [SerializeField] private TextMeshProUGUI _congratTxt;
    [SerializeField] private GameObject _congrat;
    [SerializeField] private GameObject _fail;
    [SerializeField] private TextMeshProUGUI _failText;
    [SerializeField] private TextMeshProUGUI _stageText;
    [SerializeField] private float _timeToMoveIcon;
    [SerializeField] private float _timeGoFade;
    [SerializeField] private float _timeCongratsOff;

    private List<Vector3> _startPosIconsObj = new List<Vector3>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _fade.gameObject.SetActive(true);
        _fade.DOFade(0, _timeFadeOff).OnComplete(DeactiveFade);

        GetObjStartPos();
        LaunchMoveIcon();
    }
    
    private void DeactiveFade()
    {
        ActiveOrNotFade(false);
    }
    
    private void ActiveOrNotFade(bool which)
    {
        _fade.gameObject.SetActive(which);
    }

    private void GetObjStartPos()
    {
        foreach (var icon in _iconsObj)
        {
            _startPosIconsObj.Add(icon.transform.position);
        }
    }

    public void LaunchMoveIcon()
    {
        AudioManager.Instance.PlaySound("Decompte ");
        StartCoroutine(MoveIcon());
    }

    private IEnumerator MoveIcon()
    {
        foreach (var icon in _iconsObj) // Security
        {
            icon.GetComponent<Image>().DOFade(1, 1f);
        }
        
        _fade.DOFade(0, _timeFadeOff).OnComplete(DeactiveFade);
        
        PlayerCam.Instance.UpdateMove(false);
        PlayerMovementTutorial.Instance.UpdateMove(false);

        yield return new WaitForSeconds(_timeToMoveIcon * 2);
        for (int i = 0; i < _iconsObj.Length; i++)
        {
            _iconsObj[i].transform.DOMove(_iconsObjBG[i].transform.position, 1);
            _iconsObj[i].transform.DOScale(Vector3.one, 1);
            yield return new WaitForSeconds(_timeToMoveIcon);
        }

        _goTxt.gameObject.SetActive(true);
        _goTxt.DOFade(1, 0);
        yield return new WaitForSeconds(_timeToMoveIcon);

        foreach (var icon in _iconsObj)
        {
            icon.GetComponent<Image>().DOFade(0, 1f);
        }

        _goTxt.DOFade(0, _timeGoFade);
        _instructionsText.DOFade(0, _timeGoFade);

        PlayerCam.Instance.UpdateMove(true);
        PlayerMovementTutorial.Instance.UpdateMove(true);
        Manager.Instance.BlockOrNotChrono(true);
    }

    public void ResetPos()
    {
        for (int i = 0; i < _iconsObj.Length; i++)
        {
            _iconsObj[i].transform.DOMove(_startPosIconsObj[i], 0);
            _iconsObj[i].transform.DOScale(1.5f, 0);
            _instructionsText.DOFade(1, 0);
        }
    }

    public void LaunchFail()
    {
        StartCoroutine(Fail());
    }

    IEnumerator Fail()
    {
        PlayerCam.Instance.UpdateMove(false);
        PlayerMovementTutorial.Instance.UpdateMove(false);
        PlayerMovementTutorial.Instance.BlockRb();
        
        yield return new WaitForSeconds(1f);
        _failText.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        _stageText.text = $"Stage : {Manager.Instance.GetActualLevel()+1}";
        _stageText.DOFade(1, 1);
        _fail.SetActive(true);

        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LaunchCongrats()
    {
        _congrat.SetActive(true);
        AudioManager.Instance.PlaySound("Win");
        PlayerCam.Instance.UpdateMove(false);
        PlayerMovementTutorial.Instance.UpdateMove(false);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void MoreDifficult()
    {
        StartCoroutine(MoreDifficultAnim());
    }

    IEnumerator MoreDifficultAnim()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem .GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _fade.gameObject.SetActive(true);
        _fade.DOFade(1, _timeFadeOn);
        yield return new WaitForSeconds(_timeFadeOn);
        
        _congrat.SetActive(false);
        Manager.Instance.StartScene();
        
        ResetPos();
        LaunchMoveIcon();
    }

    public void RestartButton()
    {
        _fade.gameObject.SetActive(true);
        _fade.DOFade(1, _timeFadeOn).OnComplete(GoRestart);
    }

    private void GoRestart()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitMenu()
    {
        ActiveOrNotFade(true);
        _fade.DOFade(1, _timeFadeOn).OnComplete(GoToMenu);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}