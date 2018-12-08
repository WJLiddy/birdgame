using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMgr : MonoBehaviour
{
    public AudioSource lead;
    public AudioSource bass;
    public GameObject intro;
    public GameObject title;

    public List<GameObject> cubes = new List<GameObject>();

    public enum State
    {
        INTRO,
        TITLE,
        CHARSELECT,
        GAMEPLAY
    }

    State currentState = State.INTRO;

    void Start()
    {
        transition(State.CHARSELECT);
    }

    public void playerSetup()
    {
        int pCount = JoyconManager.Instance.j.Count;
        if (pCount >= 1)
        {
            GameObject cube = Instantiate(Resources.Load<GameObject>("cubicle"));
            GameObject player = Instantiate(Resources.Load<GameObject>("player"));
            cube.transform.localPosition = new Vector2(-2, 0);
            player.transform.localPosition = new Vector2(-1, 1);
            player.GetComponent<PlayerController>().controllerID = 0;
        }
    }
    public void transition(State s)
    {
        currentState = s;
        switch (s)
        {
            case State.TITLE:
                intro.SetActive(false);
                title.SetActive(true);
                break;
            case State.INTRO:
                intro.SetActive(true);
                lead.clip = Resources.Load<AudioClip>("music/birdicarus lead");
                bass.clip = Resources.Load<AudioClip>("music/birdicarus bass");
                lead.Play();
                bass.Play();
                break;
            case State.CHARSELECT:
                lead.volume = 0f;
                title.SetActive(false);
                playerSetup();
                break;
            case State.GAMEPLAY:
                break;
        }
    }

    void Update()
    {
        if (!lead.isPlaying)
        {
            lead.time = 7.034f;
            bass.time = 7.034f;
            lead.Play();
            bass.Play();
        }

        if(currentState == State.CHARSELECT && Common.allPlayersReady())
        {
            transition(State.GAMEPLAY);
        }

    }
}
