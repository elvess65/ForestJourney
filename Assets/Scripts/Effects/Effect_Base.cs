using UnityEngine;

public abstract class Effect_Base : MonoBehaviour
{
    [Header("AutoDesctruct")]
    public bool AutoDestructAfterActivation = false;
    public float AutoDestructSec = 5;

    public abstract void Deactivate();

    public virtual void Activate()
    {
		if (AutoDestructAfterActivation)
			LaunchAutoDestruct();
    }

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
}
