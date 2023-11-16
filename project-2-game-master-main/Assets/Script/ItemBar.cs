using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public GameObject itemBarPanel;  // Reference to the item bar UI Panel
    public Image itemBarIcon;
    public Sprite openIcon; // Reference to the open icon
    public Sprite closeIcon; // Reference to the close icon


    // Start is called before the first frame update
    void Start()
    {
        // Make sure the item bar panel is active at the start
        itemBarPanel.SetActive(true);

        // Initialize mouse cursor to be locked and not visible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the 'E' key is pressed
        if (Input.GetKeyDown(KeyCode.Tab) && !Inspector.ON && !KeybitSystem.ON)
        {
            // Toggle mouse cursor state
            if (GameStatus.GamePauseCode == GameStatus.Pause.ItemBar)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                GameStatus.GamePauseCode = GameStatus.Pause.Null;
                itemBarIcon.sprite = closeIcon;
            }
            else if (GameStatus.GamePauseCode == GameStatus.Pause.Null)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameStatus.GamePauseCode = GameStatus.Pause.ItemBar;
                itemBarIcon.sprite = openIcon;
            }
        }
    }
}
