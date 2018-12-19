using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : Enemy
{
    public float shootCooldown = 0;
    public float shootCooldownMax = 1f;
    public bool runDir;
    public float runSpeed = 0.4f;

    public override bool killable()
    {
        return true;
    }

    public override string spriteName()
    {
        return "snake";
    }

    public override GameObject create()
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/GenericEnemy"));
        go.transform.SetParent(go.transform);
        GameObject parent = GameObject.Find("enemyParent");
        go.AddComponent<EnemySnake>();
        go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("enemy/" + spriteName());
        go.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        return go;
    }

    public override void setUp()
    {
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    public override void doAI()
    {
        shootCooldown -= Time.deltaTime;
        shootCooldown = Mathf.Max(0, shootCooldown);
        // Stolen from snail - need GroundEnemy class.
        PlayerCharacter closest = Common.getClosestPC(transform.position);
        if (closest == null)
        {
            return;
        }

        if(Vector2.Distance(this.transform.position,closest.transform.position) < 3 && shootCooldown == 0)
        {
            shootCooldown = shootCooldownMax;
            var aimvec = closest.transform.position - this.transform.position;

            aimvec = aimvec.normalized;
            makeProjectile(this.transform.position, aimvec);
            var angle = Mathf.Atan2(aimvec.x, aimvec.y);

            makeProjectile(this.transform.position, new Vector2(Mathf.Sin(angle + 0.1f), Mathf.Cos(angle + 0.1f)));
            makeProjectile(this.transform.position, new Vector2(Mathf.Sin(angle - 0.1f), Mathf.Cos(angle - 0.1f)));
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

