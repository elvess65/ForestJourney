using mytest.Effects;
using UnityEngine;
using UnityEngine.AI;

namespace mytest.Use
{
    public class AssistantController : Item_Base
    {
        [Header(" - DERRIVED -")]
        [Header("Settings")]
        public Vector3 AnchorOffset;
        [Header(" - Interaction")]
        [Header("   - Idle")]
        public Transform IdleTarget;
        public Transform Graphic;
        [Header("   -  Focusing")]
        public float FocusingSpeed = 0.1f;
        [Header(" - Following")]
        public float RotationSpeed = 2;
        public float FollowingSpeed = 0.05f;
        [Header(" - Assist")]
        public float TrailTime = 10;
        [Header(" - Assisted")]
        public float MaxAssistedWaitTime = 30;
        public float SqrDistToDisable = 10;
        [Header("References")]
        public TrailRenderer Trail;
        public Animator AnimationController;
        public Effect_Base Effect;

        private Behaviour m_Behaviour;
        private NavMeshAgent m_Agent;
        private Vector3 m_GraphicInitPos;

        public NavMeshAgent Agent
        {
            get { return m_Agent; }
        }
        public Vector3 GraphicInitPos
        {
            get { return m_GraphicInitPos; }
        }

        protected override void Start()
        {
            Trail.enabled = false;
            m_Agent = GetComponent<NavMeshAgent>();
            m_Agent.enabled = false;
            m_GraphicInitPos = Graphic.transform.localPosition;

            m_Behaviour = new IdleBehaviour(this);
        }

        protected override void Update()
        {
            if (!GameManager.Instance.IsActive)
                return;

            if (m_Behaviour != null)
                m_Behaviour.Update(Time.deltaTime);
        }

        public override void Interact()
        {
            if (GameManager.Instance.GameState.Player.TryAddAssistant(this))
            {
                m_Behaviour = new FollowingBehaviour(this);

                base.Interact();
            }
        }

        public override void Use()
        {
            base.Use();

            Trail.enabled = true;
            m_Agent.enabled = true;

            MoveTo(GameManager.Instance.AssistManager.FindNext());
            m_Behaviour = new UsingBehaviour(this);
            m_Behaviour.OnBehaviourFinished += UsingFinishedHandler;
        }


        void UsingFinishedHandler()
        {
            m_Behaviour = new ArrivedToDestinationBehaviour(this);
            m_Behaviour.OnBehaviourFinished += ArrivedToDestinationFinishedHandler;
        }

        void MoveTo(Vector3 pos)
        {
            Trail.time = TrailTime;
            m_Agent.SetDestination(pos);
        }


        void ArrivedToDestinationFinishedHandler()
        {
            Disable();
        }

        void Disable()
        {
            m_Behaviour = new DisabledBehaviour(this);
            Effect.Deactivate();

            Destroy(gameObject, 10);
        }


        abstract class Behaviour
        {
            public System.Action OnBehaviourFinished;
            protected AssistantController m_Controller;

            public Behaviour(AssistantController controller)
            {
                m_Controller = controller;
            }

            public abstract void Update(float deltaTime);
        }

        class IdleBehaviour : Behaviour
        {
            private Vector3 m_CurAnchorOffset;

            public IdleBehaviour(AssistantController controller) : base(controller)
            {
                m_CurAnchorOffset = controller.Graphic.transform.position - controller.IdleTarget.position;
            }

            public override void Update(float deltaTime)
            {
                if (m_Controller.IdleTarget != null)
                {
                    Quaternion curAngle = Quaternion.AngleAxis(m_Controller.RotationSpeed, Vector3.up);
                    m_CurAnchorOffset = curAngle * m_CurAnchorOffset;

                    Vector3 newPos = m_Controller.IdleTarget.position + m_CurAnchorOffset;
                    m_Controller.Graphic.transform.position = Vector3.Slerp(m_Controller.Graphic.transform.position, newPos, m_Controller.FollowingSpeed);
                }
            }
        }

        class FollowingBehaviour : Behaviour
        {
            private bool m_InitFocus = false;
            private Vector3 m_CurAnchorOffset;

            public FollowingBehaviour(AssistantController controller) : base(controller)
            {
                m_CurAnchorOffset = m_Controller.AnchorOffset;
            }

            public override void Update(float deltaTime)
            {
                if (!m_InitFocus)
                {
                    //Move to offset position
                    Vector3 destPos = GameManager.Instance.GameState.Player.transform.position + m_CurAnchorOffset;
                    m_Controller.transform.position = Vector3.Slerp(m_Controller.transform.position, destPos, m_Controller.FocusingSpeed);
                    m_Controller.Graphic.localPosition = Vector3.Slerp(m_Controller.Graphic.localPosition, m_Controller.GraphicInitPos, m_Controller.FocusingSpeed);

                    if ((destPos - m_Controller.transform.position).sqrMagnitude <= 0.1f)
                    {
                        m_Controller.transform.position = destPos;
                        m_Controller.Graphic.localPosition = m_Controller.GraphicInitPos;
                        m_InitFocus = true;
                    }
                }
                else
                {
                    //Rotation around
                    Quaternion curAngle = Quaternion.AngleAxis(m_Controller.RotationSpeed, Vector3.up);
                    m_CurAnchorOffset = curAngle * m_CurAnchorOffset;

                    //Align speed to sin wave (up by amplitude to prevent <0 speed)
                    //float speed = Mathf.Sin(Time.time) + 1f;

                    //Slerp to offset
                    Vector3 newPos = GameManager.Instance.GameState.Player.transform.position + m_CurAnchorOffset;
                    m_Controller.transform.position = Vector3.Slerp(m_Controller.transform.position, newPos, m_Controller.FollowingSpeed);
                }
            }
        }

        class UsingBehaviour : Behaviour
        {
            public UsingBehaviour(AssistantController controller) : base(controller)
            {
            }

            public override void Update(float deltaTime)
            {
                if (m_Controller.Agent.remainingDistance < Mathf.Infinity && m_Controller.Agent.remainingDistance <= 1f && m_Controller.Agent.remainingDistance > 0)
                {
                    if (OnBehaviourFinished != null)
                        OnBehaviourFinished();
                }
            }
        }

        class ArrivedToDestinationBehaviour : Behaviour
        {
            private float m_CurWaitTime = 0;

            public ArrivedToDestinationBehaviour(AssistantController controller) : base(controller)
            {
            }

            public override void Update(float deltaTime)
            {
                float sqrDist = (GameManager.Instance.GameState.Player.transform.position - m_Controller.transform.position).sqrMagnitude;

                //If player not arrived wait time
                if (sqrDist > m_Controller.SqrDistToDisable)
                {
                    m_CurWaitTime += Time.deltaTime;

                    //If wait time elapsed set dist to 0 (player arrived)
                    if (m_CurWaitTime >= m_Controller.MaxAssistedWaitTime)
                        sqrDist = 0;
                }

                if (sqrDist <= m_Controller.SqrDistToDisable)
                {
                    if (OnBehaviourFinished != null)
                        OnBehaviourFinished();


                }
            }
        }

        class DisabledBehaviour : Behaviour
        {
            public DisabledBehaviour(AssistantController controller) : base(controller)
            {
            }

            public override void Update(float deltaTime)
            {
            }
        }
    }
}
