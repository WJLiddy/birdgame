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
    public float flapTime = 0;
    public Sprite[] sprites;
	// Use this for initialization
	void Start ()
    {
        sprites = Resources.LoadAll<Sprite>("birdanim/");
        GetComponent<Rigidbody2D>().drag = 1;
        GetComponent<Rigidbody2D>().gravityScale = 0.1f;

	}

    // Bird should fly in some direction.
    public void flyDirection(Vector2 dir)
    {

        /**
         * case FlyMode.FLOATY_FLY:
                GetComponent<Rigidbody2D>().AddForce(dir * FLOATY_FLY_FORCE_PER_SEC * Time.deltaTime, ForceMode2D.Impulse);
                break;
            case FlyMode.DIRECT_INPUT:
                GetComponent<Rigidbody2D>().velocity = dir * DIRECT_INPUT_VELOCITY_PER_SEC * Time.deltaTime;
                break;
    */
        if (dir != Vector2.zero && flapTime <= 0)
        {
            GetComponent<Rigidbody2D>().AddForce(dir * FLAP_IMPULSE * Time.deltaTime, ForceMode2D.Impulse);
            flapTime = FLAP_MAX_SEC;
            if (dir[0] > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        } else
        {
            flapTime -= Time.deltaTime;
            flapTime = UnityEngine.Mathf.Max(0, flapTime);
        }
        setSprite((int)(flapTime / 0.11f));
    }

    void setSprite(int idx)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[idx];
    }

	// Update is called once per frame
	void Update () {
		
	}
}
