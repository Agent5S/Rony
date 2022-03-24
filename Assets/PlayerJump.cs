using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpSpeed;
    public float maxGroundAngle;
    public float jumpDrag;
    public float fallGravity;
    public float lowJumpGravity;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private ContactPoint2D[] _contacts = new ContactPoint2D[8];
    private float _minGroundNormal;
    private float _targetGravity;
    private bool _grounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _minGroundNormal = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        _targetGravity = fallGravity;
    }

    private void Update()
    {
        var velocity = _rigidbody.velocity;
        if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                _targetGravity = lowJumpGravity;
            }
        }
        else if (Input.GetButtonDown("Jump") && _grounded)
        {
            _targetGravity = 1;
            velocity.y = jumpSpeed;
            _rigidbody.velocity = velocity;
        }

        _rigidbody.gravityScale = velocity.y < 0 ? fallGravity : _targetGravity;
        _rigidbody.drag = _grounded ? 0 : jumpDrag;
    }

    private void CalculateGrounded()
    {
        _grounded = false;
        var n = _collider.GetContacts(_contacts);
        for (var i = 0; i < n; i++)
        {
            var contact = _contacts[i];
            if (contact.normal.y >= _minGroundNormal)
            {
                _targetGravity = fallGravity;
                _grounded = true;
                break;
            }
        }
    }

    private void OnCollisionEnter2D() { CalculateGrounded(); }

    private void OnCollisionExit2D() { CalculateGrounded(); }
}
