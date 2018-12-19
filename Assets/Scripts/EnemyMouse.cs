using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMouse : Enemy
{
    public float jumpCooldown;
    public float jumpCooldownMax = 3f;
    public bool runDir;
    public float runSpeed = 0.5f;
    public override bool killable()
    {
        return true;
    }

    public override string spriteName()
    {
        return "mouse";
    }

    public override GameObject create()
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/GenericEnemy"));
        go.transform.SetParent(go.transform);
        GameObject parent = GameObject.Find("enemyParent");
        go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("enemy/" + spriteName());
        go.AddComponent<EnemyMouse>();
        go.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        return go;
    }

    public override void setUp()
    {

    }

    public override void doAI()
    {
        jumpCooldown -= Time.deltaTime;
        jumpCooldown = Mathf.Max(0, jumpCooldown);
        PlayerCharacter closest = Common.getClosestPC(transform.position);
        if (closest == null)
        {
            return;
        }
        if (Vector2.Distance(closest.transform.position, transform.position) < 2.5 && jumpCooldown == 0)
        {


            jumpCooldown = jumpCooldownMax;

            Vector2 dir = (closest.transform.position - transform.position).normalized;

            this.gameObject.GetComponent<SpriteRenderer>().flipX = dir.x > 0;
            GetComponent<Rigidbody2D>().AddForce(dir * 100, ForceMode2D.Impulse);
        } else if (jumpCooldown == 0)
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
        //GetComponent<Rigidbody2D>().velocity = dir / 4;

    }

    // Use this for initialization
    void Start()
    {

    }

}
