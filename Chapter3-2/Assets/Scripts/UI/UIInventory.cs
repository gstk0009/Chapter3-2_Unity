using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectItemName;
    public TextMeshProUGUI selectItemDescription;
    public TextMeshProUGUI selectStatName;
    public TextMeshProUGUI selectStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipIndex;

    private void Start()
    {
        controller = PlayerManager.Instance.Player.controller;
        condition = PlayerManager.Instance.Player.condition;
        dropPosition = PlayerManager.Instance.Player.dropPosition;

        controller.Inventory += Toggle;
        PlayerManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;

        }
    }

    private void ClearSelectedItemWindow()
    {
        selectItemName.text = string.Empty;
        selectItemDescription.text = string.Empty;
        selectStatName.text = string.Empty;
        selectStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    private void Toggle()
    {
        if (IsOpen())
            inventoryWindow.SetActive(false);
        else
            inventoryWindow.SetActive(true);
    }

    private bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    private void AddItem()
    {
        ItemData data = PlayerManager.Instance.Player.itemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                PlayerManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            PlayerManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data);
        PlayerManager.Instance.Player.itemData = null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    private void ThrowItem(ItemData data)
    {
        Instantiate(data.DropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectItemName.text = selectedItem.DisplayName;
        selectItemDescription.text = selectedItem.Description;

        selectStatName.text = string.Empty;
        selectStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.Consumables.Length; i++)
        {
            BuffItemDataConsumable selectItem = selectedItem.Consumables[i];

            selectStatName.text += selectItem.BuffType.ToString() + "\n";
            selectStatValue.text += selectItem.value.ToString() + "\n";

            if (selectItem.ApplyingTime != 0)
            {
                selectStatName.text += "ApplyingTime\n";
                selectStatValue.text += selectItem.ApplyingTime.ToString() + "\n";
            }
        }

        for (int i = 0; i < selectedItem.BuffItems.Length; i++)
        {
            selectStatName.text += selectedItem.BuffItems[i].BuffType.ToString() + "\n";
            selectStatValue.text += selectedItem.BuffItems[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.Itemtype == ItemType.BuffItemConsumable);
        equipButton.SetActive(selectedItem.Itemtype == ItemType.Equiable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.Itemtype == ItemType.Equiable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem.Itemtype == ItemType.BuffItemConsumable)
        {
            for (int i = 0; i < selectedItem.Consumables.Length; i++)
            {
                BuffItemDataConsumable selectItem = selectedItem.Consumables[i];
                switch (selectItem.BuffType)
                {
                    case BuffItemType.Speed:
                        if (!PlayerManager.Instance.Player.condition._isUsedItemSpeed)
                        {
                            PlayerManager.Instance.Player.controller.UseConsumableItemSpeed = selectItem.value;
                            PlayerManager.Instance.Player.condition.GetUsedItemSpeed(true, selectItem.ApplyingTime);
                        }
                        else
                        {
                            Debug.Log("스피드 아이템 사용중");
                        }
                        break;
                    case BuffItemType.Jump:
                        if (!PlayerManager.Instance.Player.condition._isUsedItemJump)
                        {
                            PlayerManager.Instance.Player.controller.UseConsumableJump = selectItem.value;
                            PlayerManager.Instance.Player.condition.GetUsedItemJump(true, selectItem.ApplyingTime);
                        }
                        else
                        {
                            Debug.Log("점프 아이템 사용중");
                        }
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        SetEquipItemData();
        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        PlayerManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    private void UnEquip(int index)
    {
        SetUnEquipItemData();
        slots[index].equipped = false;
        PlayerManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

    private void SetUnEquipItemData()
    {
        for (int i = 0; i < slots[curEquipIndex].item.BuffItems.Length; i++)
        {
            switch (slots[curEquipIndex].item.BuffItems[i].BuffType)
            {
                case BuffItemType.Speed:
                    PlayerManager.Instance.Player.controller.EquipItemSpeed = 0;
                    break;
                case BuffItemType.Jump:
                    PlayerManager.Instance.Player.controller.EquipItemJump = 0;
                    break;
            }
        }
    }

    private void SetEquipItemData()
    {
        for (int i = 0; i < slots[selectedItemIndex].item.BuffItems.Length; i++)
        {
            switch (slots[selectedItemIndex].item.BuffItems[i].BuffType)
            {
                case BuffItemType.Speed:
                    PlayerManager.Instance.Player.controller.EquipItemSpeed = slots[selectedItemIndex].item.BuffItems[i].value;
                    break;
                case BuffItemType.Jump:
                    PlayerManager.Instance.Player.controller.EquipItemJump = slots[selectedItemIndex].item.BuffItems[i].value;
                    break;
            }
        }
    }
}
