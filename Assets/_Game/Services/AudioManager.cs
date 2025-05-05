using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IService
{
    private AudioSource musicSource;
    private AudioSource soundsSource;

    public float musicVolume { get; private set; } = 0.5f;
    public float soundVolume { get; private set; } = 0.5f;
    public void Init(bool showWhenInit)
    {
        GameObject mSource = new GameObject("MusicSource")
        {
            transform =
            {
                parent = this.transform
            }
        };
        musicSource = mSource.AddComponent<AudioSource>();
        musicSource.loop = true;
        
        GameObject sSource = new GameObject("AudioSource")
        {
            transform =
            {
                parent = this.transform
            }
        };
        soundsSource = sSource.AddComponent<AudioSource>();
        soundsSource.loop = false;
        
        PlayMusic(R.Audio.LOFIMUSIC);
        
        SetMusicVolume(musicVolume);
        SetSoundVolume(soundVolume);
    }
    public void SetMusicVolume(float value) {
        musicVolume = value;
        musicSource.volume = musicVolume;
    }

    public void SetSoundVolume(float value) {
        soundVolume = value;
        soundsSource.volume = soundVolume;
    }

    public void PlaySound(AudioClip clip) {
        if (clip == null) return;
        soundsSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip) {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.Play();
    }
    public void StopMusic() {
        musicSource.Stop();
    }
    public void PlayWithRandomPitch(AudioClip clip, float d)
    {
        if (clip == null) return;
        GameObject tempAudioObject = new GameObject("TempAudio_" + clip.name);
        DontDestroyOnLoad(tempAudioObject);
        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = soundVolume;
        audioSource.pitch = 1f + Random.Range(-d, d); // Случайный pitch в диапазоне
        audioSource.Play();

        // Уничтожаем объект после завершения воспроизведения
        Destroy(tempAudioObject, clip.length + 0.1f); // Небольшой запас на всякий случай
    }
}
