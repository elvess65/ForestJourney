/// <summary>
/// Начать следовать по пути
/// </summary>
public class TriggerEvent_FollowPath : TriggerAction_Event 
{
    public FollowPathDelayedBehaviour TargetObject;

	protected override void Event()
	{
        TargetObject.MoveAlongPath();
	}
}
