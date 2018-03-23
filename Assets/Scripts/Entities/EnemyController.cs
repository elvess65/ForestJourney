using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public BoxCollider Collider;
    [Header("Effects")]
    public Effect_Base MainEffect;
    public Effect_Base MainLightEffect;
    public Effect_Base DestroyEffect;
    public Effect_Base DamageEffectPrefab;

    private bool m_IsActive = false;
    private NavMeshAgent m_Agent;

	void Start ()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    public void Init()
    {
        StartCoroutine(DelayAndActivate(1));
        GameManager.Instance.AddEnemy(this);
    }

    public void TakeDamage(Transform target)
    {
        Instantiate(DamageEffectPrefab, target.position, Quaternion.identity).Activate();

        StartCoroutine(DelayAndRestart(2));
    }

    public void Pause()
    {
        m_Agent.enabled = false;

        Collider.enabled = false;
        enabled = false;
    }

    public void Destroy()
    {
        MainEffect.Deactivate();
        MainLightEffect.Deactivate();

        DestroyEffect.Activate();
        Destroy(gameObject, 3);
    }

    IEnumerator DelayAndRestart(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.RestartRound();
    }

    IEnumerator DelayAndActivate(float time)
    {
        yield return new WaitForSeconds(time);
        Activate();
    }

    void Activate()
    {
        m_IsActive = true;

    }

    void FixedUpdate()
    {
        if (GameManager.Instance.IsActive)
        {
            if (m_IsActive)
                m_Agent.destination = GameManager.Instance.Player.transform.position;
        }
    }
}
