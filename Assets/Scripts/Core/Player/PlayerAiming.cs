using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] private Transform turretTransform;
    [SerializeField] private InputReader inputReader;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        Vector2 aimScreenPosition = inputReader.AimPosition;
        Vector2 aimWorldPosition = _camera.ScreenToWorldPoint(aimScreenPosition);

        turretTransform.up = new Vector2(aimWorldPosition.x - turretTransform.position.x,
            aimWorldPosition.y - turretTransform.position.y);
    }
}
