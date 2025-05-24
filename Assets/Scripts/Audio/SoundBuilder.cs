using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundBuilder : PoolableObject
{
    private AudioManager audioManager;
    private float volume = 1f;
    public float pitch = 0.5f;
    private Vector3? position = null;
    private Transform target = null;
    private float spatialBlend = 0f;
    private float minDistance = 1f;
    private float maxDistance = 20f;
    private AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
    private SoundConfig config;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Init(AudioManager audioManager, SoundConfig soundConfig)
    {
        if (soundConfig == null)
        {
            Debug.LogWarning("This config is null");
            return;
        }

        this.audioManager = audioManager;
        config = soundConfig;

        this.volume = soundConfig.volume;
        this.pitch = soundConfig.isRandomPitch ? soundConfig.GetRandomPitch(): soundConfig.pitch;
        
        this.spatialBlend = soundConfig.spatialBlend;

        if (audioManager.IsDebugAudio)
        {
            source.gameObject.name += soundConfig.name;
        }
    }

    public SoundBuilder SetVolume(float volume)
    {
        this.volume = volume;
        return this;
    }

    public SoundBuilder SetPitch(float pitch)
    {
        this.pitch = pitch;
        return this;
    }

    public SoundBuilder SetPosition(Vector3 position)
    {
        this.position = position;
        this.spatialBlend = 1f;
        return this;
    }

    public SoundBuilder SetTarget(Transform target)
    {
        this.target = target;
        this.spatialBlend = 1f;
        return this;
    }

    public SoundBuilder SetSpatialBlend(float blend)
    {
        this.spatialBlend = Mathf.Clamp01(blend);
        return this;
    }

    public SoundBuilder SetRolloff(float min, float max, AudioRolloffMode mode = AudioRolloffMode.Linear)
    {
        this.minDistance = min;
        this.maxDistance = max;
        this.rolloffMode = mode;
        return this;
    }

    public AudioSource Play()
    {
        if (config == null)
        {
            Debug.Log($"Audio Config does not have");
            return null;
        }

        AudioClip clip = config.AudioClip;
        isPlay = true;
       
        source.clip = clip;
        source.pitch = pitch;
        source.volume = volume;
        source.spatialBlend = spatialBlend;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        source.rolloffMode = rolloffMode;

        if (target != null)
        {
            source.transform.SetParent(target);
            source.transform.localPosition = Vector3.zero;
        }
        else if (position.HasValue)
        {
            source.transform.position = position.Value;
        }
        Debug.Log($"Sound Manager play {source.clip.name}");
        source.Play();
        //StartCoroutine(ReturnToPool());
        return source;
    }

    //private IEnumerator ReturnToPool()
    //{
    //    yield return new WaitWhile(() => source.isPlaying);

    //    RecoverSelf();
    //}
    private bool isPlay = false;
    private void Update()
    {
        if (isPlay == false) return;
        if(source != null && source.isPlaying == false)
        {
            isPlay = false;
            RecoverSelf();
        }
    }

    public void SetAudioMixer(AudioMixerGroup audioMixerGroup)
    {
        source.outputAudioMixerGroup = audioMixerGroup;
    }
}
