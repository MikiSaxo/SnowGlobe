using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    public static PlayerCam Instance;

    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;

    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _camHolder;

    private float _xRotation;
    private float _yRotation;
    private bool _canMove;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateMove(bool moveOrNot)
    {
        _canMove = moveOrNot;
    }

    private void Update()
    {
        if (!_canMove) return;
        
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensY;

        _yRotation += mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        // rotate cam and orientation
        _camHolder.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}