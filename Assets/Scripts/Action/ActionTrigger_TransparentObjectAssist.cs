using UnityEngine;
using System.Collections;

/// <summary>
/// Подсветка прозрачным эффектом объекта, на который нужно обратить внимание
/// </summary>
public class ActionTrigger_TransparentObjectAssist : ActionTrigger, iExitableFromInteractionArea
{
    [Space(10)]
    [Tooltip("Задержка перед анимацией во время показа подсказки (Первый раз)")]
    public float DelayBeforeFirstAnimation = 1;
    public float TimeBeforeAssist = 5;                   //Время пребывания игрока в зоне, до появления подсказки
	public MeshRenderer[] Renderers;
    public TriggerAction_Event[] AssistantEvents;
    [Header("Effect settings")]
    public float TargetValue = 0.5f;
    public float TotalTime = 1;

    //Общее
    private System.Action m_OnAnimationFinished;
	private Material m_Material;
	private float m_InitRim;
	private Utils.InterpolationData<float> m_RimLerpData;

    //Первая подсказка
	private bool m_AssistShowed = false;                    //Была ли показана подсказка
	private float m_AssistCurTime = 0;                      //Текущее время пребывания в зоне
	private bool m_Interact = false;                        //Произошло ли первое взаимодейтсиве
	private bool m_CalculateTimeAfterFirstInteract = false; //Просчет времени пребывания игрока в зоне до появления подскзки

	private const string m_RIM = "_XRayRimSize";

	protected override void Start()
    {
        base.Start();

		//Создать новый материал
		m_Material = new Material(Renderers[0].sharedMaterial);
        m_Material.SetFloat(m_RIM, 0);

        //Задать материал и выключить объекты
        for (int i = 0; i < Renderers.Length; i++)
        {
            Renderers[i].sharedMaterial = m_Material;
            Renderers[i].gameObject.SetActive(false);
        }

        //Начальные данные об анмации
        m_RimLerpData = new Utils.InterpolationData<float>();
        m_RimLerpData.TotalTime = TotalTime;

        m_InitRim = 0;
    }

	public override void Interact()
	{
        //Если еще небыло взаимодейтсвия
        if (!m_Interact)
        {
            m_Interact = true;

            //Начать считать время пребывания игрока в зоне
            m_CalculateTimeAfterFirstInteract = true;
            return;
        }

        ShowAnimationOrAssist();
	}

    public void ExitFromInteractableArea()
    {
        //Если при выходе из зоны считалось время пребывания в зоне - прекратить
        if (m_CalculateTimeAfterFirstInteract)
            m_CalculateTimeAfterFirstInteract = false;
        else 
            Hide();
    }

    void ShowAnimationOrAssist()
    {
        //Если подсказка уже показывалась показать анимацию
        if (m_AssistShowed)
            Show();
        else //Если подсказки еще небыло
        {
            m_AssistShowed = true;

            //Вызвать все события
            for (int i = 0; i < AssistantEvents.Length; i++)
                AssistantEvents[i].StartEvent();

            //Показать анимацию с задержкой
            StartCoroutine(WaitDelayBeforeShow());
        }
    }

    IEnumerator WaitDelayBeforeShow()
    {
        yield return new WaitForSeconds(DelayBeforeFirstAnimation);
        Show();
    }

    void Show()
    {
		//Включить объекты
		for (int i = 0; i < Renderers.Length; i++)
			Renderers[i].gameObject.SetActive(true);

        m_OnAnimationFinished = null;
		m_RimLerpData.From = m_Material.GetFloat(m_RIM);
		m_RimLerpData.To = TargetValue;
        m_RimLerpData.Start();
	}

    void Hide()
    {
        m_OnAnimationFinished += HideAnimationFinishedHandler;
		m_RimLerpData.From = m_Material.GetFloat(m_RIM);
		m_RimLerpData.To = m_InitRim;
		m_RimLerpData.Start();
    }

    void HideAnimationFinishedHandler()
    {
        m_OnAnimationFinished = null;
		for (int i = 0; i < Renderers.Length; i++)
			Renderers[i].gameObject.SetActive(false);
    }

	void Update () 
    {
        if (m_CalculateTimeAfterFirstInteract)
        {
            m_AssistCurTime += Time.deltaTime;
            if (m_AssistCurTime >= TimeBeforeAssist)
            {
                ShowAnimationOrAssist();
                m_CalculateTimeAfterFirstInteract = false;
            }
        }

		if (m_RimLerpData.IsStarted)
		{
			m_RimLerpData.Increment();
			float rim = Mathf.Lerp(m_RimLerpData.From, m_RimLerpData.To, m_RimLerpData.Progress);
			m_Material.SetFloat(m_RIM, rim);

			if (m_RimLerpData.Overtime())
			{
				m_RimLerpData.Stop();

                if (m_OnAnimationFinished != null)
                    m_OnAnimationFinished();
			}
		}
	}
}
