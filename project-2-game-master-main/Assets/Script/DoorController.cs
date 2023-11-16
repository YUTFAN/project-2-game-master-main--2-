using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedDoorController : MonoBehaviour, Interactable
{
    private DialogSystem dialog;
    private List<List<string>> text;

    private bool isOpen = false;
    private bool isMoving = false;
    private int CallDoctor;
    // Player can not open the door with out key
    private bool locked = true;
    private Quaternion initialRotation;
    private Quaternion openRotation;

    [SerializeField] private float doorOpenAngle = 90.0f;
    [SerializeField] private float openSpeed = 2.0f;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private GameObject doctor;

    void Start()
    {
        CallDoctor = 0;
        initialRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, doorOpenAngle, 0));
        text = DialogSystem.GetTextFromFile(textFile);
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
    }

    // Interact interface
    public void Interact()
    {
        if (locked && !isOpen)
        {
            if (CallDoctor == 0) 
            {
                dialog.SendMesasge(text[0]);
                CallDoctor++;
            }
            else
            {
                dialog.SendMesasge(text[4]);
                doctor.GetComponent<Doctor>().CallDoctorOpenDoor(1);
            }
        }
        else if (!locked)
        {
            OpenDoor();
        }
    }

    public void Interact(bool gear)
    {
        // TODO: add dialog when doctor not in STATUS.LEAVING_2, prevent player to use key
        if (Doctor.Status != Doctor.STATUS.FIRSTROOMOVER && (InventorySystem.Gear == Interactable.GEAR.KEY2VALID || InventorySystem.Gear == Interactable.GEAR.KEY2INVALID))
        {
            dialog.SendMesasge(text[1]);
        }
        else if (InventorySystem.Gear == Interactable.GEAR.KEY1)
        {
            InventorySystem.Delete();
        }
        else if (InventorySystem.Gear == Interactable.GEAR.KEY2VALID)
        {
            InventorySystem.Delete();
            locked = false;
            dialog.SendMesasge(text[3]);
        }
        else if (InventorySystem.Gear == Interactable.GEAR.KEY2INVALID)
        {
            dialog.SendMesasge(text[2], gameObject);
        }
    }

    // Use the Wrong key
    public void Pickup()
    {
        InventorySystem.Delete();
        GameOverScreen.Setup();
    }

    // Dcotor call this function
    public void OpenDoor()
    {
        if (!isMoving)
        {
            isOpen = !isOpen;
            isMoving = true;
            StartCoroutine(MoveDoor(isOpen ? openRotation : initialRotation));
        }
    }

    public void Close_Lock()
    {
        if (isOpen) { OpenDoor(); }
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }

    IEnumerator MoveDoor(Quaternion targetRotation)
    {
        float startTime = Time.time;

        while (Time.time - startTime <= 1 / openSpeed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        // Snap to the target rotation
        transform.rotation = targetRotation;
        isMoving = false;
    }

}
