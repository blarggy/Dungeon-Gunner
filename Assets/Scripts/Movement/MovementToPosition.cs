using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementToPositionEvent))]
[DisallowMultipleComponent]
public class MovementToPosition : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private MovementToPositionEvent movementToPositionEvent;

    private void Awake()
    {
        // Load components
        rigidBody2D = GetComponent<Rigidbody2D>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
    }

    private void OnEnable()
    {
        // Subscribe to movement to position event
        movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;
    }

    private void OnDisable()
    {
        // UnSubscribe to movement to position event
        movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;
    }

    // On movement event
    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs movementToPositionArgs)
    {
        MoveRigidBody(movementToPositionArgs.movePosition, movementToPositionArgs.currentPosition, movementToPositionArgs.moveSpeed);
    }

    /// <summary>
    /// Move rigid body component <br/>
    /// Move the rigid body a small distance in the movement direction
    /// </summary>
    /// <param name="movePosition"></param>
    /// <param name="currentPosition"></param>
    /// <param name="moveSpeed"></param>
    private void MoveRigidBody(Vector3 movePosition, Vector3 currentPosition, float moveSpeed)
    {
        Vector2 unitVector = Vector3.Normalize(movePosition - currentPosition); // returns a direction vector of magnitude 1 (because it's normalized)
        rigidBody2D.MovePosition(rigidBody2D.position + (unitVector * moveSpeed * Time.fixedDeltaTime));
    }
}
