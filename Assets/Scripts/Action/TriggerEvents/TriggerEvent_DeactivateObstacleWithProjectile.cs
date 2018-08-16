using UnityEngine;

/// <summary>
/// Выключить объект после попадания снаряда
/// </summary>
public class TriggerEvent_DeactivateObstacleWithProjectile : TriggerAction_Event
{
    [Space(10)]
    public Obstacle_Base ObstacleObject;
    public Transform HitPoint;
    public Projectile_Launcher_Behaviour ProjectileLauncher;

    protected override void CallEvent()
    {
        CreateProjectile();
    }

    void CreateProjectile()
    {
        ProjectileLauncher.LaunchProjectile(HitPoint.position, ProjectileImpact_Handler);
    }

    void ProjectileImpact_Handler()
    {
        ObstacleObject.DisableObstacle();
    }
}
   