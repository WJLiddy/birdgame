using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSeq : MonoBehaviour
{
    string intro = "Sample Text Games Company Incorporated";
    float maxTime = 5;
    float deltaTime = 0f;
    public UnityEngine.UI.Text t;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime < maxTime)
        {
            t.text = intro.Substring(0, 1 + (int)(intro.Length * (deltaTime / maxTime)));
        } else
        {
            if(deltaTime > 7)
            {
                Common.transition(GameStateMgr.State.TITLE);
            }
        }
	}
}
