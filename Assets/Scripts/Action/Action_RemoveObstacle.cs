using UnityEngine;

/// <summary>
/// Триггер удаления преграды
/// </summary>
public class Action_RemoveObstacle : Action_Base
{
    [Header(" - DERRIVED -")]
    [Tooltip("Вращать ли камеру при взаимодействии")]
    public bool RotateCamera = false;
    public ActionComponent_Obstacle ObstacleObj;
    [Tooltip("Массив ключей, необходимых для активации")]
    public KeyController.KeyTypes[] ActivationKeys;

    [Header("Projectile")]
    public Projectile_Behaviour ProjectilePrefab;
    public Transform ProjectileSpawnPoint;
    public float ProjectileSpawnOffset;
    public GameObject ExplosionPrefab;
    
    private Projectile_Behaviour m_Projectile;

    public override void Interact()
    {
        //Проверка на достаточное количество ключей
        if (ActivationKeys.Length > 0)
        {
            if (!GameManager.Instance.GameState.HasKeysForActivation(ActivationKeys))
            {
                Debug.Log("not enough activation keys");
                return;
            }
        }

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

        m_Projectile.Launch(ObstacleObj.HitPoint.position);
    }

    void ProjectileImpact_Handler()
    {
        ObstacleObj.gameObject.SetActive(false);
    }
}
