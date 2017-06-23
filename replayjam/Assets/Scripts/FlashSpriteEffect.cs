using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashSpriteEffect : GameEffect
{

    public SpriteRenderer spriteToFlash;
    public List<SpriteRenderer> spritesToFlash = new List<SpriteRenderer>();

    public Color targetColor = Color.white;
    public float duration = 0.1f;

    public float flickerRate = 0.01f;

    private bool inEffect = false;

    public override void ActivateEffect(GameObject activator, float value, Collision2D coll, Collider2D other)
    {
        if (!inEffect)
        {
            inEffect = true;
            StartCoroutine(DoFlicker());
        }

    }

    private IEnumerator DoFlash()
    {
        Color originalColor = spriteToFlash.color;

        spriteToFlash.color = targetColor;

        yield return new WaitForSeconds(duration);

        spriteToFlash.color = originalColor;

        inEffect = false;
    }

    private IEnumerator DoFlicker()
    {
        float endTime = Time.time + duration;

        List<Color> originalColors = new List<Color>();

        foreach (SpriteRenderer sprite in spritesToFlash)
        {
            originalColors.Add(sprite.color);
        }

        while (Time.time < endTime)
        {
            foreach (SpriteRenderer sprite in spritesToFlash)
            {
                if (sprite != null)
                {
                    sprite.color = targetColor;
                }
            }

            yield return new WaitForSeconds(flickerRate);

            for (int i = 0; i < spritesToFlash.Count; i++)
            {
                SpriteRenderer sprite = spritesToFlash[i];
                if (sprite != null)
                {
                    sprite.color = originalColors[i];
                }
            }

            if (Time.time < endTime)
            {
                yield return new WaitForSeconds(flickerRate);
            }
        }

        inEffect = false;
    }

}
