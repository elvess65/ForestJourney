using mytest.Effects;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Выключить эффекты
    /// </summary>
    public class TriggerEvent_DeactivateEffects : TriggerAction_Event
    {
        [UnityEngine.Space(10)]
        public Effect_Base[] Effects;

        protected override void CallEvent()
        {
            for (int i = 0; i < Effects.Length; i++)
            {
                if (Effects[i] != null)
                    Effects[i].Deactivate();
            }
        }
    }
}
