using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public GameObject ExplisionPrefab;

    private bool m_IsActive = false;
    private NavMeshAgent m_Agent;

	void Start ()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    public void Init()
    {
        StartCoroutine(Delay(1));
    }
	
	IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        Activate();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.IsActive)
        {
            if (m_IsActive)
                m_Agent.destination = GameManager.Instance.Player.transform.position;
        }
    }

    void Activate()
    {
        m_IsActive = true;

    }
    
}
