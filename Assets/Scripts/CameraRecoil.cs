using UnityEngine;

public class CameraRecoi : MonoBehaviour
{
    float recoilX;

    void Update()
    {
        recoilX = Mathf.Lerp(recoilX, 0, Time.deltaTime * 6f);
    }

    public void CameraRecoil()
    {
        recoilX -= Random.Range(2f, 4f);
    }

    public float GetRecoil()
    {
        return recoilX;
    }
}

