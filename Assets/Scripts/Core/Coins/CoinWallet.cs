using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<Coin>(out Coin coin))
        {
            int value = coin.Collect();
            if (IsServer) TotalCoins.Value += value;
        }
    }

    public void SpendCoin(int costToFire)
    {
        TotalCoins.Value -= costToFire;
    }
}
