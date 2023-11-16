using UnityEngine;

public class FlyPoker : MonoBehaviour, Interactable
{
    public Sprite image;

    public static bool ON;
    [SerializeField] private GameObject Flypoker;

    // Start is called before the first frame update
    void Start()
    {
        ON = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public bool Use()
    {
        if (!ON)
        {
            Instantiate(Flypoker);
            ON = true;
        }
        return false;
    }

}
