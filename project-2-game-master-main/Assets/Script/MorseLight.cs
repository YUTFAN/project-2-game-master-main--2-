using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorseLight : MonoBehaviour
{
    // Start is called before the first frame update
    new private Light light;
    private GameObject player;
    private int[] morseCode;
    private int ON;

    [SerializeField] private float intensity;
    [SerializeField] private bool mirror;
    [SerializeField] private Material Dark;
    [SerializeField] private Material White;
    [SerializeField] private Material Red;
    [SerializeField] private GameObject Light;
    [SerializeField] private AudioClip sound0;
    [SerializeField] private AudioClip sound1;
    private AudioSource audioSource;

    // 8, 5
    private static int[] morseCodeMirror = { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
    // 2, 8
    private static int[] morseCodeNormal = { 0, 0, 1, 1, 1, 1, 1, 1, 0, 0 };

    void Start()
    {
        if (intensity == 0) intensity = 1;
        ON = 1;
        light = GetComponent<Light>();
        light.intensity = intensity;
        player = GameObject.Find("Player");
        if (mirror) morseCode = morseCodeMirror;
        else morseCode = morseCodeNormal;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mirror != GameStatus.Mirror) 
        { 
            ON = 1;
            Light.GetComponent<Renderer>().material = White;
        } 
        float disatnce = (transform.position - player.transform.position).magnitude;
        light.intensity = (1 - disatnce / 20) * intensity * ON;
    }

    public void StartMorse()
    {
        StartCoroutine(Morse());
    }

    IEnumerator Morse()
    {
        while (GameStatus.GameStage == GameStatus.STAGE.ThirdRoom)
        {
            yield return null;
            light.color = Color.white;
            if (GameStatus.Mirror != mirror)
            {
                ON = 1;
                continue;
            }
            foreach (var i in morseCode)
            {
                if (GameStatus.GameStage != GameStatus.STAGE.ThirdRoom || GameStatus.Mirror != mirror) break;
                if (i == 1)
                {
                    PlaySound(sound1);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    PlaySound(sound0);
                    yield return new WaitForSeconds(0.3f);
                }
                ON = 0;
                Light.GetComponent<Renderer>().material = Dark;
                yield return new WaitForSeconds(0.5f);
                ON = 1;
                Light.GetComponent<Renderer>().material = White;
            }
            light.color = Color.red;
            Light.GetComponent<Renderer>().material = Red;
            yield return new WaitForSeconds(1f);
            ON = 0;
            Light.GetComponent<Renderer>().material = Dark;
            yield return new WaitForSeconds(0.2f);
            ON = 1;
            Light.GetComponent<Renderer>().material = White;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}