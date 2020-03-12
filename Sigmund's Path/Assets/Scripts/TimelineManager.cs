using System.Collections;
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
    void Start()
    {
        playerAnim = playerAnimator.runtimeAnimatorController;
        if (!ScenesManager.scenesManager.cutSceneDone)
        {
            playerAnimator.runtimeAnimatorController = null;
            player.isOnKinematic = true;
            director.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ScenesManager.scenesManager.cutSceneDone)
        {
            if(director.state != PlayState.Playing && !fix)
            {
                fix = true;
                playerAnimator.runtimeAnimatorController = playerAnim;
                player.isOnKinematic = false;
                ScenesManager.scenesManager.cutSceneDone = true;
                SaveSystem.SaveSceneData(ScenesManager.scenesManager);
            }   
        }
        
    }
}
