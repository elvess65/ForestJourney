using System.Collections;
using UnityEngine;

public class TargetDelayedProjectile : MonoBehaviour
{
    [Header("Links")]
    public Collider Target;
    public Transform ToImpactTimer;
    public Transform TargetGraphics;
    public Projectile_Launcher_Behaviour ProjectileLauncher;
    [Header("Settings")]
    public float SpawnDelay;
    public float MoveSpeed;
    public float AreaExistenceDelay;

    private float m_TimeToTarget;
    private Utils.InterpolationData<float> m_LerpData;

	void Start ()
    {
        //На старте выключить снаряд и цель
        ProjectileLauncher.gameObject.SetActive(false);
        Target.gameObject.SetActive(false);

        //Время полета до цели
        m_TimeToTarget = Vector3.Distance(ProjectileLauncher.Projectile.transform.position, Target.transform.position) / ProjectileLauncher.Projectile.Speed;
    }

    public void LaunchProjectile()
    {
        //При запуске включить цель
        Target.gameObject.SetActive(true);

        //Ожидание перед выстрелом
        StartCoroutine(WaitSpawnDelayTime());
    }

    void ImpactHandler()
    {
        Debug.Log("Hit");

        Target.enabled = true;

        StartCoroutine(WaitAreaExistanceDelayTime());
    }

    IEnumerator WaitSpawnDelayTime()
    {
        yield return new WaitForSeconds(SpawnDelay);

        //Включить и запустить снаряд
        ProjectileLauncher.gameObject.SetActive(true);
        ProjectileLauncher.LaunchProjectile(Target.transform.position, ImpactHandler);

        //Данные для анимации цели (отсчет до выстрела)
        m_LerpData.TotalTime = m_TimeToTarget;
        m_LerpData.From = ToImpactTimer.localScale.x; 
        m_LerpData.To = 0;

        //Начало анимации
        m_LerpData.Start();
    } 

    IEnumerator WaitAreaExistanceDelayTime()
    {
        yield return new WaitForSeconds(AreaExistenceDelay);

        Target.enabled = false;
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            LaunchProjectile();

		if (m_LerpData.IsStarted)
        {
            m_LerpData.Increment();
            float scale = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
            ToImpactTimer.transform.localScale = new Vector3(scale, scale, ToImpactTimer.transform.localScale.z);

            if (m_LerpData.Overtime())
            {
                m_LerpData.Stop();

                //Выключить эффект ожидания для цели
                TargetGraphics.gameObject.SetActive(false);
            }
        }
	}
}
