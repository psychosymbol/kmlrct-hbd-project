using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
        LoadSounds();
    }

    private Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();

    private void LoadSounds()
    {
        AudioClip[] clipsArray = Resources.LoadAll<AudioClip>("Audios");
        foreach (AudioClip clip in clipsArray)
        {
            audioDictionary.Add(clip.name, clip);
        }
    }
    public void LoadAdditionalSounds(List<AudioClip> audioClips)
    {
        foreach (AudioClip clip in audioClips)
        {
            if (!audioDictionary.ContainsKey(clip.name))
                audioDictionary.Add(clip.name, clip);
        }
    }

    private void Update()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    public static AudioManager GetInstance()
    {
        if (Instance == null)
        {
            Debug.LogWarning("AudioManager instance is null!");
        }
        return Instance;
    }

    public void PlayBGM(string clipname)
    {
        AudioClip clip = audioDictionary[clipname];
        PlayBGM(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySpacialSound(string clipname, System.Action callback = null)
    {
        PlaySound(clipname, Channel.SPECIAL, callback);
    }

    public void PlaySound(string clipname, Channel channel, System.Action callback = null)
    {
        var clip = GetClip(clipname);
        if (clip != null)
        {
            AudioSource source = GetSource(channel);
            source.clip = clip;
            source.Play();
            //Debug.Log("play sound: " + clipname + " on channel: " + channel);
            StartCoroutine(WaitForSoundEnd(clip.length, callback));
        }
    }
    public void PlaySound(AudioClip audioClip, Channel channel, System.Action callback = null)
    {
        var clip = audioClip;
        if (clip != null)
        {
            AudioSource source = GetSource(channel);
            source.clip = clip;
            source.Play();
            //Debug.Log("play sound: " + clipname + " on channel: " + channel);
            StartCoroutine(WaitForSoundEnd(clip.length, callback));
        }
    }

    public void PlaySound(string clipname)
    {
        Debug.Log("Play Sound");
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                PlaySound(clipname, (Channel)sfxSources.IndexOf(source), () => { });
                return;
            }
        }
        Debug.LogWarning("No available SFX channels to play sound " + clipname);
    }
    public void PlaySound(string clipname, System.Action callback = null)
    {
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                PlaySound(clipname, (Channel)sfxSources.IndexOf(source), callback);
                return;
            }
        }
        Debug.LogWarning("No available SFX channels to play sound " + clipname);
    }
    public void PlaySound(AudioClip clip, System.Action callback = null)
    {
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                PlaySound(clip, (Channel)sfxSources.IndexOf(source), callback);
                return;
            }
        }
        Debug.LogWarning("No available SFX channels to play sound " + clip);
    }

    public void StopSound(string clipname)
    {
        foreach (AudioSource source in sfxSources)
        {
            if (source.isPlaying && source.clip.name == clipname)
            {
                source.Stop();
                return;
            }
        }
        Debug.LogWarning("No SFX channel is currently playing sound " + clipname);
    }
    public void StopSound(string clipname, Channel channel)
    {
        AudioSource source = GetSource(channel);
        if (source.isPlaying && source.clip.name == clipname)
        {
            source.Stop();
            return;
        }
        Debug.LogWarning("No SFX channel is currently playing sound " + clipname);
    }
    public void StopSound(Channel channel)
    {
        GetSource(channel).Stop();
    }

    public AudioSource GetSource(Channel channel)
    {
        switch (channel)
        {
            case Channel.BGM:
                return bgmSource;
            case Channel.SPECIAL:
                return spacialSource;
            default:
                return sfxSources[(int)channel];
        }
    }

    private IEnumerator WaitForSoundEnd(float duration, System.Action callback)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
    }

    AudioClip GetClip(string clipname)
    {
        if (audioDictionary.ContainsKey(clipname))
        {
            AudioClip clip = audioDictionary[clipname];
            return clip;
        }
        else
        {
            Debug.LogWarning("Audio clip " + clipname + " not found!");
            return null;
        }
    }

    public float GetClipLength(string clipname)
    {
        var clip = GetClip(clipname);
        if (clip)
            return clip.length;
        else
            return 0;
    }

    public enum Channel
    {
        SFX_1,
        SFX_2,
        SFX_3,
        SFX_4,
        SFX_5,
        SFX_6,
        SFX_7,
        SFX_8,
        SFX_9,
        BGM,
        SPECIAL
    }


    [Header("Additional Resources")]
    public List<AudioClip> additionalSounds = new();

    [Header("Sources")]
    public AudioSource bgmSource;
    public List<AudioSource> sfxSources = new List<AudioSource>();

    public AudioSource spacialSource;
}
