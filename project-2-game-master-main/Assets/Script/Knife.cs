using UnityEngine;

public class Knife : MonoBehaviour, Interactable
{
    private const Interactable.GEAR type = Interactable.GEAR.KNIFE;

    [SerializeField] private Sprite knifeImage;
    [SerializeField] private GameObject Gear;
    [SerializeField] private CombinedDoorController door;


    public void Interact()
    {
        GameStatus.GameStage = GameStatus.STAGE.Final;
        door.Close_Lock();
        Portal.OpenPortalFinal();

        InventorySystem.AddItem(this, knifeImage);
        transform.position = new Vector3(0, -10, 0);

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

    private void GearUse()
    {
    }
}
