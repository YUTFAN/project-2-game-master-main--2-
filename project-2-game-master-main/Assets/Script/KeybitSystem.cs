using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybitSystem : MonoBehaviour
{
    public static bool ON;
    public const int x = 23;
    public static int code;
    public GameObject newKey;
    public GameObject Wire;

    public static List<bool> record;
    private const int trueCode = 15;
    private static GameObject obj;
    public GameObject _obj;

    // Start is called before the first frame update
    void Start()
    {
        record = new List<bool>();
        for (int i = 0; i < 3; i++) { record.Add(false); };
        obj = _obj;
        ON = false;
        code = 0;
        obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ON && Input.GetKeyDown(KeyCode.E) && record[0] && record[1] && record[2])
        {
            ON = false;
            newKey.GetComponent<NewKey>().SetType(code == trueCode);
            newKey.GetComponent<NewKey>().AddtoBar();
            obj.SetActive(false);
        }
        else if (ON && Input.GetKeyDown(KeyCode.Q))
        {
            ON = false;
            InventorySystem.AddItem(Wire.GetComponent<Interactable>(), Wire.GetComponent<Wire>().image);
            obj.SetActive(false);
        }
    }

    public static void MakeKey()
    {
        obj.SetActive(true);
        ON = true;
    }

}
