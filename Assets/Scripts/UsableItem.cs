using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
    public string itemName;
    float triggerDist = 0;
    Vector3 helmArgCol;

    void GetSprite(string sname)
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("grounditem/" + sname);
        if(sname == "helm")
        {
            helmArgCol = new Vector3(Random.value, Random.value, Random.value);
            GetComponent<SpriteRenderer>().sprite = LookRandomizer.Randomize(GetComponent<SpriteRenderer>().sprite, helmArgCol);
        }
    }

    void Start()
    {
        GetSprite(itemName);
        triggerDist = 0.2f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (var c in Common.getPCs())
        {
            if(Vector2.Distance(this.transform.position,c.transform.position) < triggerDist)
            {
                // highlight this
                GetComponent<SpriteOutline>().enabled = true;
                return;
            }
        }

        GetComponent<SpriteOutline>().enabled = false;
    }

    public void tryUse(PlayerCharacter caller)
    {
        if (Vector2.Distance(this.transform.position, caller.transform.position) < triggerDist)
        {
            switch(itemName)
            {
                case "cabinet": caller.reColor(); break;
                case "gun": caller.swapWeapon(new Pistol()); break;
                case "crossbow": caller.swapWeapon(new Crossbow()); break;
                case "axe": caller.swapWeapon(new Axe()); break;
                case "notrd":
                    caller.isReady = true;
                    GetSprite("rd");
                    break;
                case "helm": caller.swapHelm(helmArgCol); Destroy(this.gameObject); break;
                case "flag": Common.transition(GameStateMgr.State.GAMEPLAY); break;
            }
        }
    }
}