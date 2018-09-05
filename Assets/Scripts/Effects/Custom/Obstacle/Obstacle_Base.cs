using UnityEngine;

namespace mytest.Effects.Custom.Obstacle
{
    /// <summary>
    /// Базовый класс для преграды 
    /// Реализации определяют способ уничтожения преграды
    /// </summary>
    public abstract class Obstacle_Base : MonoBehaviour
    {
        public abstract void DisableObstacle();

        protected void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
