using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Soul : Action_Base
{
    public ParticleSystem[] Parctiles;
    public GameObject EffectPrefab;

    public override void Action()
    {
        base.Action();

        for (int i = 0; i < Parctiles.Length; i++)
            Parctiles[i].loop = false;
    }

    public void Activate()
    {
        GameObject ob = Instantiate(EffectPrefab);
        ob.transform.position = GameManager.Instance.Player.transform.position + new Vector3(0, 2, 0);

        Destroy(ob, 5);
    }

}
