using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public List<Player> players = new List<Player>();

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public void StartGame()
    {
        if (!NetworkManager.Singleton.IsHost) return;
        RandomIT();
    }

    public void RandomIT()
    {
        Debug.Log("RandomIT");
        int i = Random.Range(0, players.Count);
        players[i].isIT = true;
        players[i].ChangeColorClientRpc(Color.red);
    }
}