using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openItembar : MonoBehaviour
{
    public GameObject openPanel;
    public bool isOpened;

    // Start is called before the first frame update
    void Start()
    {
        openPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpened)
            {
                CloseBar(); 
            }
            else
            {
                OpenBar();
            }
        }
    }

    public void OpenBar()
    {
        openPanel.SetActive(true);
        isOpened = true;
    }

    public void CloseBar()
    {
        openPanel.SetActive(false);
        isOpened = false;
    }
}
