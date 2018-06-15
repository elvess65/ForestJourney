/// <summary>
/// Интерфейс контроллера эффектов
/// </summary>
public interface iActionTrigger_EffectController
{
    event System.Action OnEffectFinished;

    void Init(System.Action onEffectFinished);

    /// <summary>
    /// Активировать эффект использования
    /// </summary>
    void ActivateEffect_Action();
}