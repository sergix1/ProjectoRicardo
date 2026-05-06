using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class NetworkUI : MonoBehaviour
{
    public GameObject menuCamera;
    enum MenuState
    {
        Main,
        Connect
    }

    MenuState state = MenuState.Main;

    string ip = "127.0.0.1";
    string port = "7777";

    bool isHost = false;

    void OnGUI()
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
        {
            if (menuCamera != null)
                menuCamera.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }

        if (menuCamera != null)
            menuCamera.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float width = 250;
        float height = 200;

        GUILayout.BeginArea(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height), GUI.skin.box);

        if (state == MenuState.Main)
        {
            GUILayout.Label("Network Menu");

            if (GUILayout.Button("Host"))
            {
                isHost = true;
                state = MenuState.Connect;
            }

            if (GUILayout.Button("Client"))
            {
                isHost = false;
                state = MenuState.Connect;
            }

            if (GUILayout.Button("Server"))
            {
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetConnectionData("0.0.0.0", ushort.Parse(port));

                NetworkManager.Singleton.StartServer();
            }
        }

        else if (state == MenuState.Connect)
        {
            GUILayout.Label(isHost ? "Host Setup" : "Client Connect");

            GUILayout.Label("IP:");
            ip = GUILayout.TextField(ip);

            GUILayout.Label("Port:");
            port = GUILayout.TextField(port);

            GUILayout.Space(10);

            if (GUILayout.Button(isHost ? "Start Host" : "Connect"))
            {
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

                if (isHost)
                {
                    transport.SetConnectionData("0.0.0.0", ushort.Parse(port));
                    NetworkManager.Singleton.StartHost();
                }
                else
                {
                    transport.SetConnectionData(ip, ushort.Parse(port));
                    NetworkManager.Singleton.StartClient();
                }
            }

            if (GUILayout.Button("Back"))
            {
                state = MenuState.Main;
            }
        }

        GUILayout.EndArea();
    }
}