using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    public int id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
    public bool canStack;
    public int alisFiyati;
    public int satisFiyati;
}
