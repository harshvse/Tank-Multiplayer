using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float turningRate = 270f;

    private Vector2 previousMovementInput;
    private bool isFiring;
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        inputReader.MoveEvent += HandleMove;
        inputReader.PrimaryFireEvent += HandleFire;
    }

    private void Update()
    {
        if (!IsOwner) return;
        float zRotation = previousMovementInput.x * -turningRate * Time.deltaTime;
        bodyTransform.Rotate(0f,0f,zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        rb.velocity = previousMovementInput.y * movementSpeed * (Vector2)bodyTransform.up;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
    }

    private void HandleMove(Vector2 movementInput)
    {
        previousMovementInput = movementInput;
    }

    private void HandleFire(bool firing)
    {
        isFiring = firing;
    }
}
