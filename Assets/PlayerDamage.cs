using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public LayerMask layerMask;
    public float minAngle;
    public float damageJump;
    public float damagePush;

    private Rigidbody2D _rigidbody;
    private WaitForSeconds _animationDelay;
    private float _maxContactNormal;
    private ContactPoint2D[] _contacts = new ContactPoint2D[4];
    private int _playerLayer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _maxContactNormal = Mathf.Cos(minAngle * Mathf.Deg2Rad);
        _playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var ignore = ((1 << collision.gameObject.layer) & layerMask) == 0;
        if (ignore) { return; }

        var n = collision.GetContacts(_contacts);
        for (var i = 0; i < n; i++)
        {
            var contact = _contacts[i];
            if (contact.normal.y <= _maxContactNormal)
            {
                var velocity = _rigidbody.velocity;
                velocity.y = damageJump;
                velocity.x = damagePush * Mathf.Sign(contact.normal.x);
                _rigidbody.velocity = velocity;
                return;
            }
        }
    }
}
