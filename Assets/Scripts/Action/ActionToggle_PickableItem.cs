using mytest.Effects.Custom;
using mytest.Effects.Custom.Projectile;
using mytest.Interaction;
using UnityEngine;

namespace mytest.ActionTrigger
{
    /// <summary>
    /// Объект, который можно подобрать и получить какой-то бонус
    /// </summary>
    public class ActionToggle_PickableItem : ActionToggle_Base
    {
        [Space(10)]
        public ScriptEffect_CurveScale InteractScale;
        public ScriptEffect_JumpObject InteractJump;
        public Collider InteractCollider;
        [Header("Effects")]
        public Projectile_Behaviour ContentPrefab;
        public GameObject CrashEffectPrefab;

        private bool m_Interacted = false;

        public override void ExitFromInteractableArea()
        {
            base.ExitFromInteractableArea();

            InteractScale.Play();
            InteractCollider.enabled = false;
        }

        public override void InteractByTap()
        {
            base.InteractByTap();

            InteractJump.OnAnimationFinished += Crash;
            InteractJump.Play();
            m_Interacted = true;
        }

        public override void Interact()
        {
            base.Interact();

            InteractScale.Play();
            InteractCollider.enabled = true;
        }


        protected override void Start()
        {
            base.Start();

            InteractCollider.enabled = false;
        }

        protected override void Update()
        {
            if (m_Interacted && Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, m_RAY_DISTANCE, InteractLayerMask))
                {
                    iInteractableByTap obj = hit.collider.GetComponentInParent<iInteractableByTap>();
                    if (obj != null)
                    {
                        Crash();
                        SpawnObject();
                    }
                }
            }

            base.Update();
        }


        //Создать объект, который храниться внутри (бонус)
        void SpawnObject()
        {
            Projectile_Behaviour ob = Instantiate(ContentPrefab, InteractJump.transform.position, Quaternion.identity);
            ob.Launch(GameManager.Instance.GameState.Player.PointBonusCollect);
        }

        //Уничтожить объект
        void Crash()
        {
            Destroy(Instantiate(CrashEffectPrefab, InteractJump.transform.position, Quaternion.identity), 2);
            Destroy(gameObject);
        }
    }
}
