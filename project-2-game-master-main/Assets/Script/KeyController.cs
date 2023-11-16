using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour, Interactable
{
    public TextAsset textFile;
    public Sprite image;
    public GameObject Gear;

    private List<List<string>> text;
    private DialogSystem dialog;
    public static bool picked;
    private int count;
    private Vector3 initPos;
    private const Interactable.GEAR type = Interactable.GEAR.KEY1;

    void Start()
    {
        count = 0;
        initPos = transform.localPosition;
        Gear.SetActive(false);
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
        text = DialogSystem.GetTextFromFile(textFile);
        picked = false;
    }

    public void Interact()
    {
        if (Doctor.Status == Doctor.STATUS.TALKING_1) dialog.SendMesasge(text[0]);
        else if (Doctor.Status == Doctor.STATUS.LEAVING_1 || Doctor.Status == Doctor.STATUS.BACK || Doctor.Status == Doctor.STATUS.WAIT_2) 
        {
            if (count < 2)
            {
                dialog.SendMesasge(text[1], gameObject);
            }
            else
            {
                dialog.SendMesasge(text[2], gameObject);
            }
        }
    }

    public void Pickup()
    {
        if (count > 2) return;
        count++;
        picked = true;
        transform.position = new Vector3(0, -10, 0);
        InventorySystem.AddItem(this, image);
    }
    
    public bool Use()
    {
        if (InventorySystem.Gear == type)
        {
            Gear.SetActive(false);
            InventorySystem.Gear = Interactable.GEAR.NULL;
        } 
        else
        {
            Gear.SetActive(true);
            InventorySystem.Gear = type;
        }
        
        return false;
    }

    public void GearUse()
    {
        picked = false;
        Gear.SetActive(false);
        transform.localPosition = initPos;
    }

    public Interactable.GEAR Type()
    {
        return type;
    }
}
