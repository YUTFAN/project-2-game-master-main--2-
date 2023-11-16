using System.Collections.Generic;
using UnityEngine;

public class SecondRoomBoundry : MonoBehaviour
{
    [SerializeField] private TextAsset textFile;
    [SerializeField] private GameObject doctor;

    private List<List<string>> text;
    private DialogSystem dialog;

    void Start()
    {
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
        text = DialogSystem.GetTextFromFile(textFile);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialog.SendMesasge(text[0]);
            doctor.GetComponent<Doctor>().CallDoctorback(0);
            Destroy(gameObject);
        }
    }
}
