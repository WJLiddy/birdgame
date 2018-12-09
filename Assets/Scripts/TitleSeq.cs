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
        if ((JoyconManager.Instance.j.Count > 0 && JoyconManager.Instance.j[0].GetButtonDown(Joycon.Button.DPAD_UP))
            || Input.GetKey(KeyCode.Return)) 
            Common.transition(GameStateMgr.State.CHARSELECT);
	}
}
