using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource sfxSource;
    public AudioSource sharedSFXSource;
    public AudioSource musicSource;
    public AudioSource uiSource;

    public float volumeIncrement = 0.1f;

    public float startingVolume = 0.8f;
    public float maxVolume = 1.0f;
    public float minVolume = 0.0f;

    public float musicFadeInTime = 2.0f;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (this == null) { return; }

        AudioListener.volume = startingVolume;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Raise Volume"))
        //{
        //    SetVolume(AudioListener.volume + volumeIncrement);
        //}

        //if (Input.GetButtonDown("Lower Volume"))
        //{
        //    SetVolume(AudioListener.volume - volumeIncrement);
        //}
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = Mathf.Clamp(volume, minVolume, maxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp(volume, minVolume, maxVolume);
    }

    public void StartMusic(float fadeInTime, bool restartIfPlaying)
    {
        if (musicSource.isPlaying)
        {
            if (!restartIfPlaying) { return; } //if we're already playing music, we don't need to start it

            musicSource.Stop();
        }

        StartCoroutine(DoStartMusic(fadeInTime));
    }

    private IEnumerator DoStartMusic(float fadeInTime)
    {
        float startTime = Time.time;
        float elapsedTime = 0.0f;

        float targetVolume = musicSource.volume;
        musicSource.volume = 0.0f;
        musicSource.Play();

        while (elapsedTime < fadeInTime)
        {
            yield return new WaitForSeconds(0.1f);
            elapsedTime = Time.time - startTime;
            musicSource.volume = Mathf.Lerp(0.0f, targetVolume, elapsedTime / fadeInTime);
        }

        musicSource.volume = targetVolume;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic(bool pause)
    {
        if (pause)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.UnPause();
        }
    }

    public void DuckMusic()
    {

    }

    public void UnduckMusic()
    {

    }
    
}
