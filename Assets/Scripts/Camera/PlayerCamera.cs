using UnityEngine;
using Cinemachine;
using Unity.Netcode;
using Unity.VisualScripting;

public class PlayerCamera : NetworkBehaviour
{
    private CinemachineTargetGroup targetGroup;
    private Transform playerTransform;

    public override void OnNetworkSpawn()
    {
        // Get a reference to the Cinemachine Target Group
        targetGroup = GameObject.FindGameObjectWithTag("TargetGroup").GetComponent<CinemachineTargetGroup>();

        // Get a reference to the player transform
        playerTransform = transform;

        // Add the player to the list of targets for the Target Group
        CinemachineTargetGroup.Target playerTarget = new CinemachineTargetGroup.Target();
        playerTarget.target = playerTransform;
        // adding target to group and setting weights and radius
        targetGroup.AddMember(playerTarget.target, 1, 1);

    }
}
