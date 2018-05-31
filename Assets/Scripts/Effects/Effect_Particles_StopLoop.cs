using UnityEngine;

public class Effect_Particles_StopLoop : Effect_Base
{
    public ParticleSystem[] ParticleSystems;

    public override void Deactivate()
    {
        for (int i = 0; i < ParticleSystems.Length; i++)
            ParticleSystems[i].Stop();
    }
}
