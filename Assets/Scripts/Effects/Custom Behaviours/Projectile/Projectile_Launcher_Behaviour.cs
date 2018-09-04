using UnityEngine;

public class Projectile_Launcher_Behaviour : MonoBehaviour 
{
	public Projectile_Behaviour Projectile;
    public bool RandomPath = false;

    private System.Action m_OnImpact;

    void Start()
    {
        Projectile.OnImpact += Impact_Handler;
    }

    public void LaunchProjectile(Vector3 targetPos, System.Action onImpact)
    {
        m_OnImpact = onImpact;
		Projectile.Launch(targetPos, RandomPath);
    }

    void Impact_Handler()
    {
        if (m_OnImpact != null)
            m_OnImpact();
    }
}
