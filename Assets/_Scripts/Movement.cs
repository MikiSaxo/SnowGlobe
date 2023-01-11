using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 0f;
    [SerializeField] private Rigidbody _rb;

    private Vector3 direction = Vector3.zero;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, 0,vertical);
    }

    private void FixedUpdate()
    {
        Vector3 newPos = _rb.position + direction * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);
    }
}