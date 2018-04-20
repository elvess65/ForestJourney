using UnityEngine;

public class Effect_Particles_StopLoop : Effect_Particles
{
    public ParticleSystem[] ParticleSystems;

    public override void Activate()
    {
        base.Activate();
    }

    public override void Deactivate()
    {
        for (int i = 0; i < ParticleSystems.Length; i++)
            ParticleSystems[i].loop = false;
    }
}
