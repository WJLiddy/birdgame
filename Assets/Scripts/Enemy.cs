using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void setUp();
    public abstract void doAI();
    public abstract string spriteName();
    public abstract GameObject create();

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
        if (col.rigidbody.gameObject.layer == 10)
        {
            Destroy(this.gameObject);
        }
    }
}
