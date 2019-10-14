﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ForestLevelManager : MonoBehaviour
{
	public static ForestLevelManager Instance { set; get; }

	private bool isGameStarted = false;
	private PlayerMotor playerMotor;
	private CameraMotor cameraMotor;

	public Animator deathMenuAnim;
    public Text deathScoreText, deathSeedText;

	// UI and the UI fields
	public Text scoreText;
	public Text seedCountText;
	public Text informationText;
	public Text livesText;
	private float score = 0;
	private float seeds = 0;
	private float modifier = 1.0f;

	private void Awake()
	{
        Instance = this;

		informationText.text = "Tap Anywhere To Begin!";
		playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
		cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
		scoreText.text = "Score : " + score.ToString("0");
		seedCountText.text = "Seeds : " + seeds.ToString();
		livesText.text = "Lives Remaining : 3";

	}

	private void Update()
	{
		if (Input.anyKey && !isGameStarted)
		{
			isGameStarted = true;
			playerMotor.StartRunning();
			cameraMotor.StartFollowing();
			informationText.text = "";
            FindObjectOfType<SideObjectSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().isFollowing = true;
		}

		if (isGameStarted)
		{
			score += (Time.deltaTime * modifier);
			scoreText.text = "Score : " + score.ToString("0");
		}

	}

	public void getSeeds()
	{
		seeds++;
		seedCountText.text = "Seeds : " + seeds.ToString();
	}

	public void updateLives(float livesAmount)
	{
		livesText.text = "Lives Remaining : " + livesAmount.ToString("0");
	}

    public void OnRetryButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Forest");
	}

    public void OnDeath()
	{
        isGameStarted = false;   
        deathScoreText.text = "Score: " + score.ToString("0");
        deathSeedText.text = "Seeds Collected: " + seeds.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        SideObjectSpawner.Instance.IsScrolling = false;
    }

    public void OnExitButtonPress()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("Forest_EndingCutscene");
    }
}
