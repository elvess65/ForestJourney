using UnityEngine;

/// <summary>
/// Вернуть уровень Expose в норму
/// </summary>
public class TriggetEvent_NormalizeExpose : TriggerAction_Event 
{
	protected override void CallEvent()
	{
        PostProcessingController.Instance.NormalizePostExposure();
	}
}
