using UnityEngine;
using System.Collections;

public class PlaySoundEffect : GameEffect {

    public string description;

    public SoundEffectHandler soundEffect;

    public override void ActivateEffect(GameObject activator, float value, Collision2D coll, Collider2D other)
    {
        if (soundEffect != null)
        {
            soundEffect.PlayEffect();
        }
    }
}
