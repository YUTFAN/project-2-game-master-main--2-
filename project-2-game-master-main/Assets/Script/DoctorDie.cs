using UnityEngine;
using System.Collections;

public class DoctorDie : MonoBehaviour
{
    void Start()
    {
        this.enabled = false;
    }

    public IEnumerator DieAfterSeconds(float seconds, GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(objectToDestroy);
    }
}
