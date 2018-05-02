using System.Collections.Generic;
using UnityEngine;

public class QueueAssistManager : MonoBehaviour
{
    public List<Transform> AssitantPointQueue;

    public Vector3 FindNext()
    {
        return AssitantPointQueue[0].position;
    }

    public void RemovePoint(Transform point)
    {
        if (AssitantPointQueue.Contains(point))
            AssitantPointQueue.Remove(point);
    }
}
