using UnityEngine;

public class TriggerEvent_RemoveObstacleWithProjectile : TriggerAction_Event
{
    [Header(" - DERRIVED -")]
    public GameObject ObstacleObject;
    public Transform HitPoint;
    [Header("Projectile")]
    public Projectile_Behaviour ProjectilePrefab;
    public Transform ProjectileSpawnPoint;
    public GameObject ExplosionPrefab;

    private Projectile_Behaviour m_Projectile;

    public override void StartEvent()
    {
        CreateProjectile();
    }

    void CreateProjectile()
    {
        Vector3 pos = ProjectileSpawnPoint.position;
        Quaternion rot = Quaternion.LookRotation(pos - HitPoint.position);

        m_Projectile = Instantiate(ProjectilePrefab);
        m_Projectile.SetInitTransform(pos, rot);
        m_Projectile.OnImpact += ProjectileImpact_Handler;

        GameManager.Instance.CameraController.FocusAt(m_Projectile.transform);
        m_Projectile.Launch(HitPoint.position);
    }

    void ProjectileImpact_Handler()
    {
        ObstacleObject.SetActive(false);
    }
}
