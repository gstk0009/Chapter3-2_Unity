using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    private float maxDistance = 50f;
    public LayerMask layerMask;
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.left*maxDistance);
        if (Physics.Raycast(transform.position, Vector3.left, maxDistance, layerMask))
        {
            // 맞으면 돌이나 당근 떨어트리기 (Interactable이 불가능한 Object로 생성)
            Debug.Log("Player 맞음");
        }
    }
}
