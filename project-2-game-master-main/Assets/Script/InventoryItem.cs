using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{

    public int index;

    private void Awake()
    {
        gameObject.GetComponent<Image>().sprite = null;
    }

    public void SetIndex(int index) { this.index = index; }

    public void Add(Sprite image) { gameObject.GetComponent<Image>().sprite = image; }

    public void Use()
    {
        // Pervent using item after gameover
        if (GameStatus.GamePauseCode != GameStatus.Pause.GameOver)
        {
            InventorySystem.Use(index);
            Inspect_Off();
        }
    }

    public void Inspect_On()
    {
        if (GameStatus.GamePauseCode == GameStatus.Pause.GameOver) return;

        if (gameObject.GetComponent<Image>().sprite != null) {
            Inspector.Inspect(gameObject.GetComponent<Image>().sprite);
            Inspector.ON = true;
        }
    }

    public void Inspect_Off()
    {
        Inspector.Inspect(null);
        Inspector.ON = false;
    }
}
