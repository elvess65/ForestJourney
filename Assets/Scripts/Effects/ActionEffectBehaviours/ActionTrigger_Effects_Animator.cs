using UnityEngine;

public class ActionTrigger_Effects_Animator : MonoBehaviour, iActionTriggerEffect
{
    public Animator AnimatorController;

    public string ActivateEffects_IsActive_Key;
    public string ActivateEffects_Action_Key;

    /// <summary>
    /// Активировать эффект активного объекта
    /// </summary>
    public void ActivateEffects_IsActive()
    {
    }

    /// <summary>
    /// Активировать эффект испоьзования
    /// </summary>
    public virtual void ActivateEffects_Action()
    {
        AnimatorController.SetTrigger(ActivateEffects_Action_Key);
    }
}
