using Unity.Netcode;
using UnityEngine;

public class PlayerAnim : NetworkBehaviour
{
    Animator anim;

    private NetworkVariable<float> netSpeed = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        netSpeed.OnValueChanged += OnSpeedChanged;
    }

    public override void OnNetworkDespawn()
    {
        netSpeed.OnValueChanged -= OnSpeedChanged;
    }

    void Update()
    {
        if (IsOwner)
        {
            float speed = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            ).magnitude;


            anim.SetFloat("Speed", speed);

            netSpeed.Value = speed;
        }
    }

    void OnSpeedChanged(float prev, float current)
    {
        if (!IsOwner) 
        {
            anim.SetFloat("Speed", current);
        }
    }
}