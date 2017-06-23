using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour, Explosive {
    public float FallTime = 2.0f;
    public GameObject ExplosionPrefab;
    public GameObject shadow;
    public AnimationCurve easing;
    public int targetPlayerNum;
    public SoundEffectHandler fallingSound;

    Vector2 startingPos;
    Vector2 targetPos;
    float elapsedTime = 0.0f;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
        targetPos = shadow.transform.position;

        float soundDuration = fallingSound.clips[0].length;
        fallingSound.playDelay = Mathf.Max(FallTime - soundDuration);
        fallingSound.PlayEffect();
    }

    // Update is called once per frame
    void Update() {
        elapsedTime += Time.deltaTime;
        // Once we get to the shadow, blow up
        if (elapsedTime < FallTime)
        {
            float easedTime = easing.Evaluate(elapsedTime / FallTime);
            transform.position = Vector2.Lerp(startingPos, targetPos, easedTime);
        } else
        {
            Transform DynamicsParent = Globals.Instance.GameManager.dynamicsParent;
            GameObject explosion = Instantiate(ExplosionPrefab, DynamicsParent);
            explosion.transform.position = targetPos;
            explosion.gameObject.GetComponent<ExplosionBehavior>().explosives.Add(this);

            Destroy(shadow);
            //Don't destroy bomb until explosion complete
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    public void KilledPlayer(int playerNum)
    {
        Globals.Instance.GameManager.AddKill(targetPlayerNum, playerNum, Kill.Weapon.Bomb);
    }

    public void StartExploding()
    {
        // Do nothing
    }

    public void EndExploding()
    {
        Destroy(gameObject);
    }
}
