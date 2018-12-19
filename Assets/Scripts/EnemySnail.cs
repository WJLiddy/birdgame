using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnail : Enemy
{
    bool runDir;
    float runSpeed = 0.1f;
    public override bool killable()
    {
        return false;
    }

    public override string spriteName()
    {
        return "snail";
    }

    public override GameObject create()
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/GenericEnemy"));
        go.transform.SetParent(go.transform);
        GameObject parent = GameObject.Find("enemyParent");

        go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("enemy/" + spriteName());
        go.AddComponent<EnemySnail>();

        go.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        return go;
    }

    public override void setUp()
    {

    }

    public override void doAI()
    {
        PlayerCharacter closest = Common.getClosestPC(transform.position);
        if (closest == null)
        {
            return;
        }
        // From mouse code.
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(runDir ? -runSpeed : runSpeed, GetComponent<Rigidbody2D>().velocity.y);

            var rb = GetComponent<Rigidbody2D>();

            RaycastHit2D hitWall = Physics2D.Raycast(new Vector2(rb.transform.position.x + ((runDir) ? -0.15f : 0.15f), rb.transform.position.y), Vector2.down * 0.01f, 0.01f);

            RaycastHit2D hitGround = Physics2D.Raycast(new Vector2(rb.transform.position.x + ((runDir) ? -0.15f : 0.15f), rb.transform.position.y + -0.2f), Vector2.down * 0.01f, 0.01f);
            // Fail if: No ground in front of us. Turn around.

            if (hitGround.rigidbody == null || hitWall.rigidbody != null)
            {
                runDir = !runDir;
                this.gameObject.GetComponent<SpriteRenderer>().flipX = runDir;
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

}

