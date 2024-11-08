using UnityEngine;
using System;

[DisallowMultipleComponent]
public class MovementToPositionEvent : MonoBehaviour
{
    public event Action<MovementToPositionEvent, MovementToPositionArgs> OnMovementToPosition;

    // Wrapper
    public void CallMovementToPositionEvent(Vector3 movePosition, Vector3 currentPosition, float moveSpeed, Vector2 moveDirection, bool isRolling = false)
    {
        OnMovementToPosition?.Invoke(this, new MovementToPositionArgs() { movePosition = movePosition, currentPosition = currentPosition, moveSpeed = moveSpeed, moveDirection = moveDirection, isRolling = isRolling });
    }
}

public class MovementToPositionArgs : EventArgs
{
    public Vector3 movePosition; // where I want to move to
    public Vector3 currentPosition;
    public float moveSpeed;
    public Vector2 moveDirection;
    public bool isRolling;
}