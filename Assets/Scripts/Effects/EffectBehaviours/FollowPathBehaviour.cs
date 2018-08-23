using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Объект, который может передвигаться по заданому пути и с задержкой на старте
/// </summary>
public abstract class FollowPathBehaviour : MonoBehaviour 
{
    public Action OnImpact;

    [Tooltip("Контроллер пути")]
    public iTweenPathMoveController PathMoveController;
	[Header("Params")]
    [Tooltip("Скорость передвижения")]
    public float Speed = 10;
    [Tooltip("Задрежка перед стартом, если разрешен автостарт")]
	public float StartDelay = 1;
    [Tooltip("Задержка перед уничтожением по достижению цели, если разрешено")]
	public float DestroyDelay = 10;
    [Header("Settings")]
    [Tooltip("Автостарт (ожидание задержки и перемещение по пути")]
	public bool MoveOnStart = true;
    [Tooltip("Выключение объекта с задержкой по достижении конечной точки пути")]
	public bool DeactivateOnArrival = true;

	protected virtual void Start()
	{
		//Если объект должен начинать движение на старте
		if (MoveOnStart)
		{
            //Если есть задержка 
			if (StartDelay > 0)
				StartCoroutine(WaitDelay());
			else //Если задержки нет
				MoveAlongPath();
		}
	}

    public virtual void MoveAlongPath()
    {
        if (PathMoveController == null)
            PathMoveController = GetComponent<iTweenPathMoveController>();

		if (PathMoveController != null)
		{
			PathMoveController.OnArrived += ImpactHandler;
            PathMoveController.StartMove(Speed, gameObject);
		}
		else
		{
			Debug.LogError("ERROR: Component RANDOM PATH GENERATOR not found");
		}
    }

    public abstract void EnableEffects(bool state);


    protected virtual void ImpactHandler()
    {
        PathMoveController.OnArrived -= ImpactHandler;

        //Вызов события
        if (OnImpact != null)
            OnImpact();

		//Отключение объекта с задержкой
		if (DeactivateOnArrival)
			Destroy(gameObject, DestroyDelay);
    }

	IEnumerator WaitDelay()
	{
		yield return new WaitForSeconds(StartDelay);

		MoveAlongPath();
	}
}
