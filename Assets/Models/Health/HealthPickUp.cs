using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class HealthPickup : NetworkBehaviour
{
    public float healAmount = 25f;
    public float respawnTime = 10f;

    Collider col;
    MeshRenderer[] rends;

    void Start()
    {
        col = GetComponent<Collider>();
        rends = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health != null)
        {
            if (health.currentHealth.Value >= health.maxHealth)
                return;

            Heal(health);

            HidePickupClientRpc();

            StartCoroutine(RespawnRoutine());
        }
    }

    void Heal(PlayerHealth player)
    {
        player.currentHealth.Value += healAmount;

        if (player.currentHealth.Value > player.maxHealth)
        {
            player.currentHealth.Value = player.maxHealth;
        }
    }

    IEnumerator RespawnRoutine()
    {
        col.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        ShowPickupClientRpc();

        col.enabled = true;
    }

    [ClientRpc]
    void HidePickupClientRpc()
    {
        foreach (MeshRenderer r in rends)
        {
            r.enabled = false;
        }
    }

    [ClientRpc]
    void ShowPickupClientRpc()
    {
        foreach (MeshRenderer r in rends)
        {
            r.enabled = true;
        }
    }
}