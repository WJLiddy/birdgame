using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSeq : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (JoyconManager.Instance.j[0].GetButtonDown(Joycon.Button.DPAD_UP))
            Common.transition(GameStateMgr.State.CHARSELECT);
	}
}
