using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    public static EnemyBulletPool Instance; // Singleton instance of the pool
    public GameObject bulletPrefab; // Prefab for the bullet object
    public int poolSize = 10; // Number of bullets to instantiate initially

    private List<EnemyBullet> bullets; // List of available bullets in the pool

    private void Awake()
    {
        Instance = this;
        bullets = new List<EnemyBullet>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bulletObject = Instantiate(bulletPrefab);
            EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();
            

            bullet.gameObject.SetActive(false);
            bullets.Add(bullet);
        }
    }

    public EnemyBullet GetBullet()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].gameObject.activeSelf)
            {
                bullets[i].gameObject.SetActive(true);
                return bullets[i];
            }
        }

        // If no available bullet found, create a new one and add it to the pool
        GameObject bulletObject = Instantiate(bulletPrefab);
        EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();
        bullets.Add(bullet);
        return bullet;
    }
}
