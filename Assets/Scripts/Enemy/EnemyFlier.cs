using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyFlier : MonoBehaviour
{
    [SerializeField]
    private GameObject poisonShot;

    [SerializeField]
    private Transform bulletSpawnPos;

    [SerializeField]
    private float minShootWaitTime = 1f, maxShootWaitTime = 3f;

    [SerializeField]
    private float detectionRange = 10f;

    private float waitTime;
    private float destroyObject = 2.5f;

    private List<Transform> playerTransforms = new List<Transform>();

    private void Start()
    {
        waitTime = Time.time + Random.Range(minShootWaitTime, maxShootWaitTime);

        // Find all player transforms and add them to the list
        
    }

    private void Update()
    {

        LoadedLevel();
        
        // Find the closest player transform
        Transform closestPlayerTransform = null;
        float closestDistanceToPlayer = Mathf.Infinity;
        foreach (Transform playerTransform in playerTransforms)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer < closestDistanceToPlayer)
            {
                closestPlayerTransform = playerTransform;
                closestDistanceToPlayer = distanceToPlayer;
            }
        }

        // If no players are found, return
        if (closestPlayerTransform == null)
        {
            return;
        }

        // Shoot at the closest player if they are within range and the wait time has elapsed
        if (closestDistanceToPlayer < detectionRange && Time.time > waitTime)
        {
            waitTime = Time.time + Random.Range(minShootWaitTime, maxShootWaitTime);
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject tempShot;
        tempShot = Instantiate(poisonShot, bulletSpawnPos.position, Quaternion.identity);
        Object.Destroy(tempShot, destroyObject);
    }

    private void LoadedLevel()
    {
        if (playerTransforms.Count == 0)
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject playerObject in playerObjects)
            {
                playerTransforms.Add(playerObject.transform);
            }
        }


    }

  


}
