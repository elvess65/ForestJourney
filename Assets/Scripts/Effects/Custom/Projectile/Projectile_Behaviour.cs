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
        private float m_LaunchTime;
        private Vector3 m_TargetPos;
        private Transform m_Target;

        private const float m_SQR_DIST_TO_IMPACT = 0.1f;

        /// <summary>
        /// Запустить снаряд в точку либо по кривой
        /// </summary>
        /// <param name="targetPos">Точка</param>
        /// <param name="curvedPath">Будет ли снаряд двигаться по кривой или в точку</param>
        public void Launch(Vector3 targetPos, bool curvedPath)
        {
            if (curvedPath)
                MoveAlongPath();
            else
            {
                PrepareToLaunch();

                m_TargetPos = targetPos;
                m_Launched = true;
            }
        }

        /// <summary>
        /// Запустить снаряд следовать за целью
        /// </summary>
        /// <param name="target">Цель за которой снаряд должен следовать</param>
        public void Launch(Transform target)
        {
            PrepareToLaunch();

            m_Target = target;
            m_Launched = true;
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
                if (Time.time - m_LaunchTime >= StartDelay)
                {
                    if (m_Target != null)
                        HandleFollowTarget();
                    else
                        HandleMoveToPosition();
                }
            }
        }


        void PrepareToLaunch()
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

            m_LaunchTime = Time.time;
        }

        void HandleMoveToPosition()
        {
            MoveTo(m_TargetPos);

            if (IsArrived(m_TargetPos))
                ArrivedHandler();
        }

        void HandleFollowTarget()
        {
            if (m_Target == null)
                ArrivedHandler();
            else
            {
                MoveTo(m_Target.transform.position);

                if (IsArrived(m_Target.transform.position))
                    ArrivedHandler();
            }
        }

        void MoveTo(Vector3 targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * Speed);
        }

        bool IsArrived(Vector3 targetPos)
        {
            float sqrDistToTarget = (transform.position - targetPos).sqrMagnitude;
            return sqrDistToTarget <= m_SQR_DIST_TO_IMPACT;
        }

        void ArrivedHandler()
        {
            m_Launched = false;
            ImpactHandler();
        }
    }
}
