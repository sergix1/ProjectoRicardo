using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    public float bobSpeed = 6f;
    public float bobAmount = 0.05f;
    public float smooth = 8f;

    public CharacterController controller; 

    private Vector3 initialPosition;
    private float timer;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float speed = controller.velocity.magnitude;

        if (speed > 0.1f)
        {
            timer += Time.deltaTime * bobSpeed;

            float bobX = Mathf.Sin(timer) * bobAmount;
            float bobY = Mathf.Cos(timer * 2) * bobAmount;

            Vector3 bob = new Vector3(bobX, bobY, 0);

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                initialPosition + bob,
                Time.deltaTime * smooth
            );
        }
        else
        {
            timer = 0;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                initialPosition,
                Time.deltaTime * smooth
            );
        }
    }
}