using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSceneController : MonoBehaviour
{
    public Text endingText;
    public Text orDidYouText;

    private bool isEndingTextActive = true;

    private void Start()
    {
        endingText.gameObject.SetActive(true);
        orDidYouText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isEndingTextActive)
            {
                endingText.gameObject.SetActive(false);
                orDidYouText.gameObject.SetActive(true);
                isEndingTextActive = false;
            }
            else
            {
                SceneManager.LoadScene("NewEndScene");
            }
        }
    }
}