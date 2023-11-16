using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBoundry : MonoBehaviour
{
    public static bool Out;
    // Start is called before the first frame update
    void Start()
    {
        Out = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { Out = !Out; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) { Out = !Out; }
    }
}
