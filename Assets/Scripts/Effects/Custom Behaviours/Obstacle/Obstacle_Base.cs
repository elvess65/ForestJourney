using UnityEngine;

/// <summary>
/// Базовый класс для преграды
/// </summary>
public abstract class Obstacle_Base : MonoBehaviour
{
    public abstract void DisableObstacle();

    protected void Disable()
    {
        gameObject.SetActive(false);
    }
}
