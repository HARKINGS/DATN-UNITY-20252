using System;
using Unity.Mathematics;
using UnityEngine;

public static class CombatEvents
{
    public static Action<SkillBase> OnPlayerSkillUsed;
    public static Action<AudioClip> OnSoundRequested;
    public static Action<bool> OnGameEnded;
}