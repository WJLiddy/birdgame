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
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("GenericEnemy"));
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

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
