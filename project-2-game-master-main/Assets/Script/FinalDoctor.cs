using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class FinalDoctor : MonoBehaviour, Interactable
{
    private List<List<string>> text;
    private DialogSystem dialog;

    private bool win;
    private bool restart;
    private float initialY;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    [SerializeField] private float MoveSpeed;
    [SerializeField] private GameObject Player;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private TextAsset MoveScript_1;
    [SerializeField] private GameObject doctorGameover;
    [SerializeField] private CombinedDoorController Door;
    [SerializeField] private Material Dark;

    private void Start()
    {
        win = false;
        restart = false;
        initialY = transform.position.y;
        initialPosition = new Vector3(-0.013f, 0.636f, 0.024f);
        initialRotation = Player.transform.rotation;
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
        text = DialogSystem.GetTextFromFile(textFile);
    }

    private void Update()
    {
        if (GameStatus.GamePauseCode != GameStatus.Pause.Null || !restart) return;
        doctorGameover.SetActive(true);
        gameObject.SetActive(false);
        dialog.SendMesasge(text[0], gameObject, true);
        restart = false;
    }

    public void Interact()
    {
    }

    public void Interact(bool gear)
    {
        if (InventorySystem.Gear == Interactable.GEAR.KNIFE)
        {
            StartCoroutine(Win());
        }
    }

    public void CallDoctorback()
    {
        StartCoroutine(Move(MoveScript_1));
    }

    public void Pickup()
    {
        Boy.FinalReset();
        Player.transform.position = initialPosition;
        Player.transform.rotation = initialRotation;
        transform.position = new Vector3(0, -10, 0);
        doctorGameover.SetActive(false);
        gameObject.SetActive(true);
    }

    IEnumerator Move(TextAsset moveScript)
    {
        Animator animator = GetComponent<Animator>();
        StringReader move = new StringReader(moveScript.text);

        while (!win)
        {
            string s;
            string line = move.ReadLine();
            if (line == null) break;
            var argument = line.Split(' ');

            s = argument[0];
            switch (s)
            {
                case "_E":
                    animator.SetBool("OpenDoor", false);
                    break;
                case "E":
                    animator.SetBool("OpenDoor", true);
                    break;
                case "_W":
                    animator.SetBool("Walk", false);
                    break;
                case "W":
                    animator.SetBool("Walk", true);
                    break;
                case "_R":
                    animator.SetBool("Run", false);
                    break;
                case "R":
                    animator.SetBool("Run", true);
                    break;
            }
            transform.position = new Vector3(float.Parse(argument[1]), initialY, float.Parse(argument[2]));
            transform.rotation = new Quaternion(0, float.Parse(argument[3]), 0, float.Parse(argument[4]));

            // Check distance
            if ((transform.position - Player.transform.position).magnitude < 5 && Player.transform.position.x < transform.position.x)
            {
                animator.SetBool("Run", false);
                animator.SetBool("Walk", false);
                animator.SetBool("OpenDoor", false);
                StartCoroutine(Chase());
                yield break;
            }
            yield return new WaitForSeconds(MoveSpeed);
        }

        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("OpenDoor", false);
        if (!win) StartCoroutine(Chase());
    }

    IEnumerator Chase()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("Run", true);
        float x = Player.transform.position.x - transform.position.x;
        while (true)
        {
            var lookPos = Player.transform.position - transform.position;
            lookPos.y = 0;
            if (x * (Player.transform.position.x - transform.position.x) < 0) lookPos *= -1;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
            transform.position += transform.forward * Time.fixedDeltaTime * 5f;

            if ((transform.position - Player.transform.position).magnitude < 3)
            {
                restart = true;
                animator.SetBool("Run", false);
                break;
            }
            if (transform.position.x > 20 || transform.position.x < -40)
            {
                if (transform.position.x > 20) transform.position = new Vector3(transform.position.x - 60, initialY, transform.position.z);
                else transform.position = new Vector3(transform.position.x + 60, initialY, transform.position.z);
            }

            if (transform.position.z > 2) transform.position = new Vector3(transform.position.x, transform.position.y, 2);
            if (transform.position.z < -1) transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Win()
    {
        GameStatus.GamePauseCode = GameStatus.Pause.Win;
        win = true;
        while (true)
        {
            var lookPos = Player.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1.5f);
            if (Mathf.Abs(transform.rotation.eulerAngles.y - rotation.eulerAngles.y) < 5f) break;
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        float fade = 1;
        while (fade > 0)
        {
            Dark.color = new Color(1f - fade, 0.0f, 0.0f, fade);
            fade -= Time.deltaTime * 0.2f;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2);
        GameStatus.GamePauseCode = GameStatus.Pause.Null;
        SceneManager.LoadScene("EndScene");
    }
}
