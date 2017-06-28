using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour {

    public float voiceCooldown = 5.0f;

    private float lastVoice = 0.0f;

    public enum VoiceType
    {
        Taunt,
        Death,
        Grunt,
        Laugh,
        Win
    }

    public List<SoundEffectHandler> taunts;
    public List<SoundEffectHandler> deaths;
    public List<SoundEffectHandler> grunts;
    public List<SoundEffectHandler> laughs;
    public List<SoundEffectHandler> wins;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayVoice(VoiceType type, int playerNumber, bool force)
    {
        if (force || Time.time > lastVoice + voiceCooldown)
        {
            int index = playerNumber - 1;

            switch (type)
            {
                case VoiceType.Taunt:
                    taunts[index].PlayEffect();
                    break;
                case VoiceType.Death:
                    deaths[index].PlayEffect();
                    break;
                case VoiceType.Grunt:
                    grunts[index].PlayEffect();
                    break;
                case VoiceType.Laugh:
                    laughs[index].PlayEffect();
                    break;
                case VoiceType.Win:
                    wins[index].PlayEffect();
                    break;
                default:
                    break;
            }

            lastVoice = Time.time;
        }
    }
}
