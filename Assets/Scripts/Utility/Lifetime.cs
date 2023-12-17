using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    private float _currentLifetime = 0f;

    private void Update()
    {
        _currentLifetime += Time.deltaTime;
        if(_currentLifetime > duration) GameObject.Destroy(gameObject);
    }
}
