using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject p1;
    [SerializeField] private GameObject p2;
    [SerializeField] private GameObject p3;

    private static GameObject portal_1;
    private static GameObject portal_2;
    private static GameObject portal_3;

    private void Start()
    {
        portal_1 = p1;
        portal_2 = p2;
        portal_3 = p3;
        portal_1.SetActive(false);
        portal_2.SetActive(false);
        portal_3.SetActive(false);
    }

    public static void OpenPortalThirdRoom ()
    {
        portal_1.SetActive(true);
        portal_2.SetActive(true);
    }

    public static void ClosePortalThirdRoom()
    {
        portal_1.SetActive(false);
        portal_2.SetActive(false);
    }

    public static void OpenPortalFinal()
    {
        portal_3.SetActive(true);
        portal_2.SetActive(true);
    }

    public static void ClosePortalFinal()
    {
        portal_3.SetActive(false);
        portal_2.SetActive(false);
    }

}
