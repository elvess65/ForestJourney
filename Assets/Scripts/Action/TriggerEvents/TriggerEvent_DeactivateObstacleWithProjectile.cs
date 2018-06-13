using UnityEngine;

public class TriggerEvent_DeactivateObstacleWithProjectile : TriggerAction_Event
{
    [Header(" - DERRIVED -")]
    public GameObject ObstacleObject;
    public Transform HitPoint;
    public Projectile_Launcher_Behaviour ProjectileLauncher;

    public override void StartEvent()
    {
        CreateProjectile();
    }

    void CreateProjectile()
    {
        ProjectileLauncher.LaunchProjectile(HitPoint.position, ProjectileImpact_Handler);
    }

    void ProjectileImpact_Handler()
    {
        ObstacleObject.SetActive(false);
    }
}
