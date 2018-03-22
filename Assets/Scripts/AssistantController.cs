using UnityEngine;
using UnityEngine.AI;

public class AssistantController : MonoBehaviour
{
    public bool Move = false;
    public Transform Target;
    public BoxCollider Collider;

    private NavMeshAgent m_Agent;

    public void Pick()
    {
        Debug.Log("Pick assistant");
        Collider.enabled = false;
    }

    public void MoveTo(Vector3 pos)
    {
        m_Agent.SetDestination(pos);
    }
}
