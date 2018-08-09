/// <summary>
/// Выключить эффекты
/// </summary>
public class TriggerEvent_DeactivateEffects : TriggerAction_Event
{
    public Effect_Base[] Effects;

    protected override void Event()
    {
		for (int i = 0; i < Effects.Length; i++)
			Effects[i].Deactivate();
    }
}
