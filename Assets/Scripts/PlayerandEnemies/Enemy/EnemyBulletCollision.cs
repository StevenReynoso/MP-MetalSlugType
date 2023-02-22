using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyBulletCollision : NetworkBehaviour
{

    public int _damage;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet hit an enemy
        if (collision.CompareTag("Player"))
        {
            // Access the enemy's status script and apply the effect
            StatusEffects statusEffects = collision.GetComponent<StatusEffects>();
            statusEffects.ApplyEffect(StatusEffect.Poison, 10f);

            // Access the enemy's health script and deal damage
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            player.TakeDamage(_damage);

            // Destroy the bullet
            NetworkObject.Destroy(gameObject);
        }
    }
}
