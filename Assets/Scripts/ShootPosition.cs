using UnityEngine;

public class ShootPosition : MonoBehaviour
{
    public Transform hipPosition;
    public Transform adsPosition;
    public float speed = 10f;

    Vector3 currentPos;

    void Start()
    {
        currentPos = hipPosition.localPosition;
    }

    void Update()
    {
        bool aiming = Input.GetMouseButton(1);

        Vector3 target = aiming ? adsPosition.localPosition : hipPosition.localPosition;

        currentPos = Vector3.Lerp(currentPos, target, Time.deltaTime * speed);

        transform.localPosition = currentPos;
    }
}