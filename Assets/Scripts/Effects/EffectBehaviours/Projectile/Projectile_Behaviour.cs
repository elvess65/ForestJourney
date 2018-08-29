using UnityEngine;

/// <summary>
/// Снаряд, который двигаеться либо по пути, либо в цели и при попадании взрываеться
/// </summary>
public class Projectile_Behaviour : FollowPathBehaviour
{
    public Effect_Base EffectImpactPrefab;
    [Tooltip("Графика снаряда без учета следов (Для отключения при попадании и плавных следов)")]
    public GameObject ProjectileGraphics;

    private bool m_Launched = false;
    private Vector3 m_TargetPos;

    private const float m_SQR_DIST_TO_IMPACT = 0.1f;

    public void Launch(Vector3 targetPos, bool curvedPath)
    {
        if (curvedPath)
            MoveAlongPath();
        else
        {
            m_TargetPos = targetPos;
            m_Launched = true;
        }
    }

	public override void EnableEffects(bool state)
	{
	}


    protected override void ImpactHandler()
    {
        base.ImpactHandler();

        //Prefab should handle autodestruct
        Effect_Base effect = Instantiate(EffectImpactPrefab);
        effect.transform.position = transform.position;
        effect.Activate();

        Destroy(effect.gameObject, 2);

        if (DeactivateOnArrival)
        {
            if (ProjectileGraphics == null)
                gameObject.SetActive(false);
            else
                ProjectileGraphics.SetActive(false);
        }
    }

    void Update()
    {
        if (m_Launched)//(GameManager.Instance.IsActive && m_Launched)
        {
            float sqrDistToTarget = (transform.position - m_TargetPos).sqrMagnitude;
            transform.position = Vector3.MoveTowards(transform.position,
                                                     m_TargetPos, 
                                                     Time.deltaTime * Speed);

            if (sqrDistToTarget <= m_SQR_DIST_TO_IMPACT)
            {
                m_Launched = false;
                ImpactHandler();
            }
        }
    }
}
