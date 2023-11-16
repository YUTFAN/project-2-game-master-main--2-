using UnityEngine;
using UnityEngine.UI;

public class CreditsRoll : MonoBehaviour
{
    public float speed = 30f;
    public RectTransform creditsRectTransform;

    private bool shouldScroll = true;

    void Update()
    {
        if (shouldScroll)
        {
            creditsRectTransform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }

        if (creditsRectTransform.position.y > 1000) 
        {
            shouldScroll = false;
        }
    }
}
