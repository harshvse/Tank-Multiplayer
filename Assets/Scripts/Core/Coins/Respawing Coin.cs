using System;
using UnityEngine;

public class RespawingCoin : Coin
{

    public Action<RespawingCoin> OnCollected;
    private Vector3 previousPosition;
    
    private void Update()
    {
        if (previousPosition != transform.position)
        {
            Show(true);
        }

        previousPosition = transform.position;
    }
    public override int Collect()
    {
        if (!IsServer)
        {
            return 0;
        }
        if (alreadyCollected) return 0;
        Show(false);
        alreadyCollected = true;
        OnCollected?.Invoke(this);
        return coinValue;
    }

    public void Reset()
    {
        alreadyCollected = false;
    }
}
