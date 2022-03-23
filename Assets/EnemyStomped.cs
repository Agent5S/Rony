using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomped : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            transform.parent.GetComponent<EnemyController>().Instance._currentState = EnemyState.dead;
        }
    }
}
