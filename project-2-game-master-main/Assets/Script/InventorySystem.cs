using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static List<GameObject> panels;
    private const int panelnum = 8;
    public List<GameObject> panels_;
    // Just for prototype
    public static List<Interactable> items;
    public static Interactable.GEAR Gear;

    void Awake()
    {
        Gear = Interactable.GEAR.NULL;
        panels = panels_;
        items = new List<Interactable>(panelnum);
        for (int i = 0; i < panelnum; i++) items.Add(null);
        for (int i = 0; i < panelnum; i++) panels[i].GetComponent<InventoryItem>().SetIndex(i);
    }

    public static void AddItem(Interactable item, Sprite image)
    {   for (int i = 0; i < panelnum; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                panels[i].GetComponent<InventoryItem>().Add(image);
                break;
            }
        }   
    }

    public static void Use(int index)
    {
        if (items[index] != null && items[index].Use())
        {
            panels[index].GetComponent<InventoryItem>().Add(null);
            items[index] = null;
        }
    }

    public static void Delete()
    {
        for (int i = 0; i < panelnum; i++)
        {
            if (items[i] != null && items[i].Type() == Gear)
            {
                panels[i].GetComponent<InventoryItem>().Add(null);
                items[i].GearUse();
                items[i] = null;
                Gear = Interactable.GEAR.NULL;
                break;
            }
        }
    }

}
