using System;
using UnityEngine;

public interface ISoundService
{
    void PlaySoundBackground(BACKGROUND_SOUND bgSound) { }
    void PlaySoundEffect(EFFECT_SOUND effectSound) { }
}