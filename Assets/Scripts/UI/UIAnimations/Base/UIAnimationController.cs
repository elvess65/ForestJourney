using System.Collections;
using UnityEngine;

namespace mytest.UI.Animations
{
    public abstract class BaseUIAnimationController : MonoBehaviour
    {
        public event System.Action OnShowFinished;
        public event System.Action OnHideFinished;

        public  abstract void PlayAnimation(bool playShowAnimation);

        protected void CallShowFinished()
        {
            if (OnShowFinished != null)
                OnShowFinished();
        }

        protected void CallHideFinished()
        {
            if (OnHideFinished != null)
                OnHideFinished();
        }
    }

    /// <summary>
    /// Базовый класс для контроля над UI анимациями
    /// </summary>
    public abstract class UIAnimationController<T> : BaseUIAnimationController
    {
        [Header("Animation")]
        public float TimeToTargetPosition = 1.0f;
        public T StartPosition;
        public AnimationCurve CurveShow;
        public AnimationCurve CurveHide;

        protected T m_TargetPosition;
        protected float m_TotalDistance = 0;

        protected virtual void Awake()
        {
            SetTargetPosition();

            m_TotalDistance = GetTotalDistance();

            ApplyPosition(StartPosition);
        }

        public override void PlayAnimation(bool playShowAnimation)
        {
            if (playShowAnimation)
                StartCoroutine(Show());
            else
                StartCoroutine(Hide());
        }

        IEnumerator Show()
        {
            ShowStarted();
            float speed = 1.0f / TimeToTargetPosition;

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
            {
                ApplyPosition(GetShowValue(t));
                yield return null;
            }

            ApplyPosition(m_TargetPosition);
            ShowFinished();
        }

        protected virtual void ShowStarted()
        {
        }

        protected virtual void ShowFinished()
        {
            CallShowFinished();
        }


        IEnumerator Hide()
        {
            HideStarted();
            float speed = 1.0f / TimeToTargetPosition;

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
            {
                ApplyPosition(GetHideValue(t));
                yield return null;
            }

            ApplyPosition(StartPosition);
            HideFinished();
        }

        protected virtual void HideStarted()
        {
        }

        protected virtual void HideFinished()
        {
            CallHideFinished();
        }


        [ContextMenu("Show")]
        public void ContextMenuShow()
        {
            StartCoroutine(Show());
        }

        [ContextMenu("Hide")]
        public void ContextMenuHide()
        {
            StartCoroutine(Hide());
        }


        protected abstract float GetTotalDistance();

        protected abstract T GetShowValue(float t);

        protected abstract T GetHideValue(float t);

        protected abstract void SetTargetPosition();

        protected abstract void ApplyPosition(T position);
    }
}
