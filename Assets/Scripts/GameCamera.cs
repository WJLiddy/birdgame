using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public bool trackingMode = false;

	// Use this for initialization
	void Start () {
		
	}
    
    public float avgPlayerX()
    {
        float sum = 0;
        foreach (var v in Common.getPCs())
        {
            sum += v.transform.position.x;
        }
        return (sum / Common.getPCs().Count);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(trackingMode)
        {
            transform.localPosition = new Vector3(avgPlayerX(), 0, -10);
        }
	}
}
