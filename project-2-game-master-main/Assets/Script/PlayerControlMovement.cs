using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;
    public float moveSpeed;
    public float jumpSpeed;
    private Vector3 dir;
    private float initialY;
    private float horizontalMove;
    private float verticalMove;
    private const float middle = 0.5f;
    private bool thirdloop;
    private bool finalloop;

    private void Start()
    {
        thirdloop = false;
        finalloop = false;
        cc = GetComponent<CharacterController>();
        initialY = transform.position.y;
    }

    private void FixedUpdate()  // Using FixedUpdate for better physics behavior
    {
        if (GameStatus.GamePauseCode == GameStatus.Pause.Null)
        {
            // Get input from the player
            horizontalMove = Input.GetAxis("Horizontal") * moveSpeed;
            if (GameStatus.Mirror) { horizontalMove *= -1; }
            verticalMove = Input.GetAxis("Vertical") * moveSpeed;
            if (RoomBoundry.Out)
            {
                horizontalMove *= 1.5f;
                verticalMove *= 1.5f;
            }
        }
        else
        {
            horizontalMove = 0;
            verticalMove = 0;
        }

        // Calculate movement direction based on input
        dir = transform.forward * verticalMove + transform.right * horizontalMove;
        cc.Move(dir * Time.fixedDeltaTime);

        switch (GameStatus.GameStage)
        {
            case GameStatus.STAGE.FirstRoom:
                fitstroom();
                break;
            case GameStatus.STAGE.SecondRoom:
                secondroom();
                break;
            case GameStatus.STAGE.ThirdRoom:
                thirdroom();
                break;
            case GameStatus.STAGE.Final:
                final();
                break;
        }
    }

    private void fitstroom()
    {
        if (cc.transform.position.x > 20 || cc.transform.position.x < -40)
        {
            if (cc.transform.position.x > 20) cc.transform.position = new Vector3(cc.transform.position.x - 60, initialY, cc.transform.position.z);
            else cc.transform.position = new Vector3(cc.transform.position.x + 60, initialY, cc.transform.position.z);
        }
        else cc.transform.position = new Vector3(cc.transform.position.x, initialY, cc.transform.position.z);
    }

    private void secondroom()
    {
        if (cc.transform.position.x > 20 || cc.transform.position.x < -40)
        {
            if (cc.transform.position.x > 20) cc.transform.position = new Vector3(cc.transform.position.x - 60, initialY, cc.transform.position.z);
            else cc.transform.position = new Vector3(cc.transform.position.x + 60, initialY, cc.transform.position.z);
            MirrorFlipCamera();
        }
        else cc.transform.position = new Vector3(cc.transform.position.x, initialY, cc.transform.position.z);
    }

    private void thirdroom()
    {
        if (!thirdloop)
        {
            cc.transform.position = new Vector3(cc.transform.position.x, initialY, cc.transform.position.z);
            if (cc.transform.position.x <= 20 && cc.transform.position.x >= -40) return;
            if (cc.transform.position.x < -40) cc.transform.position = new Vector3(cc.transform.position.x + 60, initialY, cc.transform.position.z);
            MirrorFlipCamera();
            Portal.ClosePortalThirdRoom();
            thirdloop = true;
        }
        else if (cc.transform.position.x < 20 || cc.transform.position.x > 50)
        {
            if (cc.transform.position.x < 20) cc.transform.position = new Vector3(cc.transform.position.x + 30, initialY, cc.transform.position.z);
            else cc.transform.position = new Vector3(cc.transform.position.x - 30, initialY, cc.transform.position.z);
        }
        else cc.transform.position = new Vector3(cc.transform.position.x, initialY, cc.transform.position.z);
    }

    private void final()
    {
        if (!finalloop)
        {
            cc.transform.position = new Vector3(cc.transform.position.x, initialY, cc.transform.position.z);
            if (cc.transform.position.x >= 20 && cc.transform.position.x <= 50) return;
            if (cc.transform.position.x > 50) cc.transform.position = new Vector3(cc.transform.position.x - 90, initialY, cc.transform.position.z);
            if (GameStatus.Mirror) MirrorFlipCamera();
            Portal.ClosePortalFinal();
            finalloop = true;
        }
        else if (cc.transform.position.x > 20 || cc.transform.position.x < -40)
        {
            if (cc.transform.position.x > 20) cc.transform.position = new Vector3(cc.transform.position.x - 60, initialY, cc.transform.position.z);
            else cc.transform.position = new Vector3(cc.transform.position.x + 60, initialY, cc.transform.position.z);
        }
        else cc.transform.position = new Vector3(cc.transform.position.x, initialY, cc.transform.position.z);
    }

    // Set move = false to prevent plyaer move
    public void MirrorFlipCamera(bool move = true)
    {
        GameStatus.Mirror = !GameStatus.Mirror;

        if(move) cc.transform.position = new Vector3(cc.transform.position.x, initialY, 2 * middle - cc.transform.position.z);

        Camera camera = GameObject.Find("Camera").GetComponent<Camera>();

        GameObject.Find("Camera").GetComponent<PlayerCameraController>().Mirror(move);

        Matrix4x4 mat = camera.projectionMatrix;

        mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));

        camera.projectionMatrix = mat;

        GL.invertCulling = GameStatus.Mirror;
    }

}
