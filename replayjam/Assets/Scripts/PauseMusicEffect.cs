using UnityEngine;
using System.Collections;

public class PauseMusicEffect : GameEffect {

    public bool pause = true;

    public float delay = 0.0f;

    public override void ActivateEffect(GameObject activator, float value, Collision2D coll, Collider2D other)
    {
        if (delay > 0.0f)
        {
            StartCoroutine(WaitThenPause(pause, delay));
        }
        else
        {
            AudioManager.Instance.PauseMusic(pause);
        }
    }

    private IEnumerator WaitThenPause(bool pause, float delay)
    {

        float pauseTime = Time.realtimeSinceStartup + delay;

        while (pauseTime > Time.realtimeSinceStartup)
        {
            yield return null;
        }

        AudioManager.Instance.PauseMusic(pause);

    }
}
