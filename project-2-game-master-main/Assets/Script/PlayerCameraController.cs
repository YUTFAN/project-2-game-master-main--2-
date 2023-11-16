using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform playerTransform;
    public float sensitivityX = 99.0f;
    public float sensitivityY = 99.0f;
    private float rotationX = 0.0f;
    private float mouseX = 0.0f;
    private float mouseY = 0.0f;

    //run this when building webgl
#if UNITY_WEBGL 
    private float webGLSensitivityAdjustment = 0.3f;
#endif 

    [SerializeField] private float interactDistance;
    [SerializeField] private GameObject Dot;
    [SerializeField] private GameObject Circle;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(dot_circle());
    }

    private void Update()
    {
        if (GameStatus.GamePauseCode == GameStatus.Pause.Null)
        {
            mouseX = Input.GetAxis("Mouse X") * sensitivityX;
            
            #if UNITY_WEBGL
                    mouseX *= webGLSensitivityAdjustment;
            #endif 

            if (GameStatus.Mirror) mouseX *= -1;
            mouseY = Input.GetAxis("Mouse Y") * sensitivityY;
            
            #if UNITY_WEBGL
                    mouseY *= webGLSensitivityAdjustment;
            #endif 
        }
        else
        {
            mouseX = 0;
            mouseY = 0;
        }
        // Rotate the player around the Y-axis (yaw)
        playerTransform.Rotate(Vector3.up * mouseX);

        // Calculate the vertical rotation for the camera (pitch)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        // Apply the rotations to the camera
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Make sure the player model faces the same direction as the camera
        playerTransform.rotation = Quaternion.Euler(0, transform.root.eulerAngles.y, 0);

        // Detect interactable object
        if (GameStatus.GamePauseCode == GameStatus.Pause.Null) rayDetect();
    }

    // Ray detect
    private void rayDetect()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            // CompareTag to check if this item is Interactive item
            if (Physics.Raycast(ray, out hit, interactDistance) && hit.transform.CompareTag("Interactive item"))
            {
                // Interact with Interactive item
                if (InventorySystem.Gear == Interactable.GEAR.NULL) hit.transform.GetComponent<Interactable>().Interact();
                else hit.transform.GetComponent<Interactable>().Interact(true);
            }
        }
    }

    IEnumerator dot_circle()
    {
        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            // CompareTag to check if this item is Interactive item
            if (Physics.Raycast(ray, out hit, interactDistance) && hit.transform.CompareTag("Interactive item"))
            {
                Circle.SetActive(true);
                Dot.SetActive(false);
            }
            else
            {
                Circle.SetActive(false);
                Dot.SetActive(true);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Mirror(bool move = true)
    {
        float y = transform.rotation.eulerAngles.y;
        if (move) playerTransform.rotation = Quaternion.Euler(0, 180f - y, 0);
        gameObject.GetComponent<Camera>().Render();
    }

}
