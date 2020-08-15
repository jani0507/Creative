using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlane : MonoBehaviour
{
    private Transform playerTramsform;

    private void Start()
    {
        playerTramsform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.position = Vector3.forward * playerTramsform.position.z;
    }
}
