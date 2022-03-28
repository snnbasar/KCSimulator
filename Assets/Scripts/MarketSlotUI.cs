using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MarketSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public MarketUI marketUI;

    public Image backGround;

    public int id;

    public bool panel;
    public bool market;
    public bool sell;
    public InventoryItemData itemPrefab;
    private void Start()
    {
        marketUI = MarketUI.instance;
        backGround = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        marketUI.OnItemButtonEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        marketUI.OnItemButtonClicked(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        marketUI.OnItemButtonExit(this);
    }
}
