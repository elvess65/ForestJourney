using mytest.Effects.Custom.FollowPath;
using UnityEngine;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Начать следовать по пути. Если не задан PathMoveController путь будет браться из цели. Если задан - из PathMoveController 
    /// </summary>
    public class TriggerEvent_FollowPath : TriggerAction_Event
    {
        [Space(10)]
        [Tooltip("Объект, который будет перемещаться")]
        public FollowPathBehaviour TargetObject;
        [Tooltip("Путь для цели (Если null - будет браться путь по-умолчанию")]
        public iTweenPathMoveController PathMoveController;
        [Tooltip("Деактивировать объект после окончания движения")]
        public bool DeactivateOnArrival = true;
        [Tooltip("Выключить эффекты после запуска")]
        public bool DeactivateEffects = false;

        protected override void CallEvent()
        {
            if (!TargetObject.gameObject.activeSelf)
                TargetObject.gameObject.SetActive(true);

            //Перегрузка деактивации по прибытию
            if (DeactivateOnArrival && !TargetObject.DeactivateOnArrival)
                TargetObject.DeactivateOnArrival = DeactivateOnArrival;

            //Перегрузка пути
            if (PathMoveController != null)
                TargetObject.PathMoveController = PathMoveController;

            //Начать движение по пути
            TargetObject.OnImpact += ImpactHandler;
            TargetObject.MoveAlongPath();

            //Выключить эффекты
            if (DeactivateEffects)
                TargetObject.EnableEffects(false);
        }

        void ImpactHandler()
        {
            TargetObject.OnImpact -= ImpactHandler;

            CallEventFinished();
        }
    }
}
