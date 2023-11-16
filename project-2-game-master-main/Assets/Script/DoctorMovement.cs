using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Doctor : MonoBehaviour, Interactable
{
    private List<List<string>> text;
    private DialogSystem dialog;

    private Drug drug;
    private bool gameover;
    private int stage = 0;
    private int count = 0;
    private float initialY;
    private float rotationSpeed;
    private float rotationRatio;

    [SerializeField] private bool debug;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Boundry;
    [SerializeField] private GameObject Door;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private TextAsset MoveScript_1;
    [SerializeField] private TextAsset MoveScript_2;
    [SerializeField] private TextAsset MoveScript_3;
    [SerializeField] private TextAsset MoveScript_4;

    public enum STATUS
    {
        GAMEOVER,
        WAIT,
        UPCOMING,
        TALKING_1,
        LEAVING_1,
        WAIT_2,
        BACK,
        TALKING_2,
        LEAVING_2,
        FIRSTROOMOVER,
        CHASE
    }

    public static STATUS Status;

    private void Start()
    {
        gameover = false;
        rotationSpeed = 80;
        rotationRatio = 1.5f;
        initialY = transform.position.y;
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
        text = DialogSystem.GetTextFromFile(textFile);
        drug = GameObject.Find("Drug").GetComponent<Drug>();
        Status = STATUS.WAIT;
    }

    private void Update()
    {
        if (Status == STATUS.TALKING_1 || Status == STATUS.TALKING_2)
        {
            var lookPos = Player.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationRatio);
        }
        if (Status == STATUS.BACK && (transform.position - Player.transform.position).magnitude < 3 && RoomBoundry.Out)
        {
            gameover = true;
            GameoverDoctor.GameOver();
        }
        if (Status == STATUS.TALKING_2)
        {
            if (RoomBoundry.Out)
            {
                gameover = true;
                StartCoroutine(Chase());
            }
            else if (KeyController.picked) {
                gameover = true;
                GameoverDoctor.GameOver(1);
                Status = STATUS.GAMEOVER;
            }
            else if (GameObject.Find("Key")) Destroy(GameObject.Find("Key"));
        }
    }

    public void Interact()
    {
        switch (Status)
        {
            case STATUS.TALKING_1:
                talking_1();
                break;
            case STATUS.TALKING_2:
                dialog.SendMesasge(text[4], gameObject);
                break;
        }
    }

    private void talking_1()
    {
        if (stage == 0)
        {
            dialog.SendMesasge(text[0]);
            drug.eatable = true;
            stage = 1;
        }
        else
        {
            if (drug == null)
            {
                Status = STATUS.LEAVING_1;
                // Pause the dialog, give time for Doctor to run away
                dialog.SendMesasge(text[3], gameObject, false, 5);
            }
            else if (count < 3)
            {
                dialog.SendMesasge(text[1]);
                count += 1;
            }
            else
            {
                dialog.SendMesasge(text[2], gameObject);
            }
        }
    }

    public void CallDoctorOpenDoor(float pause)
    {
        if (Status == STATUS.WAIT)
        {
            Status = STATUS.UPCOMING;
            StartCoroutine(Move(MoveScript_1, pause));
        }
    }

    public void CallDoctorback(float pause)
    {
        if (Status == STATUS.WAIT_2)
        {
            Status = STATUS.BACK;
            StartCoroutine(Move(MoveScript_3, pause, false));
        }
    }

    public void Pickup()
    {
        if (Status == STATUS.TALKING_1 && count >= 3)
        {
            GameOverScreen.Setup();
        }
        else if (Status == STATUS.LEAVING_1)
        {
            StartCoroutine(Move(MoveScript_2, 0, true, 2));
        }
        else if(Status == STATUS.TALKING_2)
        {
            StartCoroutine(Move(MoveScript_4));
            Status = STATUS.LEAVING_2;
        }
    }

    IEnumerator Move(TextAsset moveScript, float pause = 0, bool waitrotate = true, float speed = 1)
    {
        // Cancle pause for debug
        if (!debug) yield return new WaitForSeconds(pause);
        
        Animator animator = GetComponent<Animator>();
        StringReader move = new StringReader(moveScript.text);
        bool door = false;
        bool rotate = true;

        while (!gameover)
        {
            string s;
            string line = move.ReadLine();
            if (line == null) break;
            var argument = line.Split(' ');

            while (waitrotate && rotate && !debug)
            {
                float y = float.Parse(argument[3]);
                float w = float.Parse(argument[4]);
                var rotation = new Quaternion(0, y, 0, w);
                rotation.Normalize();
                if (Mathf.Abs(transform.rotation.eulerAngles.y - rotation.eulerAngles.y) < 1f) rotate = false;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                yield return null;
            }

            s = argument[0];
            switch (s)
            {
                case "_E":
                    door = false;
                    animator.SetBool("OpenDoor", false);
                    break;
                case "E":
                    if (!door && Status != STATUS.BACK) StartCoroutine(OpenDoor());
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
            yield return new WaitForSeconds(MoveSpeed / speed);
        }

        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("OpenDoor", false);

        switch (Status)
        {
            case STATUS.UPCOMING:
                Status = STATUS.TALKING_1;
                break;
            case STATUS.LEAVING_1:
                Status = STATUS.WAIT_2;
                break;
            case STATUS.BACK:
                Status = STATUS.TALKING_2;
                Boundry.SetActive(false);
                break;
            case STATUS.LEAVING_2:
                Status = STATUS.FIRSTROOMOVER;
                break;
        }

    }

    IEnumerator Chase()
    {
        StartCoroutine(OpenDoor());
        Status = STATUS.CHASE;
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
                GameoverDoctor.GameOver();
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

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(0.35f);
        Door.GetComponent<CombinedDoorController>().OpenDoor();
    }

}
