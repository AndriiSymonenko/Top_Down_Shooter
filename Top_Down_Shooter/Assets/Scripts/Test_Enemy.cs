using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Enemy : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float EnemySpeed;
    [SerializeField] private float EnemyHealth;
    public float Damage;

    private void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage();
            Debug.Log("Hit");
        }
    }

    private void Update()
    {
        GoToTarget();
        DestroyEnemy();
    }

    void GoToTarget()
    {

        transform.position = Vector3.MoveTowards(transform.position, Target.position, EnemySpeed * Time.deltaTime);
    }

    void TakeDamage()
    {
        EnemyHealth -= Damage;
    }

    void DestroyEnemy()
    {
        if (EnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
