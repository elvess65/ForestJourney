using mytest.Effects.Custom.RepairObject;
using mytest.Interaction;
using UnityEngine;

namespace mytest.ActionTrigger
{
    /// <summary>
    /// Анимация объекта, который может быть уничтожен либо собран (анимация при входе и выходе из зоны)
    /// </summary>
    public class ActionTrigger_AnimateRepairObject : ActionTrigger, iExitableFromInteractionArea
    {
        [Space(10)]
        [Tooltip("Проигрывать ли анимацию только один раз")]
        public bool OneTimePlay = false;
        public RepairObjectBehaviour Behaviour;

        public override void Interact()
        {
            Behaviour.Animate();

            if (OneTimePlay)
                Deactivate();
        }

        public void ExitFromInteractableArea()
        {
            Behaviour.Animate();
        }
    }
}
