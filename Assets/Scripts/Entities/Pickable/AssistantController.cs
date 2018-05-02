using UnityEngine;
using UnityEngine.AI;

public class AssistantController : Item_Base
{
	[Header(" - DERRIVED -")]
    [Header("Settings")]
    public Vector3 AnchorOffset;
    [Header(" - Idle")]
    public float IdleAmplitude = 0.2f;
    public float IdleFrequency = 1;
    [Header(" - Picked")]
    [Header("   -  Fosusing")]
    public float FocusingSpeed = 0.1f;
    [Header("   -  Vertical Wave")]
    public float PickedHeightOffset = 1f;
    public float PickedHeightAmplitude = 2f;
    [Header(" - Assist")]
    public float TrailTime = 10;
    [Header(" - Assisted")]
    public float MaxAssistedWaitTime = 30;
    public float SqrDistToDisable = 10;
    [Header("References")]
    public TrailRenderer Trail;
    public Effect_Particles_StopLoop Effect;

    //Idle
    private bool m_Idle = true;
    //Picked
    private bool m_InitFocus = false;
    private Vector3 m_CurAnchorOffset;
    //Way
    private NavMeshAgent m_Agent;
    //Assisted
    private float m_CurWaitTime = 0;
    private bool m_WaitingForThePlayerToDisapear = false;

    protected override void Start() 
    {
        m_Agent = GetComponent<NavMeshAgent>();
        Trail.enabled = false;
    }

    protected override void Update()
    {
        if (!GameManager.Instance.IsActive)
            return;

        if (m_Idle)
        {
            transform.position = new Vector3(transform.position.x,
                                             transform.position.y + Mathf.Sin(Time.time * IdleFrequency) * IdleAmplitude,
                                             transform.position.z);
        }
        else if (m_Picked)
        {
            if (!m_InitFocus)
            {
                //Move to offset position
                Vector3 destPos = GameManager.Instance.GameState.Player.transform.position + AnchorOffset;
                transform.position = Vector3.Slerp(transform.position, destPos, FocusingSpeed);

                if ((destPos - transform.position).sqrMagnitude <= 0.1f)
                    m_InitFocus = true;
            }
            else
            {
                //Rotation around
                Quaternion curAngle = Quaternion.AngleAxis(2, Vector3.up);
                m_CurAnchorOffset = curAngle * m_CurAnchorOffset;

                //Randomize Y position
                m_CurAnchorOffset.y = Mathf.Sin(Time.time) * PickedHeightAmplitude + PickedHeightOffset;
                //Align speed to sin wave (up by amplitude to prevent <0 speed)
                float speed = Mathf.Sin(Time.time) + 1.1f;

                //Slerp to offset
                Vector3 newPos = GameManager.Instance.GameState.Player.transform.position + m_CurAnchorOffset;
                transform.position = Vector3.Slerp(transform.position, newPos, speed);
            }
        }
        else if (m_Using)
        {
            if (m_Agent.remainingDistance != Mathf.Infinity && m_Agent.remainingDistance <= 1f && m_Agent.remainingDistance > 0)
            {
                Debug.Log("AGENT ARRIVED");
                m_Using = false;
                m_WaitingForThePlayerToDisapear = true;
            }
        }
        else if (m_WaitingForThePlayerToDisapear)
        {
            float sqrDist = (GameManager.Instance.GameState.Player.transform.position - transform.position).sqrMagnitude;

            //If player not arrived wait time
            if (sqrDist > SqrDistToDisable)
            {
                m_CurWaitTime += Time.deltaTime;

                //If wait time elapsed set dist to 0 (player arrived)
                if (m_CurWaitTime >= MaxAssistedWaitTime)
                    sqrDist = 0;
            }

            if (sqrDist <= SqrDistToDisable)
            {
                m_WaitingForThePlayerToDisapear = false;
                Disable();
            }
        }
    }

    public override void Interact()
    {
        if (GameManager.Instance.GameState.Player.TryAddAssistant(this))
        {
            m_CurAnchorOffset = AnchorOffset;
            Trail.enabled = true;
            m_Idle = false;

            base.Interact();
        }
    }

    public override void Use()
    {
        base.Use();

        MoveTo(GameManager.Instance.AssistManager.FindNext());
        m_Using = true;
    }

    void MoveTo(Vector3 pos)
    {
        Trail.time = TrailTime;
        m_Agent.SetDestination(pos);
    }

    void Disable()
    {
        Effect.Deactivate();
       
        Destroy(gameObject, 5);
    }
}
