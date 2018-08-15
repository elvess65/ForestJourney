﻿using UnityEngine;

/// <summary>
/// Останавливает проигрывание списка партиклов
/// </summary>
public class Effect_Particles_Stop : Effect_Base
{
    [Space(10)]
    public ParticleSystem[] ParticleSystems;

    public override void Activate()
    {
        base.Activate();

		for (int i = 0; i < ParticleSystems.Length; i++)
            ParticleSystems[i].Play();
    }

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
