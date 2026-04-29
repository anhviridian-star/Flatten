using System;
using UnityEngine;

public static class Constant
{
    public static readonly float MapWidthGap = 0.5f;
    public static readonly float MapHeightGap = 0.286f;

    public static readonly int LayerMultiplier = 100;

    public static readonly string[] PlayerAnimationNames = 
    {
        "GetUp",
        "Idle",
        "Idle_2",
        "Run",
        "Run_2",
        "Victory"
    };
}

[Serializable]
public enum PlayerAnimationType
{
    GET_UP,
    IDLE,
    IDLE_2,
    RUN,
    RUN_2,
    VICTORY,
    VICTORY_2,
    RUN_3,
    RUN_4,
}