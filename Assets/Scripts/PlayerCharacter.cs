using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A player Character.
// Next class up is a playerController, which maps input behaviors.
// It is unaffected by gravity, but only drag forces.
public class PlayerCharacter : MonoBehaviour
{
    public const float FLOATY_FLY_FORCE_PER_SEC = 700;
    public const float FLAP_IMPULSE = 2500;
    public const float DIRECT_INPUT_VELOCITY_PER_SEC = 100;
    enum FlyMode
    {
        FLOATY_FLY,
        DIRECT_INPUT,
        FLAP
    }
    FlyMode flymode;
	// Use this for initialization
	void Start ()
    {
        flymode = FlyMode.FLOATY_FLY;
        switch(flymode)
        {
            case FlyMode.FLOATY_FLY:
                GetComponent<Rigidbody2D>().drag = 5;
                GetComponent<Rigidbody2D>().gravityScale = 0;
                break;
            case FlyMode.DIRECT_INPUT:
                GetComponent<Rigidbody2D>().drag = 0;
                GetComponent<Rigidbody2D>().gravityScale = 0;
                break;
            case FlyMode.FLAP:
                GetComponent<Rigidbody2D>().drag = 1;
                GetComponent<Rigidbody2D>().gravityScale = 0.1f;
                break;
        }
	}

    // Bird should fly in some direction.
    public void flyDirection(Vector2 dir, bool input)
    {
        switch (flymode)
        {
            case FlyMode.FLOATY_FLY:
                GetComponent<Rigidbody2D>().AddForce(dir * FLOATY_FLY_FORCE_PER_SEC * Time.deltaTime, ForceMode2D.Impulse);
                break;
            case FlyMode.DIRECT_INPUT:
                GetComponent<Rigidbody2D>().velocity = dir * DIRECT_INPUT_VELOCITY_PER_SEC * Time.deltaTime;
                break;
            case FlyMode.FLAP:
                if (input)
                {
                    GetComponent<Rigidbody2D>().AddForce(dir * FLAP_IMPULSE * Time.deltaTime, ForceMode2D.Impulse);
                }
                break;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
