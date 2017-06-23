using UnityEngine;
using System.Collections;

public abstract class BaseMovement : MonoBehaviour
{
    public float speed;

    public Vector2 movementDirection = Vector2.zero;

    public bool disabled = false;

    public bool moveDuringUpdate = true;

    public abstract void Move();
}

