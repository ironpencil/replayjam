using UnityEngine;
using System.Collections;

public class HealEffect : GameEffect {

    public float healAmount = 25.0f;

    public override void ActivateEffect(GameObject activator, float value, Collision2D coll, Collider2D other)
    {
        TakesDamage takesDamage = activator.GetComponent<TakesDamage>();

        if (takesDamage != null)
        {
            takesDamage.Heal(healAmount);
        }
    }
}
