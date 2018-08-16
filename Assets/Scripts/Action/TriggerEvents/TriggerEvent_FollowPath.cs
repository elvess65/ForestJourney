/// <summary>
/// Начать следовать по пути
/// </summary>
public class TriggerEvent_FollowPath : TriggerAction_Event 
{
    [UnityEngine.Space(10)]
    public FollowPathDelayedBehaviour TargetObject;

	protected override void CallEvent()
	{
        TargetObject.MoveAlongPath();
	}
}
