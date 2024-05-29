using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ItemType
{
    Equiable,
    BuffItemConsumable,
    Button
}

public enum BuffItemType
{
    Speed,
    Health,
    Jump,
    ApplyingTime
}

[Serializable]
public class BuffItemData
{
    public BuffItemType BuffType;
    public float value;
}

[Serializable]
public class BuffItemDataConsumable
{
    public BuffItemType BuffType;
    public float value;
}


[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string DisplayName;
    public string Description;
    public ItemType Itemtype;
    public Sprite Icon;
    public GameObject DropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Equip")]
    public GameObject EquipPrefab;

    [Header("BuffItem")]
    public BuffItemData[] BuffItems;

    [Header("BuffItemConsumable")]
    public BuffItemDataConsumable[] Consumables;

    [Header("Button")]
    public bool canPush;
}
