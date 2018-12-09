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

    public void setUpLevel()
    {
        level = LevelGenerator.levelGenerate();
        level.transform.localPosition = new Vector2(-4, 0);
        float i = 1;
        foreach(PlayerCharacter pc in Common.getPCs())
        {
            pc.transform.localPosition = new Vector2(-3f + (0.2f * i), 79f);
            i += 1f;
        }
    }

    public void playerSetup()
    {
        int pCount = JoyconManager.Instance.j.Count;
        if (pCount == 0)
        {
            GameObject cube = Instantiate(Resources.Load<GameObject>("cubicle"));
            cubes.Add(cube);
            GameObject player = Instantiate(Resources.Load<GameObject>("player"));
            cube.transform.localPosition = new Vector2(-2, 0);
            player.transform.localPosition = new Vector2(-1, 1);
            player.GetComponent<PlayerController>().useKeys = true;
        }
        if (pCount >= 1)
        {
            GameObject cube = Instantiate(Resources.Load<GameObject>("cubicle"));
            cubes.Add(cube);
            GameObject player = Instantiate(Resources.Load<GameObject>("player"));
            cube.transform.localPosition = new Vector2(-2, 0);
            player.transform.localPosition = new Vector2(-1, 1);
            player.GetComponent<PlayerController>().controllerID = 0;
        }
    }

    public void levelOpenCinematic()
    {
        if(levelOpenCinematicTimer < 16)
        {
            levelOpenCinematicTimer += Time.deltaTime;
            Camera.main.GetComponent<GameCamera>().transform.position = new Vector3(0, ((levelOpenCinematicTimer) / 16f) * 80, -10);
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
                lead.volume = 0f;
                title.SetActive(false);
                playerSetup();
                break;
            case State.GAMEPLAY:
                levelOpenCinematicTimer = 0;
                lead.volume = 1f;
                lead.Stop();
                lead.clip = Resources.Load<AudioClip>("music/swingit lead");
                lead.Play();
                bass.Stop();
                cubes.ForEach(
                    delegate (GameObject go)
                    {
                        Destroy(go);
                    });
                cubes.Clear();
                setUpLevel();

                    break;
        }
    }

    void Update()
    {
        if (currentState == State.GAMEPLAY)
        {
            levelOpenCinematic();
        }
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
