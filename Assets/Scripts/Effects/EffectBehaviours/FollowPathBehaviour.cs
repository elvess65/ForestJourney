using System;
using UnityEngine;

/// <summary>
/// Объект, который может передвигаться по заданому пути
/// </summary>
[RequireComponent(typeof(iTweenPathMoveController))]
public abstract class FollowPathBehaviour : MonoBehaviour 
{
    public Action OnImpact;
	public float Speed = 10;

    protected iTweenPathMoveController m_RandomPathGenerator;

    protected virtual void Awake()
    {
        m_RandomPathGenerator = GetComponent<iTweenPathMoveController>();
    }

    public virtual void MoveAlongPath()
    {
		if (m_RandomPathGenerator != null)
		{
			m_RandomPathGenerator.OnArrived += ImpactHandler;
			m_RandomPathGenerator.StartMove(Speed);
		}
		else
		{
			Debug.LogError("ERROR: Component RANDOM PATH GENERATOR not found");
		}
    }

    protected virtual void ImpactHandler()
    {
        if (OnImpact != null)
            OnImpact();
    }
}
