using UnityEngine;

namespace mytest.Effects.Custom.Projectile
{
    /// <summary>
    /// Запуск снаряда 
    /// </summary>
    public class ProjectileLauncher_Behaviour : MonoBehaviour
    {
        [Tooltip("Ссылка на объект")]
        public Projectile_Behaviour Projectile;
        [Tooltip("Двигаться ли объекту по пути либо прямо к цели")]
        public bool CurvedPath = true;

        private System.Action m_OnImpact;

        void Start()
        {
            Projectile.OnImpact += Impact_Handler;
        }

        public void LaunchProjectile(Vector3 targetPos, System.Action onImpact)
        {
            m_OnImpact = onImpact;
            Projectile.Launch(targetPos, CurvedPath);
        }

        void Impact_Handler()
        {
            if (m_OnImpact != null)
                m_OnImpact();
        }
    }
}
