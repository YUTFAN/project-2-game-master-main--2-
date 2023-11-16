using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wire : MonoBehaviour, Interactable
{
    public TextAsset textFile;
    public Sprite image;

    private List<List<string>> text;
    private DialogSystem dialog;

    void Start()
    {
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
        text = DialogSystem.GetTextFromFile(textFile);
    }

    public void Interact()
    {
        dialog.SendMesasge(text[0], gameObject);
    }

    public void Pickup()
    {
        transform.position = new Vector3(0, -10, 0);
        InventorySystem.AddItem(this, image);
    }

    public bool Use()
    {
        KeybitSystem.MakeKey();
        return true;
    }
}
