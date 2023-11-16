using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordLock : MonoBehaviour
{
    private int ans;
    private int count;
    private bool ON;
    private bool pause;
    private bool locked;
    private static int unlock;

    [SerializeField] private List<PasswordNumber> words;
    [SerializeField] private GameObject Player;
    [SerializeField] private int correctAns;

    void Start()
    {
        ans = 0;
        count = 0;
        ON = false;
        pause = false;
        locked = true;
        unlock = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ON && (Player.transform.position - transform.position).magnitude > 10)
        {
            ON = false;
            foreach (var word in words) word.TurnOff();
        }
        else if (!ON && (Player.transform.position - transform.position).magnitude < 10)
        {
            ON = true;
            foreach (var word in words) word.TurnOn();
            int tmp = ans;
            for (int i = count - 1; i >= 0; i--) { words[i].Set(tmp % 10); tmp /= 10; }
        }
    }

    public void Press(int n)
    {
        if (pause || !locked) return;
        words[count].Set(n);
        ans *= 10;
        ans += n;
        count++;
        if (count != 4) return;
        if (ans == correctAns)
        {
            locked = false;
            unlock++;
            if (unlock == 2) Passwordboxdoorcontroll.Open = true;
        }
        else
        {
            StartCoroutine(Clear());
        }
    }

    // TODO
    IEnumerator Open()
    {
        yield return null;
    }

    IEnumerator Clear()
    {
        ans = 0;
        count = 0;
        pause = true;
        yield return new WaitForSeconds(1.5f);
        foreach (var word in words) { word.Clear(); }
        pause = false;
    }

}
