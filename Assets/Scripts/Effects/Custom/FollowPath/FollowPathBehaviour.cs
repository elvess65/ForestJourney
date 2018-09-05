using System;
using System.Collections;
using UnityEngine;

namespace mytest.Effects.Custom.FollowPath
{
    /// <summary>
    /// Объект, который может передвигаться по заданому пути и с задержкой на старте
    /// </summary>
    public abstract class FollowPathBehaviour : MonoBehaviour
    {
        public Action OnImpact;

        [Tooltip("Контроллер пути")]
        public iTweenPathMoveController PathMoveController;
        [Header("Params")]
        [Tooltip("Скорость передвижения")]
        public float Speed = 10;
        [Tooltip("Задрежка перед стартом, если разрешен автостарт")]
        public float StartDelay = 1;
        [Tooltip("Задержка перед уничтожением по достижению цели, если разрешено")]
        public float DestroyDelay = 10;
        [Header("Settings")]
        [Tooltip("Автостарт (ожидание задержки и перемещение по пути")]
        public bool MoveOnStart = true;
        [Tooltip("Выключение объекта с задержкой по достижении конечной точки пути")]
        public bool DeactivateOnArrival = true;

        protected Vector3 m_InitPosition;
        protected Coroutine m_DeactivationDelayCoroutine;

        protected virtual void Start()
        {
            //Если объект должен начинать движение на старте
            if (MoveOnStart)
            {
                //Если есть задержка 
                if (StartDelay > 0)
                    StartCoroutine(WaitStartDelay());
                else //Если задержки нет
                    MoveAlongPath();
            }

            m_InitPosition = transform.position;
        }

        public virtual void MoveAlongPath()
        {
            if (PathMoveController == null)
                PathMoveController = GetComponent<iTweenPathMoveController>();

            if (PathMoveController != null)
            {
                PathMoveController.OnArrived += ImpactHandler;
                PathMoveController.StartMove(Speed, gameObject);
            }
            else
            {
                Debug.LogError("ERROR: Component RANDOM PATH GENERATOR not found");
            }
        }

        public abstract void EnableEffects(bool state);


        protected virtual void ImpactHandler()
        {
            if (PathMoveController != null)
                PathMoveController.OnArrived -= ImpactHandler;

            //Вызов события
            if (OnImpact != null)
                OnImpact();

            DeactivateGraphicsOnImpact();
        }

        IEnumerator WaitStartDelay()
        {
            yield return new WaitForSeconds(StartDelay);

            MoveAlongPath();
        }

        protected IEnumerator WaitDeactivationDelay()
        {
            yield return new WaitForSeconds(DestroyDelay);

            gameObject.SetActive(false);
            m_DeactivationDelayCoroutine = null;
        }

        protected virtual void DeactivateGraphicsOnImpact()
        {
            //Отключение объекта с задержкой
            if (DeactivateOnArrival)
                m_DeactivationDelayCoroutine = StartCoroutine(WaitDeactivationDelay());
        }
    }
}
