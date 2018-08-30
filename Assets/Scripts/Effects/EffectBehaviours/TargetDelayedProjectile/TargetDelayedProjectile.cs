using System.Collections;
using UnityEngine;

public class TargetDelayedProjectile : MonoBehaviour
{
    [Header("Links")]
    public Collider TargetCollider;      //Коллайдер, который наносит урон
    public GameObject TargetGraphics;    //Графика (без коллайдера)
    public GameObject ToImpactTimer;     //Таймер до попадения (часть графики)
    public Projectile_Launcher_Behaviour ProjectileLauncher;
    [Header("Settings")]
    public float SpawnDelay;                //Задержка перед выстрелом
    public float DamageAreaDestroyDelay;    //Задержка перед удалением зоны, которая наносит урон

    private bool m_IsLaunching = false;
    private float m_TimeToTarget;
    private float m_ImpactTimerInitScale;
    private Utils.InterpolationData<float> m_LerpData;

    private WaitForSeconds m_WaitSpawnDelayTime = null;
    private WaitForSeconds m_WaitAreaExistanceDelayTime = null;

    void Start ()
    {
        m_WaitSpawnDelayTime = new WaitForSeconds(SpawnDelay);
        m_WaitAreaExistanceDelayTime = new WaitForSeconds(DamageAreaDestroyDelay);

        //На старте выключить снаряд и цель
        ProjectileLauncher.gameObject.SetActive(false);
        TargetCollider.gameObject.SetActive(false);
        TargetGraphics.SetActive(false);

        //Время полета до цели
        m_TimeToTarget = Vector3.Distance(ProjectileLauncher.Projectile.transform.position, TargetCollider.transform.position) / ProjectileLauncher.Projectile.Speed;
        //Начальный размер таймера (для перезапуска)
        m_ImpactTimerInitScale = ToImpactTimer.transform.localScale.x;
    }

    public void LaunchProjectile()
    {
        if (m_IsLaunching)
            return;

        m_IsLaunching = true;

        //При запуске включить цель
        TargetGraphics.gameObject.SetActive(true);

        //Вернуть таймер в начальную позицию
        ToImpactTimer.transform.localScale = new Vector3(m_ImpactTimerInitScale, m_ImpactTimerInitScale, ToImpactTimer.transform.localScale.z);

        //Ожидание перед выстрелом
        StartCoroutine(WaitSpawnDelayTime());
    }

    void ImpactHandler()
    {
        //Включить зону поражения
        TargetCollider.enabled = true;

        //Выключить эффект ожидания для цели
        TargetGraphics.gameObject.SetActive(false);

        StartCoroutine(WaitAreaExistanceDelayTime());

        m_IsLaunching = false;
    }

    IEnumerator WaitSpawnDelayTime()
    {
        yield return m_WaitSpawnDelayTime;

        //Включить и запустить снаряд
        ProjectileLauncher.gameObject.SetActive(true);
        ProjectileLauncher.LaunchProjectile(TargetCollider.transform.position, ImpactHandler);

        //Данные для анимации цели (отсчет до выстрела)
        m_LerpData.TotalTime = m_TimeToTarget;
        m_LerpData.From = ToImpactTimer.transform.localScale.x; 
        m_LerpData.To = 0;

        //Начало анимации
        m_LerpData.Start();
    } 

    IEnumerator WaitAreaExistanceDelayTime()
    {
        yield return m_WaitAreaExistanceDelayTime;

        //Выключить зону поражения
        TargetCollider.enabled = false;
    }

    void Update ()
    {
		if (m_LerpData.IsStarted)
        {
            m_LerpData.Increment();
            float scale = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
            ToImpactTimer.transform.localScale = new Vector3(scale, scale, ToImpactTimer.transform.localScale.z);

            if (m_LerpData.Overtime())
                m_LerpData.Stop();
        }
	}
}
