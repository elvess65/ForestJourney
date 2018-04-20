using UnityEngine;

/// <summary>
/// Триггер создания врага
/// </summary>
public class Action_SpawnEnemy : Action_Base 
{
    [Header(" - DERRIVED -")]
    [Tooltip("Объект")]
    public EnemyController EnemyPrefab;
    [Tooltip("Точка создания врага")]
    public Transform SpawnPoint;

    public override void Interact()
    {
        base.Interact();

        EnemyController enemy = Instantiate(EnemyPrefab, SpawnPoint.position, Quaternion.identity);
        enemy.Init();
    }
}
