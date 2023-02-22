using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
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
        anim.SetBool("Die", true);
        Destroy(gameObject);
        PlayerDiedServerRpc();
    }

    [ServerRpc]
    private void PlayerDiedServerRpc()
    {
        // Call a client RPC to notify all clients that the enemy has died
        PlayerDiedClientRpc();
    }

    [ClientRpc]
    private void PlayerDiedClientRpc()
    {
        // Handle the enemy dying on the client
        Debug.Log("Player died!");
    }

   

}
