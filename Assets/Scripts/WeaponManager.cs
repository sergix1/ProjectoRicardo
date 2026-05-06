using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform weapon;

    [Header("ADS")]
    public Vector3 hipPosition;
    public Vector3 adsPosition;
    public float adsSpeed = 10f;

    [Header("Sway")]
    public float swayAmount = 0.02f;
    public float maxSway = 0.05f;
    public float swaySmooth = 8f;

    [Header("Recoil")]
    public float recoilReturnSpeed = 5f;
    public float recoilSnappiness = 10f;

    Vector3 currentADS;
    Vector3 currentSway;
    Vector3 currentRecoil;

    Vector3 targetRecoil;

    void Update()
    {
        HandleADS();
        HandleSway();
        HandleRecoil();

  
        Vector3 finalPos = currentADS + currentSway + currentRecoil;

        weapon.localPosition = finalPos;
    }

    void HandleADS()
    {
        bool aiming = Input.GetMouseButton(1);

        Vector3 target = aiming ? adsPosition : hipPosition;
        currentADS = Vector3.Lerp(currentADS, target, Time.deltaTime * adsSpeed);
    }

    void HandleSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float moveX = -mouseX * swayAmount;
        float moveY = -mouseY * swayAmount;

        moveX = Mathf.Clamp(moveX, -maxSway, maxSway);
        moveY = Mathf.Clamp(moveY, -maxSway, maxSway);

        // idle
        float idleX = Mathf.Sin(Time.time * 1.5f) * 0.003f;
        float idleY = Mathf.Cos(Time.time * 1.5f) * 0.003f;

        Vector3 target = new Vector3(moveX + idleX, moveY + idleY, 0);

        currentSway = Vector3.Lerp(currentSway, target, Time.deltaTime * swaySmooth);
    }

    void HandleRecoil()
    {
        targetRecoil = Vector3.Lerp(targetRecoil, Vector3.zero, recoilReturnSpeed * Time.deltaTime);
        currentRecoil = Vector3.Lerp(currentRecoil, targetRecoil, recoilSnappiness * Time.deltaTime);
    }

    public void AddRecoil()
    {
        targetRecoil += new Vector3(
            Random.Range(-0.02f, 0.02f),
            Random.Range(0.05f, 0.1f),
            Random.Range(-0.2f, -0.3f)
        );
    }
}