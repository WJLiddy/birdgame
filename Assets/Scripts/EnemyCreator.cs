using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator
{
    public enum Type
    {
        SNAKE,
        BUTTERFLY,
        PENG
    }

	// Update is called once per frame
	static GameObject createEnemy(Type t, Vector2 v)
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("t"));
        go.transform.localPosition = v;
        go.GetComponent<Enemy>().Create(t);
        return go;
	}
}
