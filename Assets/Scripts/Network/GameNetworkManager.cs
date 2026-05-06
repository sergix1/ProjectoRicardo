using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using System;

public class GameNetworkManager : MonoBehaviour
{
    public  GameObject uiServerDisconnected;
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnServerStopped += OnServerStopped;
    }

    private void OnServerStopped(bool obj)
    {
        uiServerDisconnected.SetActive(true);
    }


    void OnClientConnected(ulong clientId)
    {

        if (!NetworkManager.Singleton.IsServer) return;

        var player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;

        Spawner spawner = FindFirstObjectByType<Spawner>();
        if (spawner == null) return;

        Vector3 spawnPos = spawner.GetSpawnPosition(clientId);

        Debug.Log("Spawning player " + clientId + " at " + spawnPos);

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // ?? LA CLAVE
        var nt = player.GetComponent<NetworkTransform>();

        if (nt != null)
        {
            nt.Teleport(
                spawnPos,
                Quaternion.identity,
                player.transform.localScale
            );
        }
        else
        {
            // fallback por si acaso
            player.transform.position = spawnPos;
        }
        if (cc != null) cc.enabled = true;
    }
}