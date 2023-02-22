using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.VersionControl;
using UnityEngine;

public class HealthUpdateMessage : NetworkBehaviour
{
    public int health;

    public HealthUpdateMessage() { }

    public HealthUpdateMessage(int health)
    {
        this.health = health;
    }
}
