using mytest.Effects.Custom.Obstacle;
using mytest.Effects.Custom.Projectile;
using UnityEngine;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Выключить преграду после попадания снаряда
    /// </summary>
    public class TriggerEvent_DeactivateObstacleWithProjectile : TriggerAction_Event
    {
        [Space(10)]
        [Tooltip("Объект преграды (реализует тип уничтожения преграды при попадании снаряда)")]
        public Obstacle_Base ObstacleObject;
        [Tooltip("Контроллер запуска снаряда")]
        public ProjectileLauncher_Behaviour ProjectileLauncher;
        [Tooltip("Точка, куда летит снаряд (нужна если снаряд летит не по пути)")]
        public Transform HitPoint;
   
        protected override void CallEvent()
        {
            ProjectileLauncher.LaunchProjectile(HitPoint.position, ProjectileImpact_Handler);
        }

        void ProjectileImpact_Handler()
        {
            ObstacleObject.DisableObstacle();
        }
    }
}
   