using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerInput _playerInput;

    private float _moveSpeed = 4f;
    private float _rotateSpeed = 120f;

    private float _moveForwardDirection = 0f;
    private float _moveSideDirection = 0f;
    private float _rotateDirection = 0f;
    private bool _isSpaceBarDown = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isSpaceBarDown = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _isSpaceBarDown = false;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _moveForwardDirection += 1f;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            _moveForwardDirection -= 1f;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _moveForwardDirection -= 1f;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            _moveForwardDirection += 1f;
        }

        if (_isSpaceBarDown)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _rotateDirection -= 1f;
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                _rotateDirection += 1f;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _rotateDirection += 1f;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                _rotateDirection -= 1f;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _moveSideDirection -= 1f;
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                _moveSideDirection += 1f;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _moveSideDirection += 1f;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                _moveSideDirection -= 1f;
            }
        }

        if (_moveForwardDirection != 0f)
        {
            transform.position += transform.forward * _moveForwardDirection * _moveSpeed * Time.deltaTime;
        }

        if (_moveSideDirection != 0f)
        {
            transform.position += transform.right * _moveSideDirection * _moveSpeed * Time.deltaTime;
        }

        if (_rotateDirection != 0f)
        {
            var y = transform.localEulerAngles.y + _rotateDirection * _rotateSpeed * Time.deltaTime;

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
        }
    }
}
