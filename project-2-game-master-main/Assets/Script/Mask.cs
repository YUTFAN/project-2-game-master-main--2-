using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour, Interactable
{
    private List<List<string>> text;
    private DialogSystem dialog;

    [SerializeField] private TextAsset textFile;
    [SerializeField] private Sprite image;
    [SerializeField] private GameObject Player;
    [SerializeField] private MorseLight l1;
    [SerializeField] private MorseLight l2;
    [SerializeField] private GameObject ThirdRoom_Doctor;

    void Start()
    {
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
        text = DialogSystem.GetTextFromFile(textFile);
        
    }

    public void Interact()
    {
        dialog.SendMesasge(text[0], gameObject, true, 0);
    }

    // TODO: when pickup mask, do some cool effect to doctor
    public void Pickup()
    {
        if (ThirdRoom_Doctor != null)
        {
            DoctorDie doctorDieComponent = ThirdRoom_Doctor.GetComponent<DoctorDie>();
            if (doctorDieComponent != null)
            {
                doctorDieComponent.enabled = true;
                StartCoroutine(doctorDieComponent.DieAfterSeconds(5f, ThirdRoom_Doctor)); // Start the coroutine here
            }
            else
            {
                Debug.LogError("DoctorDie component is missing on ThirdRoom_Doctor!");
            }

            /*
            // Loop through all children and enable the ApplyShader component
            foreach (Transform child in ThirdRoom_Doctor.transform)
            {
                ApplyShader childShader = child.GetComponent<ApplyShader>();
                if (childShader != null)
                {
                    childShader.enabled = true;
                }
            }
            */
            foreach (Transform child in ThirdRoom_Doctor.transform)
            {
                ApplyShader childShader = child.GetComponent<ApplyShader>();
                if (childShader != null)
                {
                    childShader.enabled = true;
                    childShader.ActivateEffect(); // Activate the effect here
                }
            }
        }
        else
        {
            Debug.LogError("ThirdRoom_Doctor is not assigned in the inspector!");
        }
        transform.position = new Vector3(0, -10, 0);
        InventorySystem.AddItem(this, image);
        l1.StartMorse();
        l2.StartMorse();
      
    }

    public bool Use()
    {
        if (FlyPoker.ON && GameStatus.GameStage != GameStatus.STAGE.Final) Player.GetComponent<PlayerMovement>().MirrorFlipCamera(false);
        return false;
    }
}
