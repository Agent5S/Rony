using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _health;
    private void Awake()
    {
        _health = 100f;
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _health -= other.gameObject.GetComponent<EnemyController>().Instance.DamageDealt;
        }
    }
}
