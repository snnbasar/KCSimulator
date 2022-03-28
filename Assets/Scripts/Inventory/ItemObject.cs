using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    public InventoryItem inventoryItem;
    public bool AmIInInventory;
    public ItemSlot slot;

    public List<ItemRequirement> requirements;
    public bool removeRequirementsOnPickup;
    public void OnHandlePickupItem()
    {
        if (MeetsRequirements())
        {
            if (removeRequirementsOnPickup)
            {
                RemoveRequirements();
            }
            InventorySystem.instance.Add(referenceItem, this);
            //Destroy(gameObject);
        }
    }

    private bool MeetsRequirements()
    {
        foreach (ItemRequirement requirement in requirements)
        {
            if (!requirement.HasRequirement()) { return false; }
        }
        return true;
    }

    private void RemoveRequirements()
    {
        foreach (ItemRequirement requirement in requirements)
        {
            for (int i = 0; i < requirement.amount; i++)
            {
                //InventorySystem.instance.Remove(requirement.itemData);
            }
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }



    [Serializable]
    public struct ItemRequirement
    {
        public InventoryItemData itemData;
        public int amount;
        public bool HasRequirement()
        {
            InventoryItem item = InventorySystem.instance.Get(itemData);
            if (item == null || item.stackSize < amount) { return false; }
            return true;
        }
    }
}


