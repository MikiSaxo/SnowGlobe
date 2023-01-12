using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Image _fade;
    [SerializeField] private float _timeFadeOn;
    [SerializeField] private float _timeFadeOff;

    private void Start()
    {
        ActiveOrNotFade(true);
        _fade.DOFade(0, _timeFadeOff).OnComplete(DeactiveFade);
    }

    private void DeactiveFade()
    {
        ActiveOrNotFade(false);
    }

    private void ActiveOrNotFade(bool which)
    {
        _fade.gameObject.SetActive(which);
    }

    public void StartButton()
    {
        ActiveOrNotFade(true);
        _fade.DOFade(1, _timeFadeOn).OnComplete(GoToMainScene);
    }

    private void GoToMainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}