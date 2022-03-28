using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    public int inventoryMaxSize = 16;
    private int inventoryCurrentSize;

    public event Action onInventoryChangedEvent;

    public Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;


    [SerializeField] public List<InventoryItem> Inventory;


    private void Awake()
    {
        instance = this;
        Inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }
    



    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public void Add(InventoryItemData referenceData, ItemObject item)
    {
        if (inventoryCurrentSize < inventoryMaxSize)
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            newItem.connectedObje = item.gameObject;
            Inventory.Add(newItem);
            //m_itemDictionary.Add(referenceData, newItem);
            inventoryCurrentSize++;
            ShowConnectedObject(item.transform, false);
            InventoryUI.instance.AddToInventory(newItem);
            item.inventoryItem = newItem;
            item.AmIInInventory = true;
        }
        onInventoryChangedEvent?.Invoke();
    }

    public void Remove(InventoryItemData referenceData, InventoryItem value)
    {
        /*if (value.stackSize == 0)
        {
            Inventory.Remove(value);
            //m_itemDictionary.Remove(referenceData);
        }*/
        Inventory.Remove(value);
        ItemObject itemObject = value.connectedObje.GetComponent<ItemObject>();
        itemObject.AmIInInventory = false;
        itemObject.slot.Reset();
        itemObject.slot = null;
        inventoryCurrentSize--;
        ShowConnectedObject(value.connectedObje.transform, true);
        onInventoryChangedEvent?.Invoke();

        /*if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            if(referenceData.canStack)
                value.RemoveFromStack();

            Inventory.Remove(value);
            m_itemDictionary.Remove(referenceData);
            ShowConnectedObject(value.connectedObje.transform, true);
        }
        onInventoryChangedEvent?.Invoke();*/
    }
    public void Delete(InventoryItemData referenceData, InventoryItem value)
    {
        /*if (value.stackSize == 0)
        {
            Inventory.Remove(value);
            //m_itemDictionary.Remove(referenceData);
        }*/
        Inventory.Remove(value);
        ItemObject itemObject = value.connectedObje.GetComponent<ItemObject>();
        itemObject.AmIInInventory = false;
        itemObject.slot.Reset();
        itemObject.slot = null;
        inventoryCurrentSize--;
        value.connectedObje.GetComponent<ItemObject>().DestroyMe();
        onInventoryChangedEvent?.Invoke();

        /*if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            if(referenceData.canStack)
                value.RemoveFromStack();

            Inventory.Remove(value);
            m_itemDictionary.Remove(referenceData);
            ShowConnectedObject(value.connectedObje.transform, true);
        }
        onInventoryChangedEvent?.Invoke();*/
    }
    public void ShowConnectedObject(Transform obje, bool visibility)
    {
        switch (visibility)
        {
            case true:
                obje.parent = null;
                obje.position = KarakterController.instance.holdPoint.position;
                obje.gameObject.SetActive(true);
                break;
            case false:
                obje.parent = this.transform;
                obje.position = Vector3.zero;
                obje.rotation = Quaternion.identity;
                obje.gameObject.SetActive(false);
                break;
        }

    }

    public void EnvanterdenCikar(InventoryItem item)
    {
        Remove(item.data, item);
        CheckForBesle(item);
    }

    public void CheckForBesle(InventoryItem item)
    {
        if (item.connectedObje.TryGetComponent<Besin>(out Besin besin))
            BeslemeManager.instance.CheckForBesle(besin);
    }


}


[Serializable]
public class InventoryItem 
{
    [SerializeField]
    public InventoryItemData data;
    [SerializeField]
    public int stackSize;
    [SerializeField]
    public GameObject connectedObje; //Add de setlenir

    public InventoryItem(InventoryItemData source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}
