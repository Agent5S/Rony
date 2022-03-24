using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasePlayer : MonoBehaviour
{
    public float enableDelay;
    public float speed;
    public float minSeparation;

    private Rigidbody2D _rigidbody;
    private WaitForSeconds _enableDelay;

    private Rigidbody2D _target;
    public Rigidbody2D target
    {
        get => _target;
        set
        {
            if (value == _target) { return; }
            _target = value;
            if (value) { StartCoroutine(Enable()); }
            else { enabled = false; }
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _enableDelay = new WaitForSeconds(enableDelay);
        enabled = target != null;
    }

    private void Update()
    {
        var velocity = _rigidbody.velocity;
        var diff = target.position.x - _rigidbody.position.x;
        if (Mathf.Abs(diff) < minSeparation) { return; }
        var direction = System.Math.Sign(diff);
        velocity.x = direction * speed;
        _rigidbody.velocity = velocity;
    }

    private IEnumerator Enable()
    {
        //TODO: Throw?
        yield return _enableDelay;
        if (target)
        {
            enabled = true;
        }
    }
}
