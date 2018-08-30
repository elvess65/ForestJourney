using UnityEngine;

/// <summary>
/// Анимация объекта, который может быть уничтожен либо собран
/// </summary>
public class ActionTrigger_AnimateRepairObject : ActionTrigger, iExitableFromInteractionArea
{
    [Space(10)]
    [Tooltip("Проигрывать ли анимацию только один раз")]
    public bool OneTimePlay = false;
    public Object_RepairBehaviour Behaviour;

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
