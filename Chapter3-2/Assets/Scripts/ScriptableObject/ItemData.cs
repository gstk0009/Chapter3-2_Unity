using System;
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
    Jump
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
    public float ApplyingTime;
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
