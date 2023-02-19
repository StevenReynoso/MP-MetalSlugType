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
        if(!IsOwner) return;

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            ShotsFiredServerRpc();
        }
    }

    [ServerRpc]
    private void ShotsFiredServerRpc()
    {
        ShotsFiredClientRpc();
    }

    [ClientRpc]
    private void ShotsFiredClientRpc()
    {
        GameObject bulletCopy = Instantiate(bullet, bulletTransform.position, Quaternion.identity);
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
