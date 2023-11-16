using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Passwordboxdoorcontroll : MonoBehaviour
{
    private bool open;
    private Quaternion openRotation;
    private Quaternion initRotation;
    private float rotationSpeed = 2.0f; 
    private float rotationProgress = 0.0f;

    public static bool Open;
    [SerializeField] private bool mirror;

    private void Start()
    {
        Open = false;
        open = false;
        initRotation = transform.rotation;
        if (!mirror) openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -110, 0));
        else openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 110, 0));
        rotationProgress = 0.0f;
    }

    void Update()
    {
        if (open != Open)
        {
            open = Open;
            StartCoroutine(RotateDoor());
        }
    }

    IEnumerator RotateDoor()
    {
        while (true)
        {
            rotationProgress += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(initRotation, openRotation, rotationProgress);
            if (rotationProgress >= 1.0f) break;
            yield return null;
        }
    }

}
