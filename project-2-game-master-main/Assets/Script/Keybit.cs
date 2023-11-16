using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Keybit : MonoBehaviour
{
    public bool Used = false;
    public float y;
    public int value;
    public int index;

    public void MakeBit()
    {
        if (!Used) 
        {
            for (int index = 0; index < 3; ++index)
            {
                if (KeybitSystem.record[index]) continue;
                KeybitSystem.record[index] = true;
                GameObject newBit = Instantiate(gameObject, transform.parent);
                newBit.GetComponent<Keybit>().Used = true;
                newBit.transform.localPosition = new Vector3(KeybitSystem.x + index * 4, y, 0);
                newBit.GetComponent<Keybit>().index = index;
                int _value = value;
                for (int i = 0; i < index; i++) _value *= 3;
                KeybitSystem.code += _value;
                newBit.GetComponent<Keybit>().value = _value;
                break;
            }
            
        }
        else if (Used)
        {
            KeybitSystem.record[index] = false;
            KeybitSystem.code -= value;
            Destroy(gameObject);
        }
        
    }
}
