using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class EndPlyaer : MonoBehaviour
{
    private float initialY;

    [SerializeField] private float MoveSpeed;
    [SerializeField] private TextAsset MoveScript_1;
    [SerializeField] private TextAsset MoveScript_2;
    [SerializeField] private EndDoctor endDoctor;

    private void Start()
    {
        initialY = transform.position.y;
        transform.position = new Vector3(-14.86f, initialY, 4.28f);
        transform.rotation = new Quaternion(0, 0.4703519f, 0, -0.882479f);
    }

    public void Talk_1()
    {
        StartCoroutine(Move(MoveScript_1));
    }

    public void Talk_2()
    {
        StartCoroutine(Move(MoveScript_2));
    }

    IEnumerator Move(TextAsset moveScript)
    {
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

            yield return new WaitForSeconds(MoveSpeed);
        }
        endDoctor.Pickup();
        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("OpenDoor", false);
    }
}
