using UnityEngine;

public abstract class Effect_Base : MonoBehaviour
{
    public bool AutoDetect = false;

    [Header("AutoDesctruct")]
    public bool AutoDestructAfterActivation = false;
    public float AutoDestructSec = 5;

    protected virtual void Start()
    {
        if (AutoDetect)
            PerformAutodetectEffects();
    }


	public virtual void Activate()
	{
		if (AutoDestructAfterActivation)
			LaunchAutoDestruct();
	}

    public abstract void Deactivate();


    public void ForceAutoDestruct()
    {
        ForceAutoDestruct(AutoDestructSec);
    }

    public void ForceAutoDestruct(float time)
    {
        if (AutoDestructAfterActivation)
            return;

        AutoDestructSec = time;
        LaunchAutoDestruct();
    }

    protected void LaunchAutoDestruct()
    {
        Destroy(gameObject, AutoDestructSec);
    }


    protected virtual void PerformAutodetectEffects()
    {
    }
}
