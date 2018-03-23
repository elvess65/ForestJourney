using UnityEngine;

/// <summary>
/// Simple particle effect controller (enable gameObject to start)
/// </summary>
public class Effect_Particles : Effect_Base
{
    [Header("References")]
    public ParticleSystem MainParticleSystemObj;

    public override void Activate()
    {
        MainParticleSystemObj.gameObject.SetActive(true);

        if (AutoDestructAfterActivation)
            LaunchAutoDestruct();
    }

    public override void Deactivate()
    {
        MainParticleSystemObj.gameObject.SetActive(false);
    }
}
