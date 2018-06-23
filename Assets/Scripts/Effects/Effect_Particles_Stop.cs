﻿using UnityEngine;

/// <summary>
/// Останавливает проигрывание списка партиклов
/// </summary>
public class Effect_Particles_Stop : Effect_Base
{
    public ParticleSystem[] ParticleSystems;

    public override void Deactivate()
    {
        for (int i = 0; i < ParticleSystems.Length; i++)
            ParticleSystems[i].Stop();
    }

    protected override void PerformAutodetectEffects()
    {
        ParticleSystems = GetComponentsInChildren<ParticleSystem>();
    }
}
