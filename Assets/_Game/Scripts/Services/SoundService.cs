using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundService: MonoBehaviour, ISoundService
{
    [SerializeField] private AudioSource bgSource;
    [SerializeField] private AudioSource effectSouce;

    [SerializeField] private SoundServiceData soundData;

    private Dictionary<BACKGROUND_SOUND, AudioClip> bgSoundDict = new();
    private Dictionary<EFFECT_SOUND, AudioClip> effectSoundDict = new();

    public void Init()
    {
        //Debug.Log("Init");

        for (int i = 0; i < soundData.allBgSounds.Count; i++)
        {
            bgSoundDict.Add(soundData.allBgSounds[i].soundType,
                soundData.allBgSounds[i].soundClip);
        }

        for (int i = 0; i < soundData.allEffectSounds.Count; i++)
        {
            effectSoundDict.Add(soundData.allEffectSounds[i].soundType,
                soundData.allEffectSounds[i].soundClip);
        }
    }

    public void PlaySoundBackground(BACKGROUND_SOUND bgSound)
    {
        if (bgSoundDict.ContainsKey(bgSound))
        {
            bgSource.Stop();
            bgSource.clip = bgSoundDict[bgSound];
            bgSource.Play();
        }
    }

    public void PlaySoundEffect(EFFECT_SOUND effectSound)
    {
        if (effectSoundDict.ContainsKey(effectSound))
        {
            effectSouce.Stop();
            effectSouce.clip = effectSoundDict[effectSound];
            effectSouce.Play();
        }
    }
}