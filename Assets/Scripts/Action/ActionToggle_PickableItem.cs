using mytest.Effects.Custom;
using mytest.Interaction;
using UnityEngine;

namespace mytest.ActionTrigger
{
    public class ActionToggle_PickableItem : ActionToggle_Base
    {
        [Space(10)]
        public ScriptEffect_CurveScale InteractScale;
        public ScriptEffect_PingPongMove TapMove;
        public Collider TapCollider;

        private bool m_Interacted = false;

        public override void ExitFromInteractableArea()
        {
            base.ExitFromInteractableArea();

            TapCollider.enabled = false;
        }

        public override void InteractByTap()
        {
            base.InteractByTap();

            TapMove.OnAnimationFinished += Crash;
            TapMove.Play();
            m_Interacted = true;
        }

        public override void Interact()
        {
            base.Interact();

            InteractScale.Play();
            TapCollider.enabled = true;
        }


        protected override void Start()
        {
            base.Start();

            TapCollider.enabled = false;
        }

        protected override void Update()
        {
            if (m_Interacted && Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    iInteractableByTap obj = hit.collider.GetComponentInParent<iInteractableByTap>();
                    if (obj != null)
                    {
                        Crash();
                        Spawn();
                    }
                }
            }

            base.Update();
        }


        void Crash()
        {
            Debug.Log("Crash");
            Destroy(gameObject);
        }

        void Spawn()
        {
            Debug.Log("Spawn");
        }
    }
}
