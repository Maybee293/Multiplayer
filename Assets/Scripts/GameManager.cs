using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI textTimeTag;
    public TextMeshProUGUI textEndGame;
    public float playTime = 120;
    public List<Player> players = new List<Player>();
    public NetworkObject speedPref;
    public float timeSpawnSpeed = 15;

    public const float SPEED_UP = 1.3f;
    public const float START_SPEED = 3;
    public const float TIME_SPEED_UP = 5;
    private void Awake()
    {
        Instance = this;
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public void StartGame()
    {
        if (!NetworkManager.Singleton.IsHost) return;
        RandomIT();
        StartCoundDownTimeClientRpc();
        InvokeRepeating("SpawnSpeed", 0, timeSpawnSpeed);
    }

    public void RandomIT()
    {
        Debug.Log("RandomIT");
        int i = Random.Range(0, players.Count);
        players[i].isIT = true;
        players[i].ChangeColorClientRpc(Color.red);
    }

    [ClientRpc]
    private void StartCoundDownTimeClientRpc()
    {
        InvokeRepeating("CountDownTime", 1, 1);
    }

    private void CountDownTime()
    {
        if (playTime > 0)
        {
            playTime--;
        }
        else
        {
            playTime = 0;
            CancelInvoke("CountDownTime");
            EndGame();
        }
        SetTextTime();
    }

    private void SetTextTime()
    {
        textTime.text = playTime.ToString();
    }

    private void Update()
    {
        UpdateTimeTag();
    }

    private void UpdateTimeTag()
    {
        if (playTime < 120)
        {
            string timeTag = string.Empty;
            for (int i = 0; i < players.Count; i++)
            {
                timeTag += $"PLayer {players[i].NetworkObjectId} : {players[i].timeIT.Value} \n";
            }
            textTimeTag.text = timeTag;
        }
    }

    private void EndGame()
    {
        Player winPlayer = GetPlayerWinGame();
        if (winPlayer != null)
        {
            textEndGame.text = $"Player {winPlayer.NetworkObjectId} win";
        }
    }

    private Player GetPlayerWinGame()
    {
        Player winPlayer = null;
        for (int i = 0; i < players.Count; i++)
        {
            if (winPlayer == null || players[i].timeIT.Value < winPlayer.timeIT.Value)
            {
                winPlayer = players[i];
            }
        }
        return winPlayer;
    }

    private void SpawnSpeed()
    {
        NetworkObject instance = Instantiate(speedPref);
        NetworkObject instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
        instanceNetworkObject.transform.position = new Vector3(Random.Range(0f, 5f), 0, Random.Range(0f, 5f));
    }
}