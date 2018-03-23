using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_SpawnEnemy : Action_Base {

    public GameObject Enemy;
    public Transform SpawnPosition;

    public override void Action()
    {
        base.Action();

        Enemy.transform.position = SpawnPosition.position;
        Enemy.GetComponent<EnemyController>().Init();
    }
}
