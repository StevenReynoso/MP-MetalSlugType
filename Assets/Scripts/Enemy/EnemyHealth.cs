using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int Damage)
    {
        currentHealth -= Damage;
    }

    private void Die()
    {
        Destroy(gameObject);
        EnemyDiedServerRpc();
    }

    [ServerRpc]
    private void EnemyDiedServerRpc()
    {
        // Call a client RPC to notify all clients that the enemy has died
        EnemyDiedClientRpc();
    }

    [ClientRpc]
    private void EnemyDiedClientRpc()
    {
        // Handle the enemy dying on the client
        Debug.Log("Enemy died!");
    }

}
