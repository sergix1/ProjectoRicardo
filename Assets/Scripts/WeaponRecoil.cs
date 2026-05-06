using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public Vector3 recoilKick = new Vector3(0, 0, -0.1f);
    public float returnSpeed = 5f;
    public float snappiness = 10f;

    Vector3 targetPosition;
    Vector3 currentPosition;
    Vector3 initialPosition; 

    void Start()
    {
        initialPosition = transform.localPosition; 
    }

    void Update()
    {
        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.deltaTime);

        transform.localPosition = initialPosition + currentPosition; 
    }

    public void Recoil()
    {
        targetPosition += new Vector3(
          Random.Range(-0.02f, 0.02f),  
          Random.Range(0.05f, 0.1f),   
          Random.Range(-0.2f, -0.3f)   
      );
    }
}