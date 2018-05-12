using UnityEngine;

/// <summary>
/// Триггер удаления преграды
/// </summary>
public class Action_RemoveObstacle : ActionTrigger
{
    [Header(" - DERRIVED -")]
    [Tooltip("Вращать ли камеру при взаимодействии")]
    public bool RotateCamera = false;
    public ActionComponent_Obstacle ObstacleObj;

    [Header("Projectile")]
    public Projectile_Behaviour ProjectilePrefab;
    public Transform ProjectileSpawnPoint;
    public float ProjectileSpawnOffset;
    public GameObject ExplosionPrefab;
    
    private Projectile_Behaviour m_Projectile;

    public override void Interact()
    {
 

        //Создать объект, который разарушит преграду
        CreateObject();

        base.Interact();
    }

    void CreateObject()
    {
        Vector3 pos = ProjectileSpawnPoint.position;
        Quaternion rot = Quaternion.LookRotation(pos - ObstacleObj.HitPoint.position);

        m_Projectile = Instantiate(ProjectilePrefab);
        m_Projectile.SetInitTransform(pos, rot);
        m_Projectile.OnImpact += ProjectileImpact_Handler;

        GameManager.Instance.CameraController.FocusAt(m_Projectile.transform);
        m_Projectile.Launch(ObstacleObj.HitPoint.position);
    }

    void ProjectileImpact_Handler()
    {
        ObstacleObj.gameObject.SetActive(false);
    }
}
