using System.Collections;
using UnityEngine;

/// <summary>
/// Объект, который может двигаться по пути с задержкой
/// </summary>
public abstract class FollowPathDelayedBehaviour : FollowPathBehaviour 
{
    public bool MoveOnStart = true;
    [Header("Delays")]
    public float StartDelay = 1;
    public float DestroyDelay = 10;

    protected virtual void Start()
    {
        //Если объект должен начинать движение на старте
        if (MoveOnStart)
        {
            if (StartDelay > 0)
                StartCoroutine(WaitDelay());
            else
                MoveAlongPath();
        }
    }

    IEnumerator WaitDelay()
	{
		yield return new WaitForSeconds(StartDelay);

		MoveAlongPath();
	}

    protected override void ImpactHandler()
    {
        //Вызов события
        base.ImpactHandler();

        //Отключение объекта с задержкой
        Destroy(gameObject, DestroyDelay);
    }
}
