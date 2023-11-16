
using UnityEngine;

public class NewKey : MonoBehaviour, Interactable
{
    public Sprite image;
    private Interactable.GEAR type;
    public GameObject Gear;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void Interact() { }

    public void AddtoBar()
    {
        InventorySystem.AddItem(this, image);
    }

    public void SetType(bool valid)
    {
        type = valid ? Interactable.GEAR.KEY2VALID : Interactable.GEAR.KEY2INVALID;
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

    public Interactable.GEAR Type()
    {
        return type;
    }

    public void GearUse()
    {
        Gear.SetActive(false);
        gameObject.SetActive(true);
    }

}
