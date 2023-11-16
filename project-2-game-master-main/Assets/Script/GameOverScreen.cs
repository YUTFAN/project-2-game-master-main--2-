using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject _obj;
    private static GameObject obj;
    private static bool gameover;


    private void Start()
    {
        gameover = false;
        obj = _obj;
        obj.SetActive(false);
    }

    public static void Setup (){
        gameover = true;
    }
    
    // Busy Wait
    void Update()
    {
        while (GameStatus.GamePauseCode != GameStatus.Pause.Null || !gameover) return;
        obj.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameStatus.GamePauseCode = GameStatus.Pause.GameOver;
        gameover = false;
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("StartScene");
        print("The button is working");
    }
}
