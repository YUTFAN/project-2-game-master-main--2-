using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundry : MonoBehaviour
{
    [SerializeField] private bool enter = true;
    [SerializeField] private TextAsset textFile;

    private List<List<string>> text;
    private DialogSystem dialog;

    void Start()
    {
        if (enter)
        {
            dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
            text = DialogSystem.GetTextFromFile(textFile);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enter && other.CompareTag("Player"))
        {
            dialog.SendMesasge(text[0]);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enter && other.CompareTag("Player")) GameoverDoctor.GameOver();
    }
}
