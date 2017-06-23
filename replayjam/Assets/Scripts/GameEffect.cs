using UnityEngine;
using System.Collections;

public abstract class GameEffect : MonoBehaviour
{

    public abstract void ActivateEffect(GameObject activator, float value, Collision2D coll, Collider2D other);

}
