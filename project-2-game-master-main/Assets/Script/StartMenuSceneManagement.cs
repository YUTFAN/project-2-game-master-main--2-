using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuSceneManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        Debug.Log("Start game button clicked!");
        SceneManager.LoadScene("MainScene");
    }


    // public void QuitGame()
    // {
    //     Application.Quit();
    // }
}
