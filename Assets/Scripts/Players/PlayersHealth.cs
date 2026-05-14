using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;

public class PlayerHealth : NetworkBehaviour
{
    public float maxHealth = 100f;
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>();

    Rigidbody[] rbs;
    Collider[] cols;
    public CapsuleCollider capsule;

    CharacterController cc;
    PlayerMovement movement;
    Animator anim;
    private TextMeshProUGUI vidaText;
    public bool isRespawning = false;

    void Start()
    {
     

        rbs = GetComponentsInChildren<Rigidbody>();
        cols = GetComponentsInChildren<Collider>();

        cc = GetComponent<CharacterController>();
        movement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();


        foreach (Rigidbody rb in rbs)
            rb.isKinematic = true;

        foreach (Collider col in cols)
            if (col != capsule)
                col.enabled = false;
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
            currentHealth.Value = maxHealth;

        currentHealth.OnValueChanged += OnHealthChanged;

        if (IsOwner)
        {
            Invoke(nameof(SetupUI), 0.2f);
        }
    }

    void SetupUI()
    {
   

        GameObject vidaObj = GameObject.FindGameObjectWithTag("Vida");

        if (vidaObj != null)
        {
   

            vidaText = vidaObj.GetComponent<TextMeshProUGUI>();

            UpdateHealthText(currentHealth.Value);
        }
        
    }
    void OnHealthChanged(float oldValue, float newValue)
    {
        if (IsOwner)
        {
            UpdateHealthText(newValue);
        }

        if (newValue <= 0 && oldValue > 0)
        {
            DieVisual();
        }
    }

    void UpdateHealthText(float hp)
    {
        if (vidaText != null)
        {
            vidaText.text = Mathf.CeilToInt(hp) + " HP";
        }
    }
    public override void OnNetworkDespawn()
    {
        currentHealth.OnValueChanged -= OnHealthChanged;
    }



    public void TakeDamage(float dmg, Vector3 hitDir)
    {
        if (!IsServer) return;

        currentHealth.Value -= dmg;

        if (currentHealth.Value <= 0)
        {
            Die(hitDir); 
        }
    }
    void Die(Vector3 hitDir)
    {
        Debug.Log("Muerto en server");

        if (movement != null) movement.isDead = true;

  
        ApplyRagdollForce(hitDir);

        Invoke(nameof(Respawn), 3f);
    }
    void ApplyRagdollForce(Vector3 dir)
    {
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;

      
            Vector3 force = dir * 5f + Vector3.up * 2f;

            rb.AddForce(force, ForceMode.Impulse);
        }
    }
    void DieVisual()
    {
        if (anim != null) anim.enabled = false;

        if (cc != null)
            cc.enabled = false;

        foreach (Rigidbody rb in rbs)
            rb.isKinematic = false;

        foreach (Collider col in cols)
        {
            if (col != capsule)
                col.enabled = true;
        }
    }
    void Respawn()
    {
        Vector3 spawn = Spawner.instance.GetSpawnPosition(OwnerClientId);

        currentHealth.Value = maxHealth;

        RespawnClientRpc(spawn);
    }
    [ClientRpc]
    void RespawnClientRpc(Vector3 pos)
    {
        if (cc != null) cc.enabled = false;
        transform.position = pos;
        if (cc != null) cc.enabled = true;

        if (anim != null) anim.enabled = true;

        foreach (Rigidbody rb in rbs)
            rb.isKinematic = true;

        foreach (Collider col in cols)
            if (col != capsule)
                col.enabled = false;

        if (movement != null)
            movement.isDead = false;
    }
}