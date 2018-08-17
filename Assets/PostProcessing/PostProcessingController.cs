using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using Wilberforce.FinalVignette;

public class PostProcessingController : MonoBehaviour
{
    public PostProcessingBehaviour Behaviour;
    public FinalVignetteCommandBuffer Vignette;

    private enum PostEffectsTypes
    {
        Saturation,
        PostExposure
    }

    private Dictionary<PostEffectsTypes, AnimationBehaviour> m_AnimationBehaviours;
    private List<AnimationBehaviour> m_ActiveAnimationBehaviours;
    private PostProcessingProfile m_Profile;
    private Utils.InterpolationData<float> m_SaturationLerpData;

    //Saturation
	private const float m_SATURATION_DECREASED = 0f;
    private const float m_SATURATION_NORMALIZED = 1f;
	private const float m_SATURATION_FADE_TIME = 1f;
	//PostExposure
	private const float m_POSTEXPOSURE_DECREASED = -5f;
    private const float m_POSTEXPOSURE_NORMALIZED = 0f;
	private const float m_POSTEXPOSURE_FADE_TIME = 1f;
    //Vignette 
    private float m_VignetteInnerValue;
    private float m_VignetteOuterValue;
    private const float m_VIGNETTE_DECREASED = 1;

    void Start()
    {
        m_AnimationBehaviours = new Dictionary<PostEffectsTypes, AnimationBehaviour>();
        m_ActiveAnimationBehaviours = new List<AnimationBehaviour>();

        //Создать новый профиль и начать его использовать
        m_Profile = ScriptableObject.CreateInstance<PostProcessingProfile>();
        Behaviour.profile = m_Profile;

        ///Color grading
        m_Profile.colorGrading = new ColorGradingModel();
        m_AnimationBehaviours.Add(PostEffectsTypes.Saturation, new SaturationAnimationBehaviour(m_Profile, m_SATURATION_FADE_TIME));
        m_AnimationBehaviours.Add(PostEffectsTypes.PostExposure, new ExposeAnimationBehaviour(m_Profile, m_POSTEXPOSURE_FADE_TIME));

        ///Vignette
        m_VignetteInnerValue = Vignette.VignetteInnerValue;
        m_VignetteOuterValue = Vignette.VignetteOuterValue;

		/*m_Profile.vignette = new VignetteModel();
        m_Profile.vignette.enabled = true;
        VignetteModel.Settings vignetteSettings = m_Profile.vignette.settings;
        vignetteSettings.intensity = 0.4f;
        m_Profile.vignette.settings = vignetteSettings;*/

		/*VignetteModel.Settings vignetteSettings = m_Profile.vignette.settings;
       vignetteSettings.intensity = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
       m_Profile.vignette.settings = vignetteSettings;*/
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			DecreasePostExposure();
			//DecreaseSaturation();
		}

		if (Input.GetKeyDown(KeyCode.N))
		{
			NormalizePostExposure();
			//NormalizeSaturation();
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
            Vignette.VignetteInnerValue = m_VIGNETTE_DECREASED;
            Vignette.VignetteOuterValue = m_VIGNETTE_DECREASED;
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			Vignette.VignetteInnerValue = m_VignetteInnerValue;
			Vignette.VignetteOuterValue = m_VignetteOuterValue;
		}

		if (m_ActiveAnimationBehaviours.Count > 0)
		{
			for (int i = 0; i < m_ActiveAnimationBehaviours.Count; i++)
				m_ActiveAnimationBehaviours[i].Update();
		}

		Debug.Log(Vignette.VignetteInnerValue + " " + Vignette.VignetteOuterValue);
	}


    public void DecreaseSaturation()
    {
        //Включить эффекты
        EnablePostEffects();

        //Включить Color grading
        if (!m_Profile.colorGrading.enabled)
            m_Profile.colorGrading.enabled = true;

        AnimationBehaviour behaviour = m_AnimationBehaviours[PostEffectsTypes.Saturation];

        //Событие окончания анимации
        behaviour.OnAnimationFinished += () =>
        {
            Debug.Log("Decreasing finished");

			//Удалить эффект из списка активных
			m_ActiveAnimationBehaviours.Remove(behaviour);
        };

        //Добавить эффект в список активных
        m_ActiveAnimationBehaviours.Add(behaviour);

		//Начать анимацию
		behaviour.StartAnimation(m_SATURATION_DECREASED);
    }

    public void NormalizeSaturation()
    {
        AnimationBehaviour behaviour = m_AnimationBehaviours[PostEffectsTypes.Saturation];

        //Событие окончания анимации
        behaviour.OnAnimationFinished += () =>
        {
            Debug.Log("Finished");

			//Выключить Color grading
			m_Profile.colorGrading.enabled = false;

            //Удалить эффект из списка активных
            m_ActiveAnimationBehaviours.Remove(behaviour);

			//Выключить эффекты
			DisablePostEffects();
        };

		//Добавить эффект в список активных
		m_ActiveAnimationBehaviours.Add(behaviour);

		//Начать анимацию
		behaviour.StartAnimation(m_SATURATION_NORMALIZED);
    }


    public void DecreasePostExposure()
    {
		//Включить эффекты
		EnablePostEffects();

		//Включить Color grading
		if (!m_Profile.colorGrading.enabled)
			m_Profile.colorGrading.enabled = true;

        AnimationBehaviour behaviour = m_AnimationBehaviours[PostEffectsTypes.PostExposure];

		//Событие окончания анимации
		behaviour.OnAnimationFinished += () =>
		{
			Debug.Log("Decreasing finished");

			//Удалить эффект из списка активных
			m_ActiveAnimationBehaviours.Remove(behaviour);
		};

		//Добавить эффект в список активных
		m_ActiveAnimationBehaviours.Add(behaviour);

		//Начать анимацию
		behaviour.StartAnimation(m_POSTEXPOSURE_DECREASED);
    }

    public void NormalizePostExposure()
    {
        AnimationBehaviour behaviour = m_AnimationBehaviours[PostEffectsTypes.PostExposure];

		//Событие окончания анимации
		behaviour.OnAnimationFinished += () =>
		{
			Debug.Log("Finished");

			//Выключить Color grading
			m_Profile.colorGrading.enabled = false;

			//Удалить эффект из списка активных
			m_ActiveAnimationBehaviours.Remove(behaviour);

			//Выключить эффекты
			DisablePostEffects();
		};

		//Добавить эффект в список активных
		m_ActiveAnimationBehaviours.Add(behaviour);

		//Начать анимацию
        behaviour.StartAnimation(m_POSTEXPOSURE_NORMALIZED);
    }


    void EnablePostEffects()
    {
		//Включить обработку эффектов
		if (!Behaviour.enabled)
			Behaviour.enabled = true;

        //TODO: 
        //if (!enabled)
        //    enabled = true;
    }

    void DisablePostEffects()
    {
        //TODO:
        //Проверить наличие эффектов в стеке обработки

        //Выключить обработку эффектов
        if (Behaviour.enabled)
            Behaviour.enabled = false;

		//TODO: 
		//if (enabled)
        //    enabled = false;
    }


    abstract class AnimationBehaviour
    {
        public Action OnAnimationFinished;

        protected PostProcessingProfile m_Profile;
        protected Utils.InterpolationData<float> m_LerpData;

        public AnimationBehaviour(PostProcessingProfile profile, float totalTime)
        {
            //Профиль
            m_Profile = profile;

            //Базовые данные об анимации
            m_LerpData = new Utils.InterpolationData<float>();
            m_LerpData.TotalTime = totalTime;
        }

        public virtual void StartAnimation(float targetValue)
        {
			m_LerpData.Start();
		}

        public void Update()
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                Animate();

                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();

                    if (OnAnimationFinished != null)
                    {
                        OnAnimationFinished();
                        OnAnimationFinished = null;
                    }
                }
            }
        }


        protected float LerpResult()
        {
            return Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
        }

        protected abstract void Animate();
    }

    class SaturationAnimationBehaviour : AnimationBehaviour
    {
        public SaturationAnimationBehaviour(PostProcessingProfile profile, float totalTime) : base(profile, totalTime)
        {
        }

		public override void StartAnimation(float targetValue)
		{
			m_LerpData.From = m_Profile.colorGrading.settings.basic.saturation;
			m_LerpData.To = targetValue;

			base.StartAnimation(targetValue);
		}

        protected override void Animate()
        {
			ColorGradingModel.Settings colorGradingModelSettings = m_Profile.colorGrading.settings;
            colorGradingModelSettings.basic.saturation = LerpResult();
			m_Profile.colorGrading.settings = colorGradingModelSettings;
        }
    }

    class ExposeAnimationBehaviour : AnimationBehaviour
    {
        public ExposeAnimationBehaviour(PostProcessingProfile profile, float totalTime) : base(profile, totalTime)
        {
        }

		public override void StartAnimation(float targetValue)
		{
            m_LerpData.From = m_Profile.colorGrading.settings.basic.postExposure;
			m_LerpData.To = targetValue;

			base.StartAnimation(targetValue);
		}

        protected override void Animate()
        {
			ColorGradingModel.Settings colorGradingModelSettings = m_Profile.colorGrading.settings;
            colorGradingModelSettings.basic.postExposure = LerpResult();
			m_Profile.colorGrading.settings = colorGradingModelSettings;
        }
    }

    class VignetteAnimationBehaviour : AnimationBehaviour
    {


        public VignetteAnimationBehaviour(PostProcessingProfile profile, float totalTime, 
                                          FinalVignetteCommandBuffer vignette, float innerValue, float outerValue) : base(profile, totalTime)
        {
        }

		public override void StartAnimation(float targetValue)
		{
			m_LerpData.From = m_Profile.colorGrading.settings.basic.postExposure;
			m_LerpData.To = targetValue;

			base.StartAnimation(targetValue);
		}

        protected override void Animate()
        {
            throw new NotImplementedException();
        }
    }
}
