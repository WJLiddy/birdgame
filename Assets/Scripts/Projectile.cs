using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// later.. make abstract
public class Projectile : MonoBehaviour
{
    bool team;
    Vector2 velocity;
    int damage;

    public enum ProjType
    {
        ARROW,
        AXE,
        BULLET
    }

    public Vector2 getWeaponDim(ProjType p)
    {
        switch (p)
        {
            case ProjType.ARROW: return new Vector2( 5, 5);
            case ProjType.AXE: return new Vector2 ( 15, 7 );
            case ProjType.BULLET: return new Vector2 ( 15, 7 );
        }
        return Vector2.zero;
    }
    
	// Use this for initialization
	public void setProjType(ProjType p, Vector2 start, Vector2 rel_vel, Vector2 firevec, bool friendly, bool alt)
    {

        this.gameObject.layer = friendly ? 10 : 11;

        var rb = GetComponent<Rigidbody2D>();
        this.transform.position = start;
        this.transform.eulerAngles =  new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(firevec.y, firevec.x)); 
       
        float launchscale = 0;
	    switch(p)
        {
            case ProjType.ARROW:
                rb.gravityScale = 0.3f;
                launchscale = 5;
                firevec.y = 0.05f;
                break;
            case ProjType.AXE:
                rb.gravityScale = 1f;
                launchscale = 2;
                rb.AddTorque(1);
                    firevec.y = 0.3f;
                break;
            case ProjType.BULLET:
                rb.gravityScale = 0f;
                launchscale = 7;
                break;
        }
        
        GetComponent<Rigidbody2D>().velocity = rel_vel + (firevec * launchscale);
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("projectile/" + p.ToString());
        var bx = gameObject.AddComponent<BoxCollider2D>();
        bx.size = 0.01f * getWeaponDim(p);


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D cd)
    {
        Destroy(this.gameObject);
    }
}
