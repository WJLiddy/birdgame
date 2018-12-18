using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyButterfly : Enemy
{
    public override string spriteName()
    {
        return "bfly";
    }

    public override GameObject create()
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/GenericEnemy"));
        go.transform.SetParent(go.transform);
        GameObject parent = GameObject.Find("enemyParent");

        go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("enemy/" + spriteName());
        return go;
    }

    public override void setUp()
    {

    }

    public override void doAI()
    {
        PlayerCharacter closest = Common.getClosestPC(transform.position);
        if(closest == null)
        {
            return;
        }
        if(Vector2.Distance(closest.transform.position,transform.position) > 2)
        {
            return;
        }
        Vector2 dir = (closest.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = dir / 4;

    }

    // Use this for initialization
    void Start () {
		
	}

}
