using UnityEngine;
using TMPro;
using Unity.Netcode;

public class PlayerName : NetworkBehaviour
{
    public TextMeshProUGUI text;

    public override void OnNetworkSpawn()
    {
        text.text = "Player " + OwnerClientId;

        if (IsOwner)
            text.gameObject.SetActive(false); 
    }
    void LateUpdate()
    {
        if (Camera.main == null) return;

        transform.forward = Camera.main.transform.forward;
    }
}