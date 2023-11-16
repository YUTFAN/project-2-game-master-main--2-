using System.Collections.Generic;
using UnityEngine;

public class GameoverDoctor : MonoBehaviour, Interactable
{
    private List<List<string>> text;
    private static bool gameover;
    private static int messageID;

    [SerializeField] private GameObject Doctor;
    [SerializeField] private GameObject Final_Doctor;
    [SerializeField] private DialogSystem dialog;
    [SerializeField] private GameObject doctorGameover;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private AudioClip jumpscareSoundEffect;
    private AudioSource audioSource;

    void Awake()
    {
        gameover = false;
        doctorGameover.SetActive(false);
        text = DialogSystem.GetTextFromFile(textFile);

        // Ensure you have an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public static void GameOver(int messageid = 0)
    {
        gameover = true;
        messageID = messageid;
    }

    void Update()
    {
        if (GameStatus.GamePauseCode != GameStatus.Pause.Null || !gameover) return;

        PlayDoctorKillSound(); // Play the kill sound

        doctorGameover.SetActive(true);
        Doctor.SetActive(false);
        Final_Doctor.SetActive(false);
        dialog.SendMesasge(text[messageID], gameObject, true);
        gameover = false;
    }

    private void PlayDoctorKillSound()
    {
        if (jumpscareSoundEffect && audioSource)
        {
            audioSource.PlayOneShot(jumpscareSoundEffect);
        }
    }

    public void Pickup()
    {
        GameOverScreen.Setup();
    }

    public void Interact() { }
}
