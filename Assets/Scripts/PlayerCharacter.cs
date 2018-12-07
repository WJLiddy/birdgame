using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A player Character.
// Next class up is a playerController, which maps input behaviors.
// It is unaffected by gravity, but only drag forces.
public class PlayerCharacter : MonoBehaviour
{ 
    public const float FLAP_IMPULSE = 3500;
    public const float FLAP_MAX_SEC = 0.3f;
    public const float RUN_FORCE_SEC = 75;
    public float WALK_FRAME_SEC = 0.4f;
    public float walkAnim;
    public float flapTime = 0;
    public Sprite[] flysprites, groundsprites;
	// Use this for initialization
	void Start ()
    {
        flysprites = Resources.LoadAll<Sprite>("birdflyanim/");
        groundsprites = Resources.LoadAll<Sprite>("birdwalkanim/");
        GetComponent<Rigidbody2D>().drag = 1;
        GetComponent<Rigidbody2D>().gravityScale = 0.1f;

	}

    public bool isGrounded()
    {
        return (Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y - (GetComponent<CapsuleCollider2D>().size.y/2)), -Vector3.up, + 0.01f).collider != null);
    }

    // Bird should fly in some direction.
    public void flyDirection(Vector2 dir)
    {

        if (dir != Vector2.zero && flapTime <= 0 && !isGrounded())
        {
            GetComponent<Rigidbody2D>().AddForce(dir * FLAP_IMPULSE * Time.deltaTime, ForceMode2D.Impulse);
            flapTime = FLAP_MAX_SEC;
            if (dir[0] > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            if (dir[0] < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        } else
        {
            flapTime -= Time.deltaTime;
            flapTime = UnityEngine.Mathf.Max(0, flapTime);
        }

        if(isGrounded())
        {
            GetComponent<Rigidbody2D>().AddForce(dir *  RUN_FORCE_SEC * Time.deltaTime, ForceMode2D.Impulse);
            if (dir[0] > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            if (dir[0] < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        if (isGrounded())
        {
            if(GetComponent<Rigidbody2D>().velocity != Vector2.zero)
            {
                walkAnim += Time.deltaTime;
                if(walkAnim > (2 * WALK_FRAME_SEC))
                {
                    walkAnim = 0;
                }
            }
            setGroundSprite((int)(walkAnim / WALK_FRAME_SEC));
        }
        else
        {
            setFlySprite((int)(flapTime / 0.11f));
            if(flapTime == 0)
            {
                setFlySprite(1);
            }
        }
    }

    void setFlySprite(int idx)
    {
        GetComponent<SpriteRenderer>().sprite = flysprites[idx];
    }

    void setGroundSprite(int idx)
    {
        GetComponent<SpriteRenderer>().sprite = groundsprites[idx];
    }

    // Update is called once per frame
    void Update () {
		
	}
}
