using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPlayer : MonoBehaviour
{
    [SerializeField] private Button yourButton;
    private float initialY;
    void Start()
    {
        initialY = transform.position.y;
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(2 * Time.fixedDeltaTime, 0, 0);
        if (transform.position.x > 20 || transform.position.x < -40)
        {
            if (transform.position.x > 20) transform.position = new Vector3(transform.position.x - 60, initialY, transform.position.z);
            else transform.position = new Vector3(transform.position.x + 60, initialY, transform.position.z);
        }
        else transform.position = new Vector3(transform.position.x, initialY,     transform.position.z);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("MainScene");
    }
}
