using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.Netcode;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputreader;
    [SerializeField] private CoinWallet coinWallet;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private Transform projectileSpawnPosition;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerCollider;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int costToFire = 1;
    [SerializeField] private float fireRate = .75f;
    [SerializeField] private float muzzleFlashDuration = .3f;

    private float previousFireTime;
    private float muzzleFlashTime;
    private bool isFiring;
    private float timer = 0;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        inputreader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        inputreader.PrimaryFireEvent -= HandlePrimaryFire;
    }

    private void Update()
    {
        if (muzzleFlashTime > 0f)
        {
            muzzleFlashTime -= Time.deltaTime;
            if(muzzleFlashTime <= 0f) muzzleFlash.SetActive(false);
        }
        if (!IsOwner) return;
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        if (!isFiring) return;
        if(timer > 0) return;
        if (coinWallet.TotalCoins.Value < costToFire) return;
        PrimaryFireServerRpc(projectileSpawnPosition.position, projectileSpawnPosition.up);
        SpawnDummyProjectile(projectileSpawnPosition.position, projectileSpawnPosition.up);
        timer = 1 / fireRate;
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        isFiring = shouldFire;
    }
    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (coinWallet.TotalCoins.Value < costToFire) return;
        coinWallet.SpendCoin(costToFire);
        GameObject projectileInstance = Instantiate(
            serverProjectilePrefab,
            spawnPos,
            Quaternion.identity);

        projectileInstance.transform.up = direction;
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());
        if (projectileInstance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }
        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
        SpawnDummyProjectileClientRpc(spawnPos, direction);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner) { return; }

        SpawnDummyProjectile(spawnPos, direction);
    }

    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
        GameObject projectileInstance = Instantiate(
            clientProjectilePrefab,
            spawnPos,
            Quaternion.identity);

        projectileInstance.transform.up = direction;
        muzzleFlash.SetActive(true);
        muzzleFlashTime = muzzleFlashDuration;
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());
        
        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
    }
}

