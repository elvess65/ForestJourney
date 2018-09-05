using mytest.Effects.Custom.FollowPath;
using UnityEngine;

namespace mytest.Effects.Custom.Projectile
{
    /// <summary>
    /// Объект, который двигаеться либо по пути, либо к цели
    /// При попадании создает эффект EffectImpactPrefab и вызывает OnImpact
    /// </summary>
    public class Projectile_Behaviour : FollowPathBehaviour
    {
        [Space(10)]
        [Tooltip("Графика снаряда без учета следов (Для отключения при попадании и плавных следов)")]
        public GameObject ProjectileGraphics;
        public Effect_Base EffectImpactPrefab;

        private bool m_Launched = false;
        private Vector3 m_TargetPos;

        private const float m_SQR_DIST_TO_IMPACT = 0.1f;

        public void Launch(Vector3 targetPos, bool curvedPath)
        {
            if (curvedPath)
                MoveAlongPath();
            else
            {
                //Если было запущено ожидание выключения объекта - остановить
                if (m_DeactivationDelayCoroutine != null)
                {
                    StopCoroutine(m_DeactivationDelayCoroutine);
                    m_DeactivationDelayCoroutine = null;
                }

                //Включить снаряд и графику если они были выключены
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                if (ProjectileGraphics != null && !ProjectileGraphics.activeSelf)
                    ProjectileGraphics.SetActive(true);

                //Если уже определена начальная позиция
                if (m_InitPosition.sqrMagnitude > 0)
                    transform.position = m_InitPosition;

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
        }

        protected override void DeactivateGraphicsOnImpact()
        {
            if (DeactivateOnArrival)
            {
                //Сначала отключаем графику (для плавного исчезания следа, а затем объкет)
                if (ProjectileGraphics != null)
                    ProjectileGraphics.SetActive(false);

                base.DeactivateGraphicsOnImpact();
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
}
