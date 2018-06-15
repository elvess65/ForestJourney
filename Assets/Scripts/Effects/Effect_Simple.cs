using UnityEngine;

/// <summary>
/// Simple effect controller (enable gameObject to start)
/// </summary>
public class Effect_Simple : Effect_Base
{
    [Header("References")]
    public GameObject EffectObj;

    public override void Activate()
    {
        EffectObj.SetActive(true);
    }

    public override void Deactivate()
    {
        EffectObj.SetActive(false);
    }
}
