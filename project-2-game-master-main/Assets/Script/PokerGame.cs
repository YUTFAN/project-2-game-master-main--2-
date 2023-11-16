using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerGame : MonoBehaviour, Interactable
{
    private bool ON;
    private bool skip;

    [SerializeField] private GameObject Poker;
    [SerializeField] private GameObject boy;

    // Start is called before the first frame update
    void Start()
    {
        ON = false;
        skip = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (skip)
        {
            skip = false;
            return;
        }
        if (ON && Input.GetKeyDown(KeyCode.E))
        {
            ON = false;
            Poker.SetActive(false);
            GameStatus.GamePauseCode = GameStatus.Pause.Null;
            transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            boy.GetComponent<Boy>().EndPokerGame();
        }
    }

    public void Interact()
    {
        if (Boy.inGame && !GameStatus.Mirror)
        {
            ON = true;
            Poker.SetActive(true);
            GameStatus.GamePauseCode = GameStatus.Pause.PokerGame;
            transform.position = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
            skip = true;
        }
    }

}
