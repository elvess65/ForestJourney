using UnityEngine;

namespace mytest.Interaction
{
    /// <summary>
    /// Базовый интерфейс для всех объектов, с которыми можно взаимодейтсовать 
    /// </summary>
    public interface iInteractable
    {
        event System.Action OnInteract;
        event System.Action OnInteractionFinished;

        void Interact();
    }

    /// <summary>
    /// Базовый интерфейс для всех объектов, с которыми можно взаимодейтсовать по тапу
    /// </summary>
    public interface iInteractableByTap
    {
        void InteractByTap();
    }

    /// <summary>
    /// Базовый интерфейс для всех объектов, из области действия которых можно выйти
    /// </summary>
    public interface iExitableFromInteractionArea
    {
        void ExitFromInteractableArea();
    }

    public abstract class ActionToggle_Base : ActionTrigger.ActionTrigger, iInteractableByTap, iExitableFromInteractionArea
    {
        [Space(10)]
        public AnimatedAppearDisappearObject SelectionAnimatedObject;
        public LayerMask InteractLayerMask;

        protected bool m_CanInteract = false;
   
        protected const float m_RAY_DISTANCE = 100;


        public virtual void ExitFromInteractableArea()
        {
            //Снять выделение
            Unselect();
        }

        public virtual void InteractByTap()
        {
            base.Interact();

            //Снять выделение
            Unselect();
        }

        public override void Interact()
        {
            //Разрешить использовать объект
            m_CanInteract = true;

            //Показать выделение
            SelectionAnimatedObject.OnAnimationFinished = null;
            SelectionAnimatedObject.Show();
        }


        protected virtual void Update()
        {
            HandleInput();
        }

        protected virtual void Unselect()
        {
            //Запретить использовать объект
            m_CanInteract = false;

            //Спрятать выделение
            SelectionAnimatedObject.OnAnimationFinished += SelectionAnimationFinishedHandler;
            SelectionAnimatedObject.Hide();
        }


        void HandleInput()
        {
            if (m_CanInteract && Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Debug.Log(InteractLayerMask);
                if (Physics.Raycast(ray, out hit, m_RAY_DISTANCE, InteractLayerMask))
                {
                    iInteractableByTap obj = hit.collider.GetComponentInParent<iInteractableByTap>();
                    if (obj != null)
                        obj.InteractByTap();
                }
            }
        }

        void SelectionAnimationFinishedHandler()
        {
            SelectionAnimatedObject.gameObject.SetActive(false);
        }
    }
}
