using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectInfoUI : MonoBehaviour
{
    public static ObjectInfoUI instance;

    public GameObject karincaHubInfoUI;
    public List<TextMeshProUGUI> karincaHubInfoTextler = new List<TextMeshProUGUI>();

    private KarincaHub karincaHub;
    private ItemObject itemObject;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ClearTexts();
        ToggleKarincaHub(false);
    }

    private void ClearTexts()
    {
        foreach (TextMeshProUGUI text in karincaHubInfoTextler)
        {
            text.text = "";
        }
    }

    public void ToggleKarincaHub(bool sts)
    {
        switch (sts)
        {
            case true:

                ClearTexts();
                karincaHubInfoUI.SetActive(true);

                break;
            case false:
                karincaHubInfoUI.SetActive(false);
                karincaHub = null;
                break;
        }
    }
    public void SetKarincaHubObjectInfo(KarincaHub hub)
    {
        ToggleKarincaHub(true);
        itemObject = hub.GetComponent<ItemObject>();
        karincaHub = hub;
    }
    private void Update()
    {
        if (karincaHub)
        {
            karincaHubInfoTextler[0].text = itemObject.referenceItem.displayName;
            karincaHubInfoTextler[1].text = karincaHub.karincaSayisi.ToString();
            karincaHubInfoTextler[2].text = (karincaHub.currentAclik * 100).ToString();
            karincaHubInfoTextler[3].text = (karincaHub.currentPislik).ToString();
        }
    }
}
