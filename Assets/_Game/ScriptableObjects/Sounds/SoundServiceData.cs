using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundServiceData", menuName = "Scriptable Objects/SoundServiceData")]
public class SoundServiceData : ScriptableObject
{
    public List<BackgroundSoundData> allBgSounds;
    public List<EffectSoundData> allEffectSounds;
}

[Serializable]
public struct BackgroundSoundData
{
    public BACKGROUND_SOUND soundType;
    public AudioClip soundClip;
}

[Serializable]
public struct EffectSoundData
{
    public EFFECT_SOUND soundType;
    public AudioClip soundClip;
}

[Serializable]
public enum BACKGROUND_SOUND
{
    HOME_BG,
    GAME_BG
}

[Serializable]
public enum EFFECT_SOUND
{
    BTN_CLICK,
    PLAYER_MOVE,
    WIN_GAME,
}
