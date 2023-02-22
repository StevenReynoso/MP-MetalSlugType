using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerAimWeapon : NetworkBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireDelay = 0.2f;

    private Camera _mainCamera;
    private float _fireTimer;
    private float destroyPrefab = 2.5f;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!IsLocalPlayer)
            return;

        _fireTimer += Time.deltaTime;

        
        if (Input.GetMouseButton(0) && _fireTimer > _fireDelay)
        {
            _fireTimer = 0f;

            // Calculate the mouse position in world coordinates
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            // Calculate the direction of the shot
            Vector3 direction = (mousePosition - _firePoint.position).normalized;

            // Propagate the bullet's movement and collision detection to all clients
            ShootServerRpc(direction);
           
        }
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 direction)
    {
        ShootClientRpc(direction);
    }

    [ClientRpc]
    private void ShootClientRpc(Vector3 direction)
    {
        // Spawn the bullet on the server
        
        GameObject bulletObject = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);

        // Shoot the bullet in the given direction
        BulletScript bulletScript = bulletObject.GetComponent<BulletScript>();
        bulletScript.Shoot(direction);

        Destroy(bulletObject, destroyPrefab);

    }
}