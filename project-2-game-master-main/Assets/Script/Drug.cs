using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drug : MonoBehaviour, Interactable
{
    public bool eatable = false;

    private List<List<string>> text;
    private DialogSystem dialog;

    [SerializeField] private TextAsset textFile;
    [SerializeField] private Sprite image;

    void Start()
    {
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
        text = DialogSystem.GetTextFromFile(textFile);
    }

    public void Interact()
    {
        if (!eatable)
        {
            dialog.SendMesasge(text[0]);
        }
        else
        {
            dialog.SendMesasge(text[1], gameObject);
        }
    }

    public void Pickup()
    {
        if (eatable)
        {
            transform.position = new Vector3(0, -10, 0);
            InventorySystem.AddItem(this, image);
        }
    }

    public bool Use()
    {
        Destroy(gameObject);
        return true;
    }
}
