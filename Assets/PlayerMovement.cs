using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float _acceleration;
    public float _maxSpeed;

    private Rigidbody2D _rigidBody;
    private SpriteRenderer _renderer;
    private float _facingDirection = 1;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var velocity = _rigidBody.velocity;
        var xVel = velocity.x;

        var input = Input.GetAxisRaw("Horizontal");
        var accel = input * _acceleration * Time.deltaTime;
        xVel += accel;
        xVel = Mathf.Clamp(xVel, -_maxSpeed, _maxSpeed);

        velocity.x = xVel;
        _rigidBody.velocity = velocity;

        _facingDirection = input != 0 ? Mathf.Sign(input) : _facingDirection;
        _renderer.flipX = _facingDirection == -1;
    }
}
