using UnityEngine;
using System.Collections;

public class TemporaryInvulnEffect : GameEffect {
    
    public float invulnerableTime = 1.0f;

    public TakesDamage takesDamage;

    public override void ActivateEffect(GameObject activator, float value, Collision2D coll, Collider2D other)
    {
        if (takesDamage != null)
        {
            takesDamage.Invulnerable = true;
            StartCoroutine(MakeVulnerable());
        }
    }

    public IEnumerator MakeVulnerable()
    {
        yield return new WaitForSeconds(invulnerableTime);

        takesDamage.Invulnerable = false;
    }
}
