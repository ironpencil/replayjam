using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource sfxSource;
    public AudioSource sharedSFXSource;
    public AudioSource musicSource1;
    public AudioSource musicSource2;
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

        StartMusic(musicSource1, 2.0f, true);
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

    public void StartGameMusic()
    {
        StartMusic(musicSource2, 0.5f, true);
        StopMusic(musicSource1, 0.5f);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = Mathf.Clamp(volume, minVolume, maxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        //musicSource1.volume = Mathf.Clamp(volume, minVolume, maxVolume);
    }

    public void StartMusic(AudioSource musicSource, float fadeInTime, bool restartIfPlaying)
    {
        if (musicSource.isPlaying)
        {
            if (!restartIfPlaying) { return; } //if we're already playing music, we don't need to start it

            musicSource.Stop();
        }

        StartCoroutine(DoStartMusic(musicSource, fadeInTime));
    }

    private IEnumerator DoStartMusic(AudioSource musicSource, float fadeInTime)
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

    public void StopMusic(AudioSource musicSource, float fadeOutTime)
    {
        if (musicSource.isPlaying)
        {
            StartCoroutine(DoStopMusic(musicSource, fadeOutTime));
        }        
    }

    private IEnumerator DoStopMusic(AudioSource musicSource, float fadeOutTime)
    {
        float startTime = Time.time;
        float elapsedTime = 0.0f;

        float targetVolume = 0.0f;
        float startVolume = musicSource.volume;

        while (elapsedTime < fadeOutTime)
        {
            yield return new WaitForSeconds(0.1f);
            elapsedTime = Time.time - startTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeOutTime);
        }

        musicSource.volume = targetVolume;
        musicSource.Stop();
    }

    public void PauseMusic(bool pause)
    {
        if (pause)
        {
            musicSource1.Pause();
        }
        else
        {
            musicSource1.UnPause();
        }
    }

    public void DuckMusic()
    {

    }

    public void UnduckMusic()
    {

    }
    
}
