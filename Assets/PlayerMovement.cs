using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walking,
    jumping
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;

    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _groundLinearDrag;
    private float _horizontalDirection;
    private bool _changingDirection => (_rigidBody.velocity.x > 0f && _horizontalDirection < 0f) || (_rigidBody.velocity.x < 0f && _horizontalDirection > 0f);

    [Header("Jump Variables")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _airLinearDrag;
    [SerializeField] private float _fallMultiplier;
    [SerializeField] private float _lowJumpMultiplier;

    [Header("Ground Collision Variables")]
    [SerializeField] private float _groundBoxcastLength;
    [SerializeField] private bool _isOnGround;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        _horizontalDirection = GetInput().x;
        if (Input.GetButtonDown("Jump") && _isOnGround) Jump();


    }

    private void FixedUpdate()
    {
        CheckCollisions();
        MoveCharacter();
        ApplyGroundLinearDrag();
        if (_isOnGround)
        {
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyFallMultplier();
            ApplyAirLinearDrag();
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        _rigidBody.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);
        if (Mathf.Abs(_rigidBody.velocity.x) > _maxMoveSpeed)
        {
            _rigidBody.velocity = new Vector2(Mathf.Sign(_rigidBody.velocity.x) * _maxMoveSpeed, _rigidBody.velocity.y);
        }
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f)
        {
            _rigidBody.drag = _groundLinearDrag;
        }
        else
        {
            _rigidBody.drag = 0f;
        }
    }
    private void ApplyAirLinearDrag()
    {
        _rigidBody.drag = _airLinearDrag;
    }
    private void Jump()
    {
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0f);
        _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
    private void CheckCollisions()
    {
        _isOnGround = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.down, _groundBoxcastLength, _groundLayer);
    }
    private void ApplyFallMultplier()
    {
        if (_rigidBody.velocity.y < 0)
        {
            _rigidBody.gravityScale = _fallMultiplier;

        }
        else if (_rigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rigidBody.gravityScale = _lowJumpMultiplier;
        }
        else
        {
            _rigidBody.gravityScale = 1f;
        }
    }
}
