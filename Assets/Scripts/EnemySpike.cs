using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpike : Enemy
{
    public override bool killable()
    {
        return false;
    }

    public override string spriteName()
    {
        return "spike";
    }

    public void setOrientation(float angle)
    {
        this.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public override GameObject create()
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/GenericEnemy"));
        go.transform.SetParent(go.transform);
        GameObject parent = GameObject.Find("enemyParent");

        go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("enemy/" + spriteName());
        go.AddComponent<EnemySpike>();
        go.GetComponent<Rigidbody2D>().isKinematic = true;
        return go;
    }

    public override void setUp()
    {

    }

    public override void doAI()
    {
        //lol
    }

    // Use this for initialization
    void Start()
    {

    }

}