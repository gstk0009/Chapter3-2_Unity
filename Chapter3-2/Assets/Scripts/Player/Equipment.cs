using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public GameObject curEquip;
    public Transform equipParent;

    private PlayerController controller;
    private PlayerCondition condition;

    private void Start()
    {
        controller = PlayerManager.Instance.Player.controller;
        condition = PlayerManager.Instance.Player.condition;
    }

    public void EquipNew(ItemData data)
    {
        UnEquip();
        curEquip = Instantiate(data.EquipPrefab, equipParent);
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
}
