using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum VolumeType
    {
        EnMaster,
        Music,
        SFX
    }

    [Range(0,1)]
    public float MasterVolume = 1f;
    [Range(0, 1)]
    public float MusicVolume = 1f;
    [Range(0, 1)]
    public float SFXVolume = 1f;

    public static AudioManager Instance;

    AudioSource[] m_AudioSource;
    int m_CurAudioSourceIndex;

    AudioSource m_2DAudioSource;

    ClipLibrary m_ClipLib;
    public AudioClip MainTheme;
    public AudioClip MenuTheme;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            m_AudioSource = new AudioSource[2];
            m_CurAudioSourceIndex = 0;

            for (int i = 0; i < m_AudioSource.Length; i++)
            {
                GameObject newAudioSource = new GameObject("Audio Source " + (i + 1));
                newAudioSource.transform.parent = transform;
                AudioSource AudioS = newAudioSource.AddComponent<AudioSource>();
                m_AudioSource[i] = AudioS;
            }

            GameObject new2DAudioSource = new GameObject("2D Audio Source ");
            new2DAudioSource.transform.parent = transform;
            m_2DAudioSource = new2DAudioSource.AddComponent<AudioSource>();

            m_ClipLib = GetComponent<ClipLibrary>();


            MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

            PlayMusic("Menu");
        }
    }

    public void SetVolume(float volume, VolumeType type)
    {
        switch(type)
        {
            case VolumeType.EnMaster:
                MasterVolume = volume;
                break;
            case VolumeType.Music:
                MusicVolume = volume;
                break;
            case VolumeType.SFX:
                SFXVolume = volume;
                break;
        }

        m_AudioSource[m_CurAudioSourceIndex].volume = MasterVolume * MusicVolume;

        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);

    }

    public void PlaySound(AudioClip audioclip, Vector3 position)
    {
        if(audioclip != null)
            AudioSource.PlayClipAtPoint(audioclip, position, MasterVolume * SFXVolume);
    }

    public void PlaySound(string name, Vector3 postion)
    {
        AudioClip clip = m_ClipLib.GetClipByName(name);
        PlaySound(clip, postion);
    }

    public void Play2DSound(AudioClip audioclip)
    {
        m_2DAudioSource.clip = audioclip;
        m_2DAudioSource.PlayOneShot(audioclip, SFXVolume* MasterVolume);
    }
    public void Play2DSound(string name)
    {
        AudioClip clip = m_ClipLib.GetClipByName(name);
        Play2DSound(clip);
    }

    public void PlayMusic(AudioClip music)
    {
        m_CurAudioSourceIndex = 1 - m_CurAudioSourceIndex;
        m_AudioSource[m_CurAudioSourceIndex].clip = music;
        m_AudioSource[m_CurAudioSourceIndex].Play();
        m_AudioSource[m_CurAudioSourceIndex].loop = true;

        StartCoroutine(MusicCrossFade(1));
    }

    public void PlayMusic(string clipname)
    {
        switch(clipname)
        {
            case "Menu": PlayMusic(MenuTheme);
                break;
            case "Game": PlayMusic(MainTheme);
                break;
        }
    }

    

    IEnumerator MusicCrossFade(float duration)
    {
        float curtime = 0;

        while(curtime < duration)
        {
            float percent = curtime / duration;
            curtime += Time.deltaTime;
            m_AudioSource[m_CurAudioSourceIndex].volume = Mathf.Lerp(0f, MasterVolume * MusicVolume, percent);
            m_AudioSource[1-m_CurAudioSourceIndex].volume = Mathf.Lerp(MasterVolume * MusicVolume, 0f, percent);
            yield return null;
        }
    }
}
