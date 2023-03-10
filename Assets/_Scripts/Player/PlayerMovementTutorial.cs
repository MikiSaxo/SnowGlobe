using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementTutorial : MonoBehaviour
{
    public static PlayerMovementTutorial Instance;
    
    [Header("Movement Normal")]
    [SerializeField] private float _moveSpeedNormal;
    [SerializeField] private float _groundDragNormal;
    private float _jumpForce;
    private float _jumpCooldown;
    private float _airMultiplier;

    [Header("Keybinds")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    // [SerializeField] private KeyCode _sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _orientation;
    public AudioManager audioManager;

    private float _moveSpeed;
    private float _groundDrag;
    
    private bool _readyToJump;
    private bool _grounded;
    private bool _canMove;
    
    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;

    private Rigidbody _rb;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _moveSpeed = _moveSpeedNormal;
        _groundDrag = _groundDragNormal;
        _readyToJump = true;
    }
    
    public void UpdateMove(bool moveOrNot)
    {
        _canMove = moveOrNot;
    }

    public void BlockRb()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Update()
    {
        // ground check
        _grounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.3f, _whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (_grounded)
            _rb.drag = _groundDrag;
        else
            _rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if(_canMove)
            MovePlayer();
    }

    private void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(_jumpKey) && _readyToJump && _grounded)
        {
            _readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), _jumpCooldown);
        }

        // if (Input.GetKey(_sprintKey))
        // {
        //     _moveSpeed = _moveSpeedSprint;
        //     _groundDrag = _groundDragSprint;
        // }
        // else
        // {
        //     _moveSpeed = _moveSpeedNormal;
        //     _groundDrag = _groundDragNormal;
        // }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        _moveDirection = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        // on ground
        if (_grounded)
        {
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        }


        // in air
        else if (!_grounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        _readyToJump = true;
    }
}