using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    [SerializeField] private GameObject m_slotPrefab;



    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    [SerializeField]
    private Sprite idle_icon;
    [SerializeField]
    private Sprite pressed_icon;

    [SerializeField] private GameObject inventoryUIObject;
    [SerializeField] private Transform inventoryPanel;

    bool inventoryToggle;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        InventorySystem.instance.onInventoryChangedEvent += OnUpdateInventory;
        foreach (Transform t in inventoryPanel)
        {
            ItemSlot slot = t.GetComponent<ItemSlot>();
            itemSlots.Add(slot);
            slot.Reset();
        }
        ToogleInventoryUI();
    }
    public void ToogleInventoryUI()
    {
        if (inventoryToggle)
        {
            OpenInventoryUI(true);
        }
        if (!inventoryToggle)
        {
            OpenInventoryUI(false);
        }

        inventoryToggle = !inventoryToggle;
    }

    private void OpenInventoryUI(bool sts)
    {
        switch (sts)
        {
            case true:
                inventoryUIObject.SetActive(true);
                GameManager.instance.IsInventoryOpen = true;
                GameManager.instance.CanControlPlayer(false);
                GameManager.instance.CanUseCursor(true);
                break;
            case false:
                inventoryUIObject.SetActive(false);
                GameManager.instance.IsInventoryOpen = false;
                GameManager.instance.CanControlPlayer(true);
                GameManager.instance.CanUseCursor(false);
                break;
        }
    }
    private void OnUpdateInventory()
    {
        /*foreach (ItemSlot slot in itemSlots)
        {
            if(slot.inventoryItem != null)
                if(slot.inventoryItem.connectedObje == null)
                    slot.Reset();

        }*/
        //itemSlots.Clear();
        //DrawInventory();
    }
    public void DrawInventory()
    {
        foreach (InventoryItem item in InventorySystem.instance.Inventory)
        {
            
        }
    }

    public void AddToInventory(InventoryItem item)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (!itemSlots[i].dolu)
            {
                ItemSlot slot = itemSlots[i];
                slot.Set(item);
                slot.dolu = true;
                item.connectedObje.GetComponent<ItemObject>().slot = slot;
                break;
            }
        }
    }
    /*public void AddInventorySlot(InventoryItem item, int index)
    {
        //GameObject obj = Instantiate(m_slotPrefab);
        //obj.transform.SetParent(transform, false);
        //ItemSlot slot = slot.GetComponent<ItemSlot>();
        if (!itemSlots[index].dolu)
        {
            ItemSlot slot = itemSlots[index];
            slot.Set(item);
            slot.dolu = true;
        }
    }*/

    public void OnItemButtonEnter(ItemSlot slot)
    {
        ReloadSlots();
        if (!slot.dolu)
            return;
        slot.backGround.sprite = pressed_icon;
    }
    public void OnItemButtonClicked(ItemSlot slot)
    {
        ReloadSlots();
        if (!slot.dolu)
            return;
        slot.backGround.sprite = pressed_icon;
        InventorySystem.instance.EnvanterdenCikar(slot.inventoryItem);

        slot.Reset();
    }
    public void OnItemButtonExit(ItemSlot slot)
    {
        ReloadSlots();
        if (!slot.dolu)
            return;
        slot.backGround.sprite = idle_icon;
    }

    public void ReloadSlots()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            slot.backGround.sprite = idle_icon;
        }
    }
}
