using UnityEngine;

/// <summary>
/// Объект светлячка, который может крутиться вокруг объекта и следовать по пути
/// </summary>
public class TutorialPathAssistant : FollowPathDelayedBehaviour 
{
    [Header("Objects")]
    public Transform IdleTarget;
	public GameObject Trail;
    [Header("Moving")]
    public float RotationSpeed;
	[Header("Graphics")]
	public Effect_Base Effect;
    public Effect_Base StartMoveEffect;

    private bool m_IsIdle = false;
    private Vector3 m_CurAnchorOffset;

    protected override void Start()
    {
        Trail.SetActive(false);
        StartMoveEffect.Deactivate();

        base.Start();

        //Если есть цель для вращения и объект не двигаеться на старте
        if (!MoveOnStart && IdleTarget != null)
        {
            m_CurAnchorOffset = transform.position - IdleTarget.position;
            m_IsIdle = true;
        }
    }

    public override void MoveAlongPath()
    {
        //Перестать вращаться вокруг цели
        m_IsIdle = false;
        //Включить след
        Trail.SetActive(true);
        //Задать начальную позицию пути как текущее положение объекта
        m_RandomPathGenerator.ChangeNode(0, transform.position);

        StartMoveEffect.Activate();
        StartMoveEffect.transform.parent = null;

        base.MoveAlongPath();
    }

    protected override void ImpactHandler()
    {
        //Сразу после столкновения отключить эффекты
        Effect.Deactivate();

        //Вызов события и отключение объекта с задержкой
        base.ImpactHandler();
    }

    private void Update()
    {
        if (!MoveOnStart && m_IsIdle && IdleTarget != null)
        {
            Quaternion curAngle = Quaternion.AngleAxis(RotationSpeed, Vector3.up);
            m_CurAnchorOffset = curAngle * m_CurAnchorOffset;

            Vector3 newPos = IdleTarget.position + m_CurAnchorOffset;
            transform.position = Vector3.Slerp(transform.position, newPos, Speed);
        }
    }
}
