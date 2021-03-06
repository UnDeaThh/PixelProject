﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    private bool fix = false;
    [SerializeField] PlayerController2 player;
    [SerializeField] Animator playerAnimator;
    private RuntimeAnimatorController playerAnim;
    [SerializeField] PlayableDirector director;
    [SerializeField] GameObject nachCutSceneGO;
    void Start()
    {
        playerAnim = playerAnimator.runtimeAnimatorController;
        if (!ScenesManager.scenesManager.CutSceneDone)
        {
            playerAnimator.runtimeAnimatorController = null;
            player.isOnKinematic = true;
            director.Play();
        }
        else
        {
            Destroy(nachCutSceneGO);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ScenesManager.scenesManager.CutSceneDone)
        {
            if(director.state != PlayState.Playing && !fix)
            {
                fix = true;
                playerAnimator.runtimeAnimatorController = playerAnim;
                player.isOnKinematic = false;
                ScenesManager.scenesManager.CutSceneDone = true;
                nachCutSceneGO.SetActive(false);
                SaveSystem.SaveSceneData(ScenesManager.scenesManager);
                player.facingDir = 1;
            }   
        }
        
    }
}
