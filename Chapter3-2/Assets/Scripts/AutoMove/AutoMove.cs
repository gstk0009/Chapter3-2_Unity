using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Transform movePosition;
    private float moveSpeed = 5;
    private bool isplayerRide = false;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        movePosition = GetComponent<Transform>();
        rigidBody.velocity = Vector3.left * moveSpeed;
    }

    private void FixedUpdate()
    {
        if (rigidBody.velocity.magnitude != moveSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * moveSpeed;
        }

        if (movePosition.localPosition.x < -42f)
        {
            rigidBody.velocity = Vector3.right * moveSpeed;
        }
        else if (movePosition.localPosition.x > 1f)
        {
            rigidBody.velocity = Vector3.left * moveSpeed;
        }
    }
}
