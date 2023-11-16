using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndDoctor : MonoBehaviour, Interactable
{
    private List<List<string>> text;
    private DialogSystem dialog;

    private float initialY;
    private int talk;
    private bool end;
    private float initRotateZ;
    private float initRotateX;

    [SerializeField] private float MoveSpeed;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private TextAsset MoveScript_1;
    [SerializeField] private TextAsset MoveScript_2;
    [SerializeField] private CombinedDoorController Door;
    [SerializeField] private EndPlyaer endPlyaer;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject creditsRoll;
    [SerializeField] private Button yourButton;

    private void Start()
    {
        end = false;
        talk = 0;
        initialY = transform.position.y;
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
        text = DialogSystem.GetTextFromFile(textFile);
        dialog.SendMesasge(text[talk], gameObject);
        transform.position = new Vector3(-4.02f, initialY, 0.9f);
        transform.rotation = new Quaternion(0, 0.003494645f, 0, -0.9999939f);
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        creditsRoll.SetActive(false);
    }

    public void Pickup()
    {
        if (talk == 0) 
        { 
            StartCoroutine(Move(MoveScript_1));
            talk = 1;
        }
        else if (talk == 1)
        {
            dialog.SendMesasge(text[talk], gameObject);
            talk = 2;
        }
        else if (talk == 2)
        {
            talk = 3;
            endPlyaer.Talk_2();
        }
        else if (talk == 3)
        {
            talk = 4;
            dialog.SendMesasge(text[2], gameObject);
        }
        else
        {
            StartCoroutine(Move(MoveScript_2));
        }
    }

    IEnumerator Move(TextAsset moveScript)
    {
        bool door = false;
        Animator animator = GetComponent<Animator>();
        StringReader move = new StringReader(moveScript.text);

        while (true)
        {
            string s;
            string line = move.ReadLine();
            if (line == null) break;
            var argument = line.Split(' ');

            s = argument[0];
            switch (s)
            {
                case "_E":
                    door = false;
                    animator.SetBool("OpenDoor", false);
                    break;
                case "E":
                    if (!door) StartCoroutine(OpenDoor());
                    door = true;
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

            yield return new WaitForSeconds(MoveSpeed);
        }

        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("OpenDoor", false);
        if (talk == 1)
        {
            endPlyaer.Talk_1();
        }
        else
        {
            end = true;
            button.SetActive(true);
            animator.SetBool("Walk", true);

        }

        if (talk == 4) 
        {
            button.SetActive(false);
            StartCredits();
        }
    }

    IEnumerator rotate()
    {
        while (Mathf.Abs(transform.rotation.y - 90f) > 0.5)
        {
            transform.Rotate(new Vector3(0, MoveSpeed, 0));
            yield return new WaitForSeconds(MoveSpeed);
        }
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(0.35f);
        Door.GetComponent<CombinedDoorController>().OpenDoor();
    }

    public void Interact() { }

    void FixedUpdate()
    {
        if (end)
        {
            transform.position += new Vector3(2 * Time.fixedDeltaTime, 0, 0);
        }
        if (transform.position.x > 20 || transform.position.x < -40)
        {
            if (transform.position.x > 20) transform.position = new Vector3(transform.position.x - 60, initialY, transform.position.z);
            else transform.position = new Vector3(transform.position.x + 60, initialY, transform.position.z);
        }
        else transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
    }

    IEnumerator ShowCreditsRoll()
    {
        creditsRoll.SetActive(true); 

        yield return new WaitForSeconds(5);

        creditsRoll.SetActive(false); 
        button.SetActive(true); 
    }

    public void TaskOnClick()
    {
        StopCoroutine(ShowCreditsRoll()); 
        SceneManager.LoadScene("MainScene"); 
    }

    public void StartCredits()
    {
        StartCoroutine(ShowCreditsRoll());
    }

}
