using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Mixers")]
    [SerializeField] private AudioMixer mainMixer;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup sfxNoTimeEffectGroup;

    [Header("Time Scale Settings")]
    [SerializeField] private float minPitch = 0.5f;

    [SerializeField] private float maxPitch = 1f;
    [SerializeField] private float lowPassCutoffMin = 500f;
    [SerializeField] private float lowPassCutoffMax = 22000f;

    [Header("Transition Settings")]
    [SerializeField] private float pitchLerpSpeed = 10f;

    [SerializeField] private float filterLerpSpeed = 8f;

    private float targetPitch = 1f;
    private float currentPitch = 1f;
    private float targetCutoff = 22000f;
    private float currentCutoff = 22000f;

    [SerializeField] private AudioData audioData;
    [SerializeField] private nObjectPool audioPool;

    public bool IsDebugAudio = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void InitializeAudio()
    {
        // Setup audio sources if not assigned
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.outputAudioMixerGroup = musicMixerGroup;
            musicSource.loop = true;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = sfxMixerGroup;
        }
    }

    private void Update()
    {
        UpdateTimeScale(Time.timeScale);
        UpdateTimeScaleEffects();
    }

    public void UpdateTimeScale(float timeScale)
    {
        // Calculate target pitch based on time scale
        targetPitch = Mathf.Lerp(minPitch, maxPitch, timeScale);

        // Calculate low pass filter cutoff frequency
        targetCutoff = Mathf.Lerp(lowPassCutoffMin, lowPassCutoffMax, timeScale);
    }

    private const string MasterPitch = "MasterPitch";
    private const string LowPassCutoff = "LowPassCutoff";

    private void UpdateTimeScaleEffects()
    {
        // Smoothly interpolate pitch
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.unscaledDeltaTime * pitchLerpSpeed);

        // Smoothly interpolate low pass filter
        currentCutoff = Mathf.Lerp(currentCutoff, targetCutoff, Time.unscaledDeltaTime * filterLerpSpeed);

        // Apply effects to audio mixer
        var isContainMasterPitch = mainMixer.SetFloat(MasterPitch, currentPitch);
        var isContainLowPassCutoff = mainMixer.SetFloat(LowPassCutoff, currentCutoff);
        //Debug.Log("isContainMasterPitch: " + isContainMasterPitch + " isContainLowPassCutoff: " + isContainLowPassCutoff);
    }

    public void PlayMusic(SoundConfig soundConfig)
    {
        if (soundConfig == null)
        {
            StopMusic(0.2f);
            return;
        }

        if (musicSource.isPlaying)
        {
            StopMusic(0.2f, () =>
            {
                musicSource.volume = soundConfig.volume;
                musicSource.clip = soundConfig.AudioClip;
                musicSource.Play();
            });
            return;
        }

        musicSource.volume = soundConfig.volume;
        musicSource.clip = soundConfig.AudioClip;
        musicSource.Play();
    }

    public void StopMusic(float duration, Action callback = null)
    {
        //musicSource.DOFade(0, duration).SetUpdate(true).OnComplete(() =>
        //{
        //    musicSource.Stop();
        //    callback?.Invoke();
        //});
    }

    public void SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    [SerializeField] private SoundBuilder soundBuilderPrefab;
    public SoundBuilder CreateSound(SoundConfig soundConfig, bool timeAffect = true)
    {
        Debug.Log("Create Sound Builder with: " + soundConfig.name);
        var soundBuilder = ObjectPoolManager.GetObject<SoundBuilder>(soundBuilderPrefab);
        soundBuilder.Init(this, soundConfig);
        soundBuilder.SetAudioMixer(timeAffect ? sfxMixerGroup : sfxNoTimeEffectGroup);
        return soundBuilder;
    }


    public SoundConfig GetSoundConfigByName(string name)
    {
        foreach (var item in audioData.SoundConfigs)
        {
            if (item.name == name)
                return item;
        }

        Debug.Log("cannot find sound config named: " + name);
        return null;
    }
}