﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMgr : MonoBehaviour
{
    public AudioSource lead;
    public AudioSource bass;
    public GameObject intro;
    public GameObject title;

    public GameObject level;

    public float levelOpenCinematicTimer = 0f;
    public List<GameObject> cubes = new List<GameObject>();

    int lvl = 1;

    float resetTimer = 0;
    float resetMax = 5;

    public static readonly bool ENABLE_CINEMATICS = true;

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
        transition(State.INTRO);
    }

    public void setUpLevel(int difmod)
    {
        if(level != null)
        {
            Destroy(level);
        }
        level = LevelGenerator.levelGenerate(difmod);
        level.transform.localPosition = new Vector2(0, -3);
        float i = 1;
        foreach(PlayerCharacter pc in Common.getPCs())
        {
            pc.transform.localPosition = new Vector2(3f + (0.4f * i), 0f);
            i += 1f;
        }
    }

    public void spawnPlayer(int id, bool useKeys)
    {
        bool y = (id == 0 || id == 1);
        bool x = (id == 0 || id == 2);
        GameObject cube = Instantiate(Resources.Load<GameObject>("prefabs/cubicle"));
        cubes.Add(cube);
        GameObject player = Instantiate(Resources.Load<GameObject>("prefabs/player"));
        cube.transform.localPosition = new Vector2(x ? -2 : 2, y ? 0 : -3);
        player.transform.localPosition = new Vector2(x ? -1 : 1, y ? 1 : -1);
        player.GetComponent<PlayerController>().controllerID = id;
        player.GetComponent<PlayerController>().useKeys = useKeys;
        Common.pcs.Add(player.GetComponent<PlayerCharacter>());
    }

    public void playerSetup()
    {
        int pCount = JoyconManager.Instance.j.Count;
        if (pCount == 0)
        {
            spawnPlayer(0, true);
        }
        //disgust.
        if (pCount >= 1)
        {
            spawnPlayer(0, false);
        }
        if (pCount >= 2)
        {
            spawnPlayer(1, false);
        }
        if (pCount >= 3)
        {
            spawnPlayer(2, false);
        }
        if (pCount >= 4)
        {
            spawnPlayer(3, false);
        }
    }

    public void levelOpenCinematic()
    {
        if(levelOpenCinematicTimer < 16 && ENABLE_CINEMATICS)
        {
            levelOpenCinematicTimer += Time.deltaTime;
            Camera.main.GetComponent<GameCamera>().transform.position = new Vector3((1-((levelOpenCinematicTimer) / 16f)) * 50, 0, -10);
        } else
        {
            Camera.main.GetComponent<GameCamera>().trackingMode = true;
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
                Common.pcs = new List<PlayerCharacter>();
                Camera.main.GetComponent<GameCamera>().trackingMode = false;
                Camera.main.transform.localPosition = new Vector3(0, 0, -10);
                lead.volume = 0f;
                title.SetActive(false);
                playerSetup();
                break;
            case State.GAMEPLAY:
                Common.reviveAll(Vector2.zero);

                Camera.main.GetComponent<GameCamera>().trackingMode = false;
                levelOpenCinematicTimer = 0;
                lead.volume = 1f;
                lead.Stop();
                lead.time = 0f;
                lead.clip = Resources.Load<AudioClip>("music/swingit lead");
                lead.Play();
                bass.Stop();
                cubes.ForEach(
                    delegate (GameObject go)
                    {
                        Destroy(go);
                    });
                cubes.Clear();
                setUpLevel(lvl++);

                    break;
        }
    }

    void Update()
    {
        if (currentState == State.GAMEPLAY)
        {
            levelOpenCinematic();
            if (Common.getPCs().Count == 0)
            {
                lvl = 1;
                
                bass.Stop();
                lead.Stop();
                resetTimer += Time.deltaTime;
                if(resetTimer > resetMax)
                {
                    transition(State.CHARSELECT);
                    resetTimer = 0;
                    Destroy(level);
                }
            }
        }
        if (!lead.isPlaying && (currentState == State.CHARSELECT || currentState == State.TITLE))
        {
            lead.time = 7.034f;
            bass.time = 7.034f;
            lead.Play();
            bass.Play();

        }

        if (!lead.isPlaying && currentState == State.GAMEPLAY && Common.getPCs().Count > 0)
        {
            lead.time = 47.33f;
            lead.Play();

        }

        if (currentState == State.CHARSELECT && Common.allPlayersReady())
        {
            transition(State.GAMEPLAY);
        }

    }
}
