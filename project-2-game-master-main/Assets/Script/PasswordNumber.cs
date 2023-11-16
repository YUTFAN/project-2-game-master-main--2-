using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordNumber : MonoBehaviour
{
    [SerializeField] private List<GameObject> Number;
    [SerializeField] private Material Dark;
    [SerializeField] private Material Idle;
    [SerializeField] private Material ON;

    private int[,] map =
    {
        {1, 0, 1, 1, 1, 1, 1},
        {0, 0, 0, 0, 0, 1, 1},
        {1, 1, 1, 0, 1, 1, 0},
        {1, 1, 1, 0, 0, 1, 1},
        {0, 1, 0, 1, 0, 1, 1},
        {1, 1, 1, 1, 0, 0, 1},
        {1, 1, 1, 1, 1, 0, 1},
        {1, 0, 0, 0, 0, 1, 1},
        {1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 0, 1, 1},
    };

    public void TurnOff()
    {
        foreach (GameObject gameobj in Number)
        {
            gameobj.GetComponent<Renderer>().material = Dark;
        }
    }

    public void TurnOn()
    {
        foreach (GameObject gameobj in Number)
        {
            gameobj.GetComponent<Renderer>().material = Idle;
        }
    }

    public void Clear()
    {
        foreach (GameObject gameobj in Number)
        {
            gameobj.GetComponent<Renderer>().material = Idle;
        }
    }

    public void Set(int n)
    {
        for (int i = 0; i < 7; i++)
        {
            if (map[n, i] == 1) Number[i].GetComponent<Renderer>().material = ON;
        }
    }

}
