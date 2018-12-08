using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common
{
    public static List<PlayerCharacter> getPCs()
    {
        List<PlayerCharacter> pc = new List<PlayerCharacter>();

        foreach(var e in GameObject.FindGameObjectsWithTag("Player"))
        {
            pc.Add(e.GetComponent<PlayerCharacter>());
        }
        return pc;
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
}
