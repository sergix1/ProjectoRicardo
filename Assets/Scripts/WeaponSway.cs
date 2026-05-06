using UnityEngine;

public class WeaponMotion : MonoBehaviour
{
    [Header("Sway")]
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;

    [Header("Bob")]
    public float bobSpeed = 6f;
    public float bobAmount = 0.05f;

    [Header("General")]
    public float smooth = 8f;
    public CharacterController controller;

    private Vector3 initialPosition;
    private float bobTimer;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
       
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float swayX = Mathf.Clamp(-mouseX * swayAmount, -maxSwayAmount, maxSwayAmount);
        float swayY = Mathf.Clamp(-mouseY * swayAmount, -maxSwayAmount, maxSwayAmount);

        Vector3 sway = new Vector3(swayX, swayY, 0);


        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        float speed = new Vector2(moveX, moveY).magnitude;
        Vector3 bob = Vector3.zero;

        if (speed > 0.1f)
        {
            bobTimer += Time.deltaTime * bobSpeed;

            float bobX = Mathf.Sin(bobTimer) * bobAmount;
            float bobY = Mathf.Cos(bobTimer * 2) * bobAmount;

            float speedMultiplier = Mathf.Clamp01(speed / 5f);

            bob = new Vector3(bobX, bobY, 0) * speedMultiplier;
        }
        else
        {
            bobTimer = 0;
        }

      
        float idleX = Mathf.Sin(Time.time * 1.5f) * 0.005f;
        float idleY = Mathf.Cos(Time.time * 1.5f) * 0.005f;
        Vector3 idle = new Vector3(idleX, idleY, 0);

  
        Vector3 finalPosition = initialPosition + sway + bob + idle;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            finalPosition,
            Time.deltaTime * smooth
        );
    }
}