using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAimWeapon : NetworkBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    private float destroyBullet = 2.5f;
    public float timeBetweenFiring;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        // Get the mouse position in world space
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the rotation of the player based on the mouse position
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        // If the player can fire and the left mouse button is pressed, fire a shot
        if (canFire && Input.GetMouseButton(0))
        {
            canFire = false;

            // Pass the mouse position to ShotsFiredServerRpc
            Vector3 direction = mousePos - bulletTransform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            ShotsFiredServerRpc(angle);
        }

        // If the player can't fire, increment the timer and reset canFire if necessary
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenFiring)
            {
                canFire = true;
                timer = 0f;
            }
        }
    }

    [ServerRpc]
    private void ShotsFiredServerRpc(float rotation)
    {
        ShotsFiredClientRpc(rotation);
    }

    [ClientRpc]
    private void ShotsFiredClientRpc(float rotation)
    {
        // Create a new bullet and set its rotation based on the passed in rotation
        GameObject bulletCopy = Instantiate(bullet, bulletTransform.position, Quaternion.identity);
        bulletCopy.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

        // Destroy the bullet after a set amount of time
        Destroy(bulletCopy, destroyBullet);
    }




    [ServerRpc]
    private void SendMousePositionServerRpc(Vector3 mousePosition)
    {
        SendMousePositionClientRpc(mousePosition);
    }

    [ClientRpc]
    private void SendMousePositionClientRpc(Vector3 mousePosition)
    {
        mousePos = mousePosition;
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            SendMousePositionServerRpc(mousePos);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        }
    }


}
