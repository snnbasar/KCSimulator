using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketUI : MonoBehaviour
{
    public static MarketUI instance;
    public bool marketToggle;
    public GameObject marketUIObject;
    public List<GameObject> paneller = new List<GameObject>();
    public List<MarketSlotUI> slotlar = new List<MarketSlotUI>();


    [SerializeField]
    private Sprite idle_icon;
    [SerializeField]
    private Sprite pressed_icon;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ToogleMarketUI();
    }

    public void ToogleMarketUI()
    {
        if (marketToggle)
        {
            OpenInventoryUI(true);
        }
        if (!marketToggle)
        {
            OpenInventoryUI(false);
        }

        marketToggle = !marketToggle;
    }

    private void OpenInventoryUI(bool sts)
    {
        switch (sts)
        {
            case true:
                marketUIObject.SetActive(true);
                GameManager.instance.IsMarketOpen = true;
                GameManager.instance.CanControlPlayer(false);
                GameManager.instance.CanUseCursor(true);
                break;
            case false:
                marketUIObject.SetActive(false);
                GameManager.instance.IsMarketOpen = false;
                GameManager.instance.CanControlPlayer(true);
                GameManager.instance.CanUseCursor(false);
                break;
        }
    }

    public void OnItemButtonEnter(MarketSlotUI slot)
    {
        ReloadSlots();
        slot.backGround.sprite = pressed_icon;
    }
    public void OnItemButtonClicked(MarketSlotUI slot)
    {
        ReloadSlots();
        slot.backGround.sprite = pressed_icon;
        CheckForPanel(slot);
        CheckForBuyable(slot);
        CheckForSellable(slot);

    }
    public void OnItemButtonExit(MarketSlotUI slot)
    {
        ReloadSlots();
        slot.backGround.sprite = idle_icon;
    }

    public void ReloadSlots()
    {
        foreach (MarketSlotUI slot in slotlar)
        {
            slot.backGround.sprite = idle_icon;
        }
    }

    private void CheckForPanel(MarketSlotUI slot)
    {
        if (slot.panel)
        {
            ClosePanels();
            switch (slot.id)
            {
                case 0: //hayvanlar
                    paneller[0].SetActive(true);
                    break;
                case 1: //craft
                    paneller[1].SetActive(true);
                    break;
                case 2: //yiyecek
                    paneller[2].SetActive(true);
                    break;
                case 3: //sell
                    paneller[3].SetActive(true);
                    break;

            }
        }
    }

    private void ClosePanels()
    {
        foreach (GameObject panel in paneller)
        {
            panel.SetActive(false);
        }
    }


    private void CheckForBuyable(MarketSlotUI slot)
    {
        if (slot.market)
        {
            if (!CheckIfPlayerCanEfford(slot))
                return;
            Economy.instance.ParaAzalt(slot.itemPrefab.alisFiyati, slot.itemPrefab.displayName);
            GameManager.instance.SatinAlinaniSpawnla(slot.itemPrefab.prefab);
        }
    }

    private bool CheckIfPlayerCanEfford(MarketSlotUI slot)
    {
        if (Economy.instance.para >= slot.itemPrefab.alisFiyati)
            return true;
        else
            return false;
    }

    InventoryItem satilacakItem;

    private void CheckForSellable(MarketSlotUI slot)
    {
        if (slot.sell)
        {
            if (!CheckIfPlayerHasItem(slot))
                return;

            Economy.instance.ParaArttir(satilacakItem.data.satisFiyati, satilacakItem.data.displayName);
            InventorySystem.instance.Delete(satilacakItem.data, satilacakItem);
        }
    }
    private bool CheckIfPlayerHasItem(MarketSlotUI slot)
    {
        bool hasitem = false;
        foreach (InventoryItem item in InventorySystem.instance.Inventory)
        {
            if (item.data.id == slot.id)
            {
                hasitem = true;
                satilacakItem = item;
            }
        }

        return hasitem;
    }
}
