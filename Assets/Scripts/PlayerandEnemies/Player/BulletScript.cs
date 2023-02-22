using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class BulletScript : NetworkBehaviour
{
    [SerializeField] private float _speed = 10f;
    private Rigidbody2D _rigidbody;
    private Vector3 _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector3 direction)
    {
        _direction = direction.normalized;
        _rigidbody.velocity = _direction * _speed;
    }

}

