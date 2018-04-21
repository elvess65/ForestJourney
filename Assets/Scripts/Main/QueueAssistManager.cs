using System.Collections.Generic;
using UnityEngine;

public class QueueAssistManager : MonoBehaviour
{
    public List<Transform> TargetQueue;

    public Vector3 FindNext()
    {
        return TargetQueue[0].position;
    }

    public void RemovePoint(Transform point)
    {
        if (TargetQueue.Contains(point))
            TargetQueue.Remove(point);
    }
}
