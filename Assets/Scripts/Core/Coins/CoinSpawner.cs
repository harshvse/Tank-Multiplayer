using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private RespawingCoin coinPrefab;
    [SerializeField] private int spawnAmount;
    [SerializeField] private int coinValue = 10;
    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 ySpawnRange;
    [SerializeField] private LayerMask layerMask;

    private float radius;
    private Collider2D[] coinBuffer = new Collider2D[1];
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        radius = coinPrefab.GetComponent<CircleCollider2D>().radius;
        for (int i = 0; i < spawnAmount; i++)
        {
            SpawnCoin();
        }
    }
    public override void OnNetworkDespawn()
    {
        if (!IsServer) return;
    }
    
    private void SpawnCoin()
    {
        RespawingCoin coinInstance = Instantiate(coinPrefab, GetSpawnPoint(), Quaternion.identity);
        coinInstance.SetValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();
        coinInstance.OnCollected += HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawingCoin coin)
    {
        coin.transform.position = GetSpawnPoint();
        coin.Reset();
    }
    private Vector2 GetSpawnPoint()
    {
        float x = 0;
        float y = 0;
        while (true)
        {
            x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            y = Random.Range(ySpawnRange.x, ySpawnRange.y);
            Vector2 spawnPoint = new Vector2(x, y);
            int numColliders = Physics2D.OverlapCircleNonAlloc(spawnPoint, radius, coinBuffer,layerMask);
            if (numColliders == 0)
            {
                return spawnPoint;
            }
        }
    }

  
}

