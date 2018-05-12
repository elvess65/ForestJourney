/// <summary>
/// Отключение эффектов
/// </summary>
public class TriggerEvent_DeactivateEffects : TriggerAction_Event
{
    public Effect_Base[] Effects;

    public override void StartEvent()
    {
        for (int i = 0; i < Effects.Length; i++)
            Effects[i].Deactivate();
    }
}
