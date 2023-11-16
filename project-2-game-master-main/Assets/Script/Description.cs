using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is for simple items that only have one single description
public class description : MonoBehaviour, Interactable
{
    [SerializeField] private TextAsset textFile;

    private List<List<string>> text;
    private DialogSystem dialog;

    void Start()
    {
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
        text = DialogSystem.GetTextFromFile(textFile);
    }

    public void Interact()
    {
        dialog.SendMesasge(text[0]);
    }

}
