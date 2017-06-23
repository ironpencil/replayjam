using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundEffectHandler : MonoBehaviour
{
    public string description;

    public bool useUISource = false;

    public List<AudioClip> clips;
    public bool playRandom = false;

    public float volume = 1.0f;
    public bool variableVolume = false;
    public float volumeMin = 0.9f;
    public float volumeMax = 1.0f;

    public float pitch = 1.0f;
    public bool variablePitch = false;
    public float pitchMin = 0.8f;
    public float pitchMax = 1.2f;

    public float playDelay = 0.0f;
    public bool variableDelay = false;
    public float playDelayMin = 0.0f;
    public float playDelayMax = 0.01f;
    public bool ignoreTimeScale = false;

    public bool playOneShot = true;
    public bool playOnStart = false;

    public void Start()
    {
        if (playOnStart)
        {
            PlayEffect();
        }
    }

    public void PlayEffect()
    {
        if (playRandom && clips.Count > 1)
        {
            int random = Random.Range(0, clips.Count);
            AudioClip clip = clips[random];
            PlayClip(clip);
        }
        else
        {
            foreach (AudioClip clip in clips)
            {
                PlayClip(clip);
            }
        }
    }

    private void PlayClip(AudioClip clip)
    {
        float clipVolume;
        float clipPitch;
        float clipDelay;

        if (variableVolume)
        {
            clipVolume = Random.Range(volumeMin, volumeMax);
        }
        else
        {
            clipVolume = volume;
        }

        if (variablePitch)
        {
            clipPitch = Random.Range(pitchMin, pitchMax);
        } else
        {
            clipPitch = pitch;
        }
        clipPitch = 1;

        if (variableDelay)
        {
            clipDelay = Random.Range(playDelayMin, playDelayMax);
        }
        else
        {
            clipDelay = playDelay;
        }

        if (clipDelay > 0.0f)
        {
            StartCoroutine(WaitThenPlay(clip, clipVolume, clipPitch, clipDelay));
        }
        else
        {
            Play(clip, clipVolume, clipPitch);
        }
    }

    private void Play(AudioClip clip, float clipVolume, float clipPitch)
    {
        AudioSource source;

        if (playOneShot)
        {
            if (useUISource)
            {
                source = AudioManager.Instance.uiSource;
            }
            else {
                source = AudioManager.Instance.sfxSource;
            }
            source.pitch = clipPitch;
            source.PlayOneShot(clip, clipVolume);
        }
        else
        {
            if (useUISource)
            {
                source = AudioManager.Instance.uiSource;
            }
            else {
                source = AudioManager.Instance.sharedSFXSource;
            }
            source.clip = clip;
            source.volume = clipVolume;
            source.pitch = clipPitch;
            source.loop = true;
            source.Play();
        }
    }



    private IEnumerator WaitThenPlay(AudioClip clip, float clipVolume, float clipPitch, float clipDelay)
    {
        if (ignoreTimeScale)
        {
            float playTime = Time.realtimeSinceStartup + clipDelay;

            while (playTime > Time.realtimeSinceStartup)
            {
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(clipDelay);
        }

        Play(clip, clipVolume, clipPitch);
    }

}
