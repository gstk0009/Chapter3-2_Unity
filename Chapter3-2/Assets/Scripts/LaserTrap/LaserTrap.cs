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
            // ������ ���̳� ��� ����Ʈ���� (Interactable�� �Ұ����� Object�� ����)
            Debug.Log("Player ����");
        }
    }
}
