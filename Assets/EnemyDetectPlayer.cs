using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyChasePlayer))]
public class EnemyDetectPlayer : MonoBehaviour
{
    public float collisionTimeout;
    public float radius;
    public LayerMask layerMask;

    public WaitForSeconds _collisionTimeout;
    private EnemyChasePlayer _chase;
    private RaycastHit2D _hit;

    private void Awake()
    {
        _chase = GetComponent<EnemyChasePlayer>();
        _collisionTimeout = new WaitForSeconds(collisionTimeout);
    }

    private void FixedUpdate()
    {
        var origin = transform.position;
        _hit = Physics2D.CircleCast(origin, radius, Vector2.zero, 0, layerMask);
        _chase.target = _hit.rigidbody;
    }

    public void Timeout()
    {
        enabled = false;
        StartCoroutine(Enable());
    }

    private void OnDisable()
    {
        _chase.target = null;
    }

    private IEnumerator Enable()
    {
        yield return _collisionTimeout;
        enabled = true;
    }
}
