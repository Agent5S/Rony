using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomped : MonoBehaviour
{
    public LayerMask layerMask;
    public float maxAngle;
    public float animationLength;
    public float deathJump;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private EnemyDetectPlayer _detect;
    private WaitForSeconds _animationDelay;
    private float _maxContactNormal;
    private ContactPoint2D[] _contacts = new ContactPoint2D[4];

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _detect = GetComponent<EnemyDetectPlayer>();
        _maxContactNormal = -Mathf.Cos(maxAngle * Mathf.Deg2Rad);
        _animationDelay = new WaitForSeconds(animationLength);
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
                StartCoroutine(PlayAnimation());
                return;
            }
        }

        _detect.Timeout();
    }

    private IEnumerator PlayAnimation()
    {
        //TODO: Replace with actual animation
        var velocity = _rigidbody.velocity;
        velocity.y = deathJump;
        _rigidbody.velocity = velocity;
        _collider.enabled = false;

        _detect.enabled = false;
        yield return _animationDelay;
        Destroy(this.gameObject);
    }
}
