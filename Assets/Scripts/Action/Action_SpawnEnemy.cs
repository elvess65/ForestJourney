using UnityEngine;

public class Action_SpawnEnemy : Action_Base {

    public EnemyController EnemyPrefab;
    public Transform SpawnPoint;

    public override void Action()
    {
        base.Action();

        EnemyController enemy = Instantiate(EnemyPrefab, SpawnPoint.position, Quaternion.identity);
        enemy.Init();
    }
}
