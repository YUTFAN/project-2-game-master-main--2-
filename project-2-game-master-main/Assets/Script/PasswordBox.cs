using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordBox : MonoBehaviour
{
    private bool ON;

    [SerializeField] private bool mirror;

    void Start()
    {
        ON = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ON && mirror != GameStatus.Mirror)
        {
            ON = false;
            transform.position = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
        }
        else if (!ON && mirror == GameStatus.Mirror)
        {
            ON = true;
            transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        }
    }

}
