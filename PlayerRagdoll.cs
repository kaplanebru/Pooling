using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    private Rigidbody[] rigidBodies;
    void Start()
    {
        Player.OnEnableRagdoll += EnableRagdoll;
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        EnableRagdoll(true, false);
    }
    
    void EnableRagdoll(bool isKinematic, bool useGravity)
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = isKinematic;
            rigidBody.useGravity = useGravity;
        }
    }

    private void OnDisable()
    {
        Player.OnEnableRagdoll -= EnableRagdoll;
    }
}
