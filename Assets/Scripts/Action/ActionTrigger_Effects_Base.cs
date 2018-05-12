using UnityEngine;

public class ActionTrigger_Effects_Base : MonoBehaviour
{
    [Tooltip("Эффект активного объекта (Выключаеться при использовании)")]
    public Effect_Base[] Effects_IsActive;
    [Tooltip("Эффект испоьзования")]
    public Effect_Base[] Effects_Action;

    /// <summary>
    /// Активировать эффект активного объекта
    /// </summary>
    public void ActivateEffects_IsActive()
    {
        if (Effects_IsActive != null)
        {
            for (int i = 0; i < Effects_IsActive.Length; i++)
                Effects_IsActive[i].Activate();
        }
    }

    /// <summary>
    /// Дактивировать эффект активного объекта
    /// </summary>
    public void DeactivateEffects_IsActive()
    {
        if (Effects_IsActive != null)
        {
            for (int i = 0; i < Effects_IsActive.Length; i++)
                Effects_IsActive[i].Deactivate();
        }
    }

    /// <summary>
    /// Активировать эффект испоьзования
    /// </summary>
    public void ActivateEffects_Action()
    {
        if (Effects_Action != null)
        {
            for (int i = 0; i < Effects_Action.Length; i++)
                Effects_Action[i].Activate();
        }
    }
}
