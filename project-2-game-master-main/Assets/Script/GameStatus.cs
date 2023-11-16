using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    // Different kinds of pause has different GamePaused code, -1 for gameover
    // To resume game, set GamePaused to 0
    public static Pause GamePauseCode;
    public static STAGE GameStage;
    public static bool Mirror;

    private static float timeout;

    // Time for whole game
    [SerializeField] private float Timeout;
    // Debug feature
    [SerializeField] private int stage;
    [SerializeField] private PlayerMovement pm;

    public enum STAGE
    {
        FirstRoom,
        SecondRoom,
        ThirdRoom,
        Final
    }

    public enum Pause
    {
        Null,
        GameOver,
        ItemBar,
        Dialogue,
        PokerGame,
        Win
    }

    private void Awake()
    {
        GameStage = STAGE.FirstRoom;
        Mirror = false;
        GamePauseCode = Pause.Null;
        timeout = Time.time + Timeout;
        if (stage != 0) InventorySystem.AddItem(GameObject.Find("FlyPoker").GetComponent<FlyPoker>(), GameObject.Find("FlyPoker").GetComponent<FlyPoker>().image);
        StartCoroutine(Timer());
        // debug
        switch (stage)
        {
            case 2:
                GameStage = STAGE.SecondRoom;
                break;
            case 3:
                GameStage = STAGE.ThirdRoom;
                break;
            case 4:
                GameStage = STAGE.Final;
                break;
        }
    }

    // This time is time in unity, which means if you pause the update, time will be pause too.
    IEnumerator Timer()
    {
        while (true)
        {
            if (GamePauseCode == Pause.Null && Time.time > timeout)
            {
                GameoverDoctor.GameOver(2);
                break;
            }
            yield return null;
        }
        
    }

    // call from other object to set up timer in seconds
    public static void SetupTimer(float time)
    {
        timeout = timeout < Time.time + time ? timeout : Time.time + time;
    }

}
