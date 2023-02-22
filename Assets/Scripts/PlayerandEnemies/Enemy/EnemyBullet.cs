using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class EnemyBullet : NetworkBehaviour
{
    
    public Transform player;
   
    private Rigidbody2D rb;
    public float force;


    private Quaternion _syncRotation;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        Vector3 direction = player.position - transform.position; // Use player position instead of mouse position
        Vector3 rotation = transform.position - player.position; // Use player position instead of mouse position
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) + Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    [ClientRpc]
    private void SetRotationClientRpc(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    private void Update()
    {
        if (IsServer)
        {
            SetRotationClientRpc(transform.rotation);
        }
    }

}
