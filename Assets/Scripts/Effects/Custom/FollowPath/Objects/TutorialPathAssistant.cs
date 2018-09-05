using UnityEngine;

namespace mytest.Effects.Custom.FollowPath
{
    /// <summary>
    /// Объект светлячка, который может крутиться вокруг объекта и следовать по пути
    /// </summary>
    public class TutorialPathAssistant : FollowPathBehaviour
    {
        [Header("Objects")]
        [Tooltip("Объект, вокруг которого будет вращаться объект пока его не активировали")]
        public Transform IdleTarget;
        public GameObject Trail;
        [Header("Moving")]
        public float RotationSpeed;
        [Header("Graphics")]
        [Tooltip("Эффект объекта (отключаеться при окончании движения)")]
        public Effect_Base Effect;
        [Tooltip("Эффект начала движения")]
        public Effect_Base StartMoveEffect;

        private bool m_IsIdle = false;
        private Vector3 m_CurAnchorOffset;

        protected override void Start()
        {
            Trail.SetActive(false);
            StartMoveEffect.Deactivate();

            base.Start();

            //Если есть цель для вращения и объект не двигаеться на старте
            if (!MoveOnStart && IdleTarget != null)
            {
                m_CurAnchorOffset = transform.position - IdleTarget.position;
                m_IsIdle = true;
            }
        }

        public override void MoveAlongPath()
        {
            //Перестать вращаться вокруг цели
            m_IsIdle = false;
            //Включить след
            EnableEffects(true);
            //Задать начальную позицию пути как текущее положение объекта
            PathMoveController.ChangeNode(0, transform.position);

            //Эффект начала движения
            StartMoveEffect.Activate();
            StartMoveEffect.transform.parent = null;

            base.MoveAlongPath();
        }

        public override void EnableEffects(bool state)
        {
            Trail.SetActive(state);
        }


        protected override void ImpactHandler()
        {
            //Вызов события и отключение объекта с задержкой
            base.ImpactHandler();

            //После столкновения отключить эффекты (Не отключаеться в случае повторного использования)
            if (DeactivateOnArrival)
                Effect.Deactivate();
        }

        private void Update()
        {
            if (!MoveOnStart && m_IsIdle && IdleTarget != null)
            {
                Quaternion curAngle = Quaternion.AngleAxis(RotationSpeed, Vector3.up);
                m_CurAnchorOffset = curAngle * m_CurAnchorOffset;

                Vector3 newPos = IdleTarget.position + m_CurAnchorOffset;
                transform.position = Vector3.Slerp(transform.position, newPos, Speed);
            }
        }
    }
}
