using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void setUp();
    public abstract void doAI();
    public abstract string spriteName();
    public abstract GameObject create();
    public abstract bool killable();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        doAI();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.rigidbody.gameObject.layer == 10 && killable())
        {
            Destroy(this.gameObject);
        }
    }

    public void makeProjectile(Vector2 start, Vector2 vel)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("projectile/Projectile"));
        go.GetComponent<Projectile>().setProjType(Projectile.ProjType.BULLET, start, Vector2.zero, vel, false, false);
        go.transform.position = start;
        go.GetComponent<Rigidbody2D>().velocity = vel;
        go.layer = 11;
    }
}
