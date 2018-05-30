using UnityEngine;

public class Projectile_Behaviour : MonoBehaviour
{
    public System.Action OnImpact;

    public float Speed = 10;
    public Effect_Base EffectImpactPrefab;

    private bool m_Launched = false;
    private Vector3 m_TargetPos;
    private const float m_SQR_DIST_TO_IMPACT = 0.1f;

    public void SetInitTransform(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    public void Launch(Vector3 targetPos)
    {
        m_TargetPos = targetPos;
        m_Launched = true;
    }

    void Impact()
    {
        if (OnImpact != null)
            OnImpact();

        //Prefab should handle autodestruct
        Effect_Base effect = Instantiate(EffectImpactPrefab);
        effect.transform.position = transform.position;
        effect.Activate();

        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
    }

    private void OnDestroy()
    {
        //GameManager.Instance.CameraController.FocusAt(GameManager.Instance.GameState.Player.transform);
    }

    private void Update()
    {
        if (GameManager.Instance.IsActive && m_Launched)
        {
            float sqrDistToTarget = (transform.position - m_TargetPos).sqrMagnitude;
            transform.position = Vector3.MoveTowards(transform.position,
                                                     m_TargetPos, 
                                                     Time.deltaTime * Speed);

            if (sqrDistToTarget <= m_SQR_DIST_TO_IMPACT)
            {
                m_Launched = false;

                Impact();
            }
        }
    }
}
