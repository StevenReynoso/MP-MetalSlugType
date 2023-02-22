using System;
using Unity.Netcode;
using UnityEngine;

public class BulletCollisionHandler : NetworkBehaviour
{
    [SerializeField] private int _damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Check if the bullet hit an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Access the enemy's health script and deal damage
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            enemy.TakeDamage(_damage);

            // Destroy the bullet
            //Destroy(gameObject);
            NetworkObject.Destroy(gameObject);
        }
    }
}
