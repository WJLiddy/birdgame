using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Common
{

    // Add acessor later.
    public static List<PlayerCharacter> pcs = new List<PlayerCharacter>();

    public static List<PlayerCharacter> getPCs(bool alive_only = true)
    {
        List<PlayerCharacter> pc = new List<PlayerCharacter>();

        foreach(var e in pcs)
        {
            if(alive_only && !e.gameObject.active)
            {
                continue;
            }
            pc.Add(e.GetComponent<PlayerCharacter>());
        }
        return pc;
    }

    public static void reviveAll(Vector2 start)
    {
        foreach(var pc in getPCs(false))
        {
            if(!pc.gameObject.active)
            {
                pc.gameObject.SetActive(true);
                pc.transform.position = start;
                pc.swapWeapon(new Crossbow());
            }
        }
    }

    public static PlayerCharacter getClosestPC(Vector2 start)
    {
        if (getPCs().Count == 0)
            return null;
        return getPCs().Aggregate((minItem, nextItem) =>
        Vector2.Distance(minItem.transform.position, start) <
        Vector2.Distance(nextItem.transform.position, start) ? minItem : nextItem);
    }

    public static List<UsableItem> getUsables()
    {
        List<UsableItem> pc = new List<UsableItem>();

        foreach (var e in GameObject.FindGameObjectsWithTag("Useable"))
        {
            pc.Add(e.GetComponent<UsableItem>());
        }
        return pc;
    }

    public static void transition(GameStateMgr.State s)
    {
        GameObject.Find("GameManager").GetComponent<GameStateMgr>().transition(s);
    }

    public static bool allPlayersReady()
    {
        return !getPCs().Exists(delegate (PlayerCharacter pc) { return !pc.isReady; });
    }
}
