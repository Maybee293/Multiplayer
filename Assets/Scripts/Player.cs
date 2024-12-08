using TMPro;
using Unity.Netcode;
using UnityEngine;
/// <summary>
/// Component attached to the "Player Prefab" on the `NetworkManager`.
/// </summary>
public class Player : NetworkBehaviour
{
    /// <summary>
    /// If this method is invoked on the client instance of this player, it will invoke a `ServerRpc` on the server-side.
    /// If this method is invoked on the server instance of this player, it will teleport player to a random position.
    /// </summary>
    /// <remarks>
    /// Since a `NetworkTransform` component is attached to this player, and the authority on that component is set to "Server",
    /// this transform's position modification can only be performed on the server, where it will then be replicated down to all clients through `NetworkTransform`.
    /// </remarks>
    /// 
    public TextMeshPro textPlayerName;
    public bool isIT = false;
    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
        if (!IsOwner) return;
        IngameCamera.Instance.SetTargetTransform(this.transform);
    }

    [ServerRpc]
    public void RandomTeleportServerRpc()
    {
        var oldPosition = transform.position;
        transform.position = GetRandomPositionOnXYPlane();
        var newPosition = transform.position;
        print($"{nameof(RandomTeleportServerRpc)}() -> {nameof(OwnerClientId)}: {OwnerClientId} --- {nameof(oldPosition)}: {oldPosition} --- {nameof(newPosition)}: {newPosition}");
    }

    private static Vector3 GetRandomPositionOnXYPlane()
    {
        return new Vector3(Random.Range(0f, 5f), 0, Random.Range(0f, 5f));
    }

    private float moveSpeed = 3;
    private void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SetPlayerName();
        SetColor(Color.white);
        RandomPos();
    }

    private void SetPlayerName()
    {
        textPlayerName.text = "Player " + NetworkObjectId.ToString();
    }

    private void SetColor(Color color)
    {
        GetComponent<Renderer>().material.SetColor("_Color", color);
    }

    private void RandomPos()
    {
        transform.position = GetRandomPositionOnXYPlane();
    }

    private Player other = null;
    private void OnCollisionEnter(Collision collision)
    {
        if (!NetworkManager.Singleton.IsHost) return;
        Debug.Log($"OnCollisionEnter {NetworkObjectId} {isIT}");
        other = collision.gameObject.GetComponent<Player>();

        if (!delayCollision)
        {
            if (isIT && other != null)
            {
                delayCollision = true;
                other.delayCollision = true;
                Invoke("DelayCollision", timeDelayCollision);
                Debug.Log($"OnCollisionEnter {NetworkObjectId} {other.NetworkObjectId}");
                other.isIT = true;
                other.ChangeColorClientRpc(Color.red);

                this.isIT = false;
                this.ChangeColorClientRpc(Color.white);
            }
        }
    }

    public bool delayCollision = false;
    private float timeDelayCollision = 0.2f;

    private void DelayCollision()
    {
        delayCollision = false;
        other.delayCollision = false;
        other = null;
    }
    
    [ClientRpc]
    public void ChangeColorClientRpc(Color color)
    {
        Debug.Log($"ChangeColorClientRpc {NetworkObjectId}" + color.ToString());
        SetColor(color);
    }
}

