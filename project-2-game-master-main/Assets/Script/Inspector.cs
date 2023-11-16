using UnityEngine;
using UnityEngine.UI;

public class Inspector : MonoBehaviour
{
    public static bool ON;
    public GameObject obj_;
    private static GameObject obj;
    private static Vector2 initialSize; 

    private void Awake()
    {
        ON = false;
        obj = obj_;
        obj.SetActive(false);
        obj.GetComponent<Image>().sprite = null;
        initialSize = obj.GetComponent<RectTransform>().sizeDelta;
    }

    public static void Inspect(Sprite image)
{
    if (image != null) 
    {
        obj.SetActive(true); 
        Image imageComponent = obj.GetComponent<Image>();
        imageComponent.sprite = image;

        float scaleFactor = 1.8f;
        imageComponent.rectTransform.sizeDelta = new Vector2(initialSize.x * scaleFactor, initialSize.y * scaleFactor);
        ON = true; 
    }
    else 
    {
        obj.SetActive(false); 
        ON = false;
    }
}

}
