using UnityEngine;

public class ColisionsMap : MonoBehaviour
{
    void Start()
    {
        foreach (MeshFilter mf in GetComponentsInChildren<MeshFilter>())
        {
            if (mf.gameObject.GetComponent<Collider>() == null)
            {
                mf.gameObject.AddComponent<MeshCollider>();
            }
        }
    }
}