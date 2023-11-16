using UnityEngine;

public class Poker : MonoBehaviour
{
    private Vector3 iniPos;
    private Vector3 directionX;
    private Vector3 directionY;
    private GameObject Player;

    [SerializeField] private float speedX;
    [SerializeField] private float speedY;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        iniPos = Player.transform.position;
        directionX = Player.transform.forward;
        directionY = -Player.transform.right.normalized;
        transform.position = iniPos + 1f * directionX;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pos = transform.position - iniPos;
        if (iniPos != Player.transform.position || Pos.magnitude > 7 )
        {
            Destroy(gameObject);
            FlyPoker.ON = false;
        }
        else
        {
            transform.position += Time.deltaTime * (speedX * directionX + speedY * Vector3.Project(Pos, directionX).magnitude * directionY);
        }
    }
}
