using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockButton : MonoBehaviour, Interactable
{
    [SerializeField] private PasswordLock Lock;
    [SerializeField] private int num;

    public void Interact()
    {
        Lock.Press(num);
    }
}
