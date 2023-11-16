using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System;

public class DialogSystem : MonoBehaviour
{
    private float textSpeed = 0.03f;

    public GameObject Canvas;
    private Text dialogText;
    private Text dialogTitle;
    public Button dialogButton;

    private GameObject interactIteam;
    private bool isTyping = true;
    private int index = 0;
    [SerializeField] private bool firstEKeyPressed;

    public AudioClip dialogOpenSound;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        dialogText = Canvas.transform.Find("Dialoguebox/text").GetComponent<Text>();
        dialogTitle = Canvas.transform.Find("Dialoguebox/title").GetComponent<Text>();
        dialogButton = Canvas.transform.Find("Dialoguebox/Button").GetComponent<Button>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.1f;

    }

    public static List<List<string>> GetTextFromFile(TextAsset textFile)
    {
        // Store different text for different stage
        List<List<string>> textList = new List<List<string>>();
        Regex stage = new Regex("<stage[0-9]+>");
        var allText = stage.Split(textFile.text);
        foreach (string stageText in allText)
        {
            // Filter empty string
            if (stageText != "")
            {
                textList.Add(new List<string>(stageText.Trim().Split('\n')));
            }
        }
        return textList;
    }

    // Call this function to chat
    public void SendMesasge(List<string> text, GameObject item = null, bool notReverse = false, float pause = 0)
    {
        StartCoroutine(printDialog(text, item, notReverse ? false : GameStatus.Mirror, pause));
    }

    IEnumerator printDialog(List<string> text, GameObject item, bool reverse, float pause)
    {
        while (true)
        {
            if (GameStatus.GamePauseCode == GameStatus.Pause.Null) break;
            yield return null;
        }
        interactIteam = item;
        index = 0;
        GameStatus.GamePauseCode = GameStatus.Pause.Dialogue;
        isTyping = false;
        dialogText.text = "";
        dialogTitle.text = "";

        bool firstRound = true;
        Canvas.SetActive(true);

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.E) || firstRound)
            {
                if (!isTyping && index < text.Count)
                {
                    isTyping = true;
                    StartCoroutine(printline(text[index], reverse));
                    index++;
                }
                else if (isTyping)
                {
                    isTyping = false;
                }
                else
                {
                    break;
                }
                firstRound = false;
            }
            yield return null;
        }
        if (interactIteam) interactIteam.GetComponent<Interactable>().Pickup();
        if (pause > 0) yield return new WaitForSeconds(pause);
        if (GameStatus.GamePauseCode == GameStatus.Pause.Dialogue) GameStatus.GamePauseCode = GameStatus.Pause.Null;
        Canvas.SetActive(false);
        interactIteam = null;
    }

    IEnumerator printline(string line, bool reverse)
    {
        string[] tmp = line.Trim().Split("<title>");
        dialogTitle.text = tmp[0];
        string text = tmp[1];
        if (reverse)
        {
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            text = new string(array);
        }
        dialogText.text = "";
        float second = 0;
        int i = 0;

        while (isTyping && i < text.Length)
        {
            second += Time.deltaTime;
            if (second > textSpeed)
            {
                // fix colour bug
                if (text[i] == '<')
                {
                    int count = 2;
                    while (count > 0)
                    {
                        dialogText.text += text[i];
                        if (text[i] == '>') count--;
                        i++;
                    }

                    second = 0;
                }
                else
                {
                    dialogText.text += text[i];
                    if (text[i] != ' ')
                    {
                        second = 0;
                        if (!audioSource.isPlaying)
                        {
                            audioSource.PlayOneShot(dialogOpenSound);
                        }
                    }
                    i++;
                }
            }
            yield return null;
        }

        dialogText.text = text;
        isTyping = false;
    }


    void Update()
    {
        if (!firstEKeyPressed && Input.GetKeyDown(KeyCode.E))
        {
            Canvas.SetActive(false);
            firstEKeyPressed = true;
        }
    }

}
