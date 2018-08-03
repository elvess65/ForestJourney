using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(iTweenPathMoveController))]
public class FollowPathBehaviour : MonoBehaviour 
{
    public Action OnImpact;
	public float Speed = 10;

    private iTweenPathMoveController m_RandomPathGenerator;

    public void MoveAlongPath()
    {
        m_RandomPathGenerator = GetComponent<iTweenPathMoveController>();

		if (m_RandomPathGenerator != null)
		{
			m_RandomPathGenerator.OnArrived += Impact;
			m_RandomPathGenerator.StartMove(Speed);
		}
		else
		{
			Debug.LogError("ERROR: Component RANDOM PATH GENERATOR not found");
		}
    }

    protected virtual void Impact()
    {
        if (OnImpact != null)
            OnImpact();
    }
}
