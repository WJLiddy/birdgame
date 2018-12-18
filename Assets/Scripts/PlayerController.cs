using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Assumes joycon for now...
    public int controllerID = 0;
    public bool useKeys = false;

    Joycon j;
    List<Joycon> joycons;
    public PlayerCharacter pc;
	// Use this for initialization
	void Start ()
    {
        if (!useKeys)
        {
            joycons = JoyconManager.Instance.j;
            j = joycons[controllerID];
        }
    }
	
    Vector2 getKeyboardMoveVec()
    {
        float x = 0;
        float y = 0;
        if(Input.GetKey(KeyCode.D))
        {
            x = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            x = -1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            y = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            y = -1;
        }
        return new Vector2(x, y);
    }
	// Update is called once per frame
	void Update ()
    {
        
        // later: prioritise based on input.
        if ((useKeys && Input.GetKey(KeyCode.L)) || (!useKeys && j.GetButton(j.isLeft ? Joycon.Button.DPAD_DOWN : Joycon.Button.DPAD_UP)))
        {
            pc.fireProjectile();
        }

        if ((useKeys && Input.GetKey(KeyCode.I)) || (!useKeys && j.GetButton(j.isLeft ? Joycon.Button.DPAD_RIGHT : Joycon.Button.DPAD_LEFT)))
        {
            pc.altProjectile();
        }

        if ((!useKeys && j.GetButtonDown(j.isLeft ? Joycon.Button.DPAD_LEFT : Joycon.Button.DPAD_RIGHT)) || (useKeys && Input.GetKeyDown(KeyCode.K)))
        {
            pc.attemptUse();
        }

        pc.flyDirection(useKeys ? getKeyboardMoveVec() : new Vector2((j.isLeft ? -1 : 1) * j.GetStick()[1], (j.isLeft ? -1 : 1) * -j.GetStick()[0]));
        /**
            // GetButtonDown checks if a button has been pressed (not held)
            if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 pressed");
                // GetStick returns a 2-element vector with x/y joystick components
                Debug.Log(string.Format("Stick x: {0:N} Stick y: {1:N}", j.GetStick()[0], j.GetStick()[1]));

                // Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
                j.Recenter();
            }
            // GetButtonDown checks if a button has been released
            if (j.GetButtonUp(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 released");
            }
            // GetButtonDown checks if a button is currently down (pressed or held)
            if (j.GetButton(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 held");
            }

            if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                Debug.Log("Rumble");

                // Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:
                // https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

                j.SetRumble(160, 320, 0.6f, 200);

                // The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
                // (Useful for dynamically changing rumble values.)
                // Then call SetRumble(0,0,0) when you want to turn it off.
            }

            stick = j.GetStick();
            

            // Gyro values: x, y, z axis values (in radians per second)
            gyro = j.GetGyro();

            // Accel values:  x, y, z axis values (in Gs)
            accel = j.GetAccel();

            orientation = j.GetVector();
            if (j.GetButton(Joycon.Button.DPAD_UP))
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
            }
            gameObject.transform.rotation = orientation;
    */
    }
}
