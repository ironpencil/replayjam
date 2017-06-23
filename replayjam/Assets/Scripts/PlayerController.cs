using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public bool invincible = false;
    bool isDead = false;
    public int gamesWon = 0;
    public NotifyController killNotify;

    public SoundEffectHandler dissolveSound;
    public SoundEffectHandler explodeSound;
    public SoundEffectHandler fallSound;

    public enum KillType
    {
        None,
        Dissolve,
        Explode,
        Fall
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isDead)
        {
            int playerNum = gameObject.GetComponent<PlayerInput>().PlayerNum;
            Globals.Instance.GameManager.playerStats[playerNum].survivalTime += Time.time - Globals.Instance.GameManager.roundStartTime;
            Destroy(gameObject);
            Globals.Instance.GameManager.livingPlayers.Remove(playerNum);          
        }
	}

    public void NotifyOfKill()
    {
        killNotify.Notify();
    }

    public bool Kill(KillType killType)
    {
        if (!invincible)
        {
            isDead = true;

            switch (killType)
            {
                case KillType.None:
                    break;
                case KillType.Dissolve:
                    dissolveSound.PlayEffect();
                    break;
                case KillType.Explode:
                    explodeSound.PlayEffect();
                    break;
                case KillType.Fall:
                    fallSound.PlayEffect();
                    break;
                default:
                    break;
            }
        }
        return isDead;
    }
}
