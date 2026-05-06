using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    public GameObject hitEffect , hitEffectRed  , enemyHealth ;
   
    public Camera fpsCamera;
    public float range = 100f;
    public float fireRate = 10f;
    private WeaponRecoil recoil;
    public CameraRecoi recoilCamera;
    float nextTimeToFire = 0f;
    public ParticleSystem muzzleFlash ;
    public Light muzzleLight;
    public Transform shootPoint;
    IEnumerator FlashLight()
    {
        muzzleLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.03f);
        muzzleLight.gameObject.SetActive(false);
    }
    public GameObject weaponVisual;

    public override void OnNetworkSpawn()
    {
        weaponVisual.SetActive(IsOwner);
    }
    private void Start()
    {
        recoil = GetComponent<WeaponRecoil>();
   
    }

    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        recoil.Recoil();
        recoilCamera.CameraRecoil();
        muzzleFlash.Play();
        StartCoroutine(FlashLight());

        RaycastHit hit;

        if (Physics.Raycast(
            fpsCamera.transform.position,
            fpsCamera.transform.forward,
            out hit,
            range))
        {
            Debug.Log("CLIENT HIT: " + hit.collider.name);

            ulong netId = 0;

            NetworkObject netObj = hit.collider.GetComponentInParent<NetworkObject>();

            if (netObj != null)
            {
                netId = netObj.NetworkObjectId;
            }

            ShootServerRpc(netId, hit.point, hit.normal);
        }
    }
    [ServerRpc]
    void ShootServerRpc(ulong targetId, Vector3 hitPoint, Vector3 normal)
    {
        if (targetId == 0) return;

        NetworkObject targetObj;

        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(targetId, out targetObj))
        {
            PlayerHealth h = targetObj.GetComponent<PlayerHealth>();

            if (h != null)
            {
                Vector3 forceDir = (targetObj.transform.position - transform.position).normalized;

                h.TakeDamage(25, forceDir);
            }
        }

        SpawnImpactClientRpc(hitPoint, normal);
    }
    [ClientRpc]
    void SpawnImpactClientRpc(Vector3 point, Vector3 normal)
    {
        GameObject impact = Instantiate(
           hitEffectRed,
            point + normal * 0.02f,
            Quaternion.LookRotation(normal)
        );

        Destroy(impact, 2f);
    }
}


