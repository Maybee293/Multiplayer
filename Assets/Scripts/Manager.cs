using Unity.Netcode;
using UnityEngine;

public class Manager : MonoBehaviour
{
    NetworkManager networkManager = null;
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (!networkManager.IsClient && !networkManager.IsServer)
        {
            if (GUILayout.Button("Host"))
            {
                networkManager.StartHost();
            }

            if (GUILayout.Button("Client"))
            {
                networkManager.StartClient();
            }          
        }
        else
        {
            if (networkManager.IsHost)
            {
                if (GUILayout.Button("Start Game"))
                {
                    GameManager.Instance.StartGame();
                }
            }

            GUILayout.Label($"Mode: {(networkManager.IsHost ? "Host" : networkManager.IsServer ? "Server" : "Client")}");

            //// "Random Teleport" button will only be shown to clients
            //if (networkManager.IsClient)
            //{
            //    if (GUILayout.Button("Random Teleport"))
            //    {
            //        if (networkManager.LocalClient != null)
            //        {
            //            // Get `BootstrapPlayer` component from the player's `PlayerObject`
            //            if (networkManager.LocalClient.PlayerObject.TryGetComponent(out Player player))
            //            {
            //                // Invoke a `ServerRpc` from client-side to teleport player to a random position on the server-side
            //                player.RandomTeleportServerRpc();
            //            }
            //        }
            //    }
            //}
        }

        GUILayout.EndArea();
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        networkManager = NetworkManager.Singleton;
    }

    void Start()
    {
        networkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        networkManager.OnClientDisconnectCallback += OnClientDisconnectCallback;
        networkManager.OnServerStarted += OnServerStarted;
        networkManager.ConnectionApprovalCallback += ApprovalCheck;
        networkManager.OnTransportFailure += OnTransportFailure;
        networkManager.OnServerStopped += OnServerStopped;
    }

    void OnDestroy()
    {
        networkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
        networkManager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        networkManager.OnServerStarted -= OnServerStarted;
        networkManager.ConnectionApprovalCallback -= ApprovalCheck;
        networkManager.OnTransportFailure -= OnTransportFailure;
        networkManager.OnServerStopped -= OnServerStopped;
    }

    void OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log("OnClientDisconnectCallback");
    }

    void OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log("OnClientConnectedCallback");
    }

    void OnServerStarted()
    {
        Debug.Log("OnServerStarted");
    }

    void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        Debug.Log("ApprovalCheck");
    }

    void OnTransportFailure()
    {
        Debug.Log("ApprovalCheck");
    }

    void OnServerStopped(bool _) // we don't need this parameter as the ConnectionState already carries the relevant information
    {
        Debug.Log("ApprovalCheck");
    }

    public void StartClientLobby(string playerName)
    {
        Debug.Log("StartClientLobby");
    }

    public void StartClientIp(string playerName, string ipaddress, int port)
    {
        Debug.Log("StartClientIp");
    }

    public void StartHostLobby(string playerName)
    {
        Debug.Log("StartHostLobby");
    }

    public void StartHostIp(string playerName, string ipaddress, int port)
    {
        Debug.Log("StartHostIp");
    }

    public void RequestShutdown()
    {
        Debug.Log("RequestShutdown");
    }
}

