using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public InventoryUI inventoryUI;
    public Image backGround;
    [SerializeField]
    public InventoryItem inventoryItem;

    public bool dolu;

    [SerializeField]
    private Image m_icon;
    [SerializeField]
    private TextMeshProUGUI m_label;
    [SerializeField]
    private GameObject m_stackObj;
    [SerializeField]
    private TextMeshProUGUI m_stackLabel;

    private void Start()
    {
        inventoryUI = InventoryUI.instance;
        backGround = GetComponent<Image>();
    }
    public void Set(InventoryItem item)
    {
        m_icon.gameObject.SetActive(true);
        m_icon.sprite = item.data.icon;
        m_label.gameObject.SetActive(true);
        m_label.text = item.data.displayName;
        m_stackObj.SetActive(true);
        inventoryItem = item;


        if (item.stackSize <= 1)
        {
            m_stackObj.SetActive(false);
            return;
        }
        m_stackLabel.text = item.stackSize.ToString();
    }

    public void Reset()
    {
        m_icon.sprite = null;
        m_icon.gameObject.SetActive(false);
        m_label.text = "";
        m_label.gameObject.SetActive(false);
        m_stackObj.SetActive(false);
        inventoryItem = null;
        m_stackLabel.gameObject.SetActive(false);
        m_stackLabel.text = "";
        dolu = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryUI.OnItemButtonEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryUI.OnItemButtonClicked(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.OnItemButtonExit(this);
    }
}
