using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private int speed;
    private float _speedMultiplier;
    
    [Range(1,10)]
    [SerializeField] private float acceleration;

    private bool _btnPressed;

    private bool _isWallTouch;
    public LayerMask wallLayer;
    public Transform wallCheckPoint;

    private Vector2 _relativeTransform;

    private void Start()
    {
        UpdateRelativeTransform();
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateSpeedMultiplier();
        
        float targetSpeed = speed * _speedMultiplier * _relativeTransform.x;
        _rb.velocity = new Vector2(targetSpeed, _rb.velocity.y);

        _isWallTouch = Physics2D.OverlapBox(wallCheckPoint.position, new Vector2(0.03f, 0.7f), 0, wallLayer);

        if (_isWallTouch)
        {
            Flip();
        }
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        UpdateRelativeTransform();
    }

    void UpdateRelativeTransform()
    {
        _relativeTransform = transform.InverseTransformVector(Vector2.one);
    }

    public void Move(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _btnPressed = true;
        }
        else if (value.canceled)
        {
            _btnPressed = false;
        }
    }

    void UpdateSpeedMultiplier()
    {
        if (_btnPressed && _speedMultiplier < 1)
        {
            _speedMultiplier += Time.deltaTime * acceleration;
        }
        else if (!_btnPressed && _speedMultiplier > 0)
        {
            _speedMultiplier -= Time.deltaTime * acceleration;
            if (_speedMultiplier < 0)
            {
                _speedMultiplier = 0;
            }
        }
    }
}
