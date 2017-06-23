using UnityEngine;
using System.Collections;

public class PauseGameEffect : GameEffect {

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
            Globals.Instance.Pause(pause);
        }
    }

    private IEnumerator WaitThenPause(bool pause, float delay)
    {

        float pauseTime = Time.realtimeSinceStartup + delay;

        while (pauseTime > Time.realtimeSinceStartup)
        {
            yield return null;
        }

        Globals.Instance.Pause(pause);

    }

    
}
