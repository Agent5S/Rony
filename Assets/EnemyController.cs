using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    throwing,
    attacking,
    dead
}

public class EnemyController : MonoBehaviour
{
    [HideInInspector] public EnemyController Instance;
    public float DamageDealt
    {
        get { return _damageDealt; }
    }

    public EnemyState _currentState { get; set; }

    [Header("Components")]
    private GameObject _player;
    private Collider2D _hitboxCollider;


    [Header("Movement variables")]
    [SerializeField] private float _speed;

    [Header("Attack variables")]
    [SerializeField] private float _damageDealt;

    private void Start()
    {
        Instance = this;
        _currentState = EnemyState.idle;
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (_currentState == EnemyState.attacking)
        {
            transform.Translate(new Vector2(_player.transform.position.x - transform.position.x, 0f).normalized * _speed * Time.deltaTime);
        }

        if (_currentState == EnemyState.dead)
        {
            StartCoroutine(KillEnemy());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _currentState == EnemyState.idle)
        {
            _currentState = EnemyState.throwing;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        _currentState = EnemyState.throwing;
        //play throwing animation 
        yield return new WaitForSeconds(2);
        _currentState = EnemyState.attacking;
        //play animation running to atack player
    }

    private IEnumerator KillEnemy()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(2);
    }


}
