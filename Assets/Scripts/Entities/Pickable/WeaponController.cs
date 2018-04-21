using System.Collections.Generic;
using UnityEngine;

public class WeaponController : Item_Base
{
    [Header("Idle")]
    public Effect_Base EffectPick;
    public Effect_Base EffectIdle;
    [Header("Pick")]
    public float Speed = 5;
    public Effect_Base EffectPicked;
    public Vector3 PlayerOffset;
    [Header("Use")]
    public Effect_Base EffectUse;
    public Vector3 UseOffset;
    public float AffectRange = 5;
    public float AffectDelay = 2;

    private List<EnemyController> m_EnemiesInRange;

    protected override void Start()
    {
    }

    protected override void Update()
    {
        if (!GameManager.Instance.IsActive)
            return;

        if (m_Picked)
        {
            transform.position = Vector3.Slerp(transform.position,
                                               GameManager.Instance.GameState.Player.transform.position - PlayerOffset,
                                               Time.deltaTime * Speed);
        }
        else if (m_Using)
        {
            transform.position = Vector3.Slerp(transform.position,
                                               GameManager.Instance.GameState.Player.transform.position - UseOffset,
                                               Time.deltaTime * Speed);
        }
    }

    public override void Interact()
    {
        if (GameManager.Instance.GameState.Player.TryAddWeapon(this))
        {
            base.Interact();

            EffectIdle.Deactivate();
            EffectIdle.ForceAutoDestruct();

            EffectPick.Activate();
            EffectPicked.Activate();
        }
    }

    public override void Use()
    {
        base.Use();

        UseWeapon();

        EffectPicked.Deactivate();
        EffectUse.Activate();

        Destroy(gameObject, 5);
    }

    void UseWeapon()
    {
        AddEnemiesInRange();
        StartCoroutine(RemoveEnemiesWithDelay(AffectDelay));
    }

    System.Collections.IEnumerator RemoveEnemiesWithDelay(float time)
    {
        yield return new WaitForSeconds(time);

        AddEnemiesInRange();

        foreach (EnemyController enemy in m_EnemiesInRange)
            enemy.Destroy();
    }

    void AddEnemiesInRange()
    {
        m_EnemiesInRange = new List<EnemyController>();
        foreach (EnemyController enemy in GameManager.Instance.GameState.Enemies)
        {
            float distToEnemy = Vector3.Distance(GameManager.Instance.GameState.Player.transform.position, enemy.transform.position);
            if (distToEnemy <= AffectRange)
            {
                m_EnemiesInRange.Add(enemy);
                enemy.Pause();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, AffectRange);
    }
}
