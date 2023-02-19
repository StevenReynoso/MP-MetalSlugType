using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BulletScript : NetworkBehaviour
{

    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    public int damage;

    
    private Quaternion _syncRotation;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) + Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet hit an enemy
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            Debug.Log("Hitting enemy " + enemy.name);
            // Deal damage to the enemy
            enemy.TakeDamage(damage);

            // Destroy the bullet
            Destroy(gameObject);
        }
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
