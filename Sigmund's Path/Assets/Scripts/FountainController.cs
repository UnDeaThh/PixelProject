﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainController : MonoBehaviour
{
    private bool playerIn = false;
    [SerializeField] private float timeHealOne;
    private float currentTimeHealOne;

    private int sceneFountain;
    [Header("PARTICLES")]
    [SerializeField] ParticleSystem ps;
    [SerializeField] float enterParticlesSpeed = 1.5f;
    [SerializeField] float simulationSpeed;

    private LevelManager levelManager;
    private PlayerController2 plController2;

    private bool isUnlocked = false;
    [SerializeField] int fountainNumber;

    private PlayerAudio playerAudio;
    [SerializeField] AudioSource source;

    public bool PlayerIn { get => playerIn; set => playerIn = value; }

    private void Start()
    {
        isUnlocked = ScenesManager.scenesManager.FountainUnlocked[fountainNumber];
        plController2 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        sceneFountain = levelManager.levelScene;
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerAudio>();
    }

    private void Update()
    {
        if (playerIn)
        {
            if(currentTimeHealOne > 0)
            {
                currentTimeHealOne -= Time.deltaTime;
            }
            else if(currentTimeHealOne <= 0)
            {
                if(plController2.health < plController2.maxHealth)
                {
                    plController2.health++;
                    playerAudio.HeartObtained();
                    currentTimeHealOne = timeHealOne;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            if (!isUnlocked)
            {
                isUnlocked = true;
                ScenesManager.scenesManager.FountainUnlocked[fountainNumber] = true;
            }
            playerIn = true;
            currentTimeHealOne = timeHealOne;
            ParticleSystem.EmissionModule emidMod = ps.emission;
            emidMod.rateOverTime = enterParticlesSpeed;
            ParticleSystem.MainModule mainModule = ps.main;
            mainModule.simulationSpeed = simulationSpeed;

            ScenesManager.scenesManager.toLoadScene = sceneFountain;

            SaveSystem.SaveSceneData(ScenesManager.scenesManager);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            ParticleSystem.EmissionModule emidMod = ps.emission;
            emidMod.rateOverTime = 1;
            ParticleSystem.MainModule mainModule = ps.main;
            mainModule.simulationSpeed = 1;

            playerIn = false;
        }
    }
}
