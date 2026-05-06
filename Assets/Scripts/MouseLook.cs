using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    public Transform cameraPivot;

    public float normalSensitivity = 2f;
    public float adsSensitivity = 1.2f;

    public bool isOwner;

    public float xRotation = 0f;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!isOwner) return;

        float sensitivity = Input.GetMouseButton(1) ? adsSensitivity : normalSensitivity;

        

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

    
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

       
        playerBody.Rotate(Vector3.up * mouseX);
    }
}