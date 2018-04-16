using UnityEngine;

public class Action_RemoveObstacle : Action_Base
{
    [Header(" - DERRIVED -")]
    public bool RotateCamera = false;
    public ActionComponent_Obstacle ObstacleObj;
    public KeyController.KeyTypes[] ActivationKeys;
    [Header("Projectile")]
    public Projectile_Behaviour ProjectilePrefab;
    public Transform ProjectileSpawnPoint;
    public float ProjectileSpawnOffset;
    public GameObject ExplosionPrefab;
    
    private Projectile_Behaviour m_Projectile;

    public override void Action()
    {
        if (ActivationKeys.Length > 0)
        {
            if (!GameManager.Instance.HasKeysForActivation(ActivationKeys))
            {
                Debug.Log("not enough activation keys");
                return;
            }
        }

        base.Action();

        CreateProjectile();

        if (RotateCamera)
            GameManager.Instance.CamController.RotateRandomly();
    }

    void CreateProjectile()
    {
        Vector3 pos = ProjectileSpawnPoint.position;
        Quaternion rot = Quaternion.LookRotation(pos - ObstacleObj.HitPoint.position);

        m_Projectile = Instantiate(ProjectilePrefab);
        m_Projectile.SetInitTransform(pos, rot);
        m_Projectile.OnImpact += ProjectileImpact_Handler;

        m_Projectile.Launch(ObstacleObj.HitPoint.position);
    }

    void ProjectileImpact_Handler()
    {
        ObstacleObj.gameObject.SetActive(false);
    }
}
