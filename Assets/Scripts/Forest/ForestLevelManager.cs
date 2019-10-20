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

    // Cutscenes
    public DialogueTrigger startCutscene;
    public DialogueTrigger endCutscene;
    public Animator DialogueAnimator;

    public Animator deathMenuAnim;
    public Text deathScoreText, deathSeedText;

    public Animator lifeAnimation;

    // UI and the UI fields
    public Text scoreText;
	public Text seedCountText;
	public Text informationText;
    public Image heart1;
    public Image heart2;
    public Image heart3;
   
	private float score = 0;
	private float seeds = 0;
	private float modifier = 1.0f;
    private AudioSource musicPlayer;
    private GameObject audioPlayer;


    private void Awake()
	{
        Instance = this;

		informationText.text = "Tap Anywhere To Begin!";
		playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
		cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
		scoreText.text = "Score : " + score.ToString("0");
		seedCountText.text = "Seeds : " + seeds.ToString();

        if(Settings.isMusicOn)
        {
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in audios)
            {
                if (audio.CompareTag("Music"))
                {
                    musicPlayer = audio;
                }
            }

            StartCoroutine(AudioController.FadeOut(musicPlayer, 0.5f));
        }

        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);

        startCutscene.Begin();
	}

	private void Update()
	{
		if (Input.anyKey && !isGameStarted && !DialogueAnimator.GetBool("isOpen"))
		{
			isGameStarted = true;
			playerMotor.StartRunning();
			cameraMotor.StartFollowing();
			informationText.text = "";
            FindObjectOfType<SideObjectSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().isFollowing = true;
            if (Settings.isMusicOn)
            {
                audioPlayer = GameObject.FindGameObjectWithTag("SoundController");
                Music music = audioPlayer.GetComponent<Music>();
                music.changeMusic(SceneManager.GetActiveScene());
            }

        }

		if (isGameStarted)
		{
			score += (Time.deltaTime * modifier);
			scoreText.text = "Score : " + score.ToString("0");

            if (score > 60)
            {
                isGameStarted = false;
                playerMotor.StopRunning();
                cameraMotor.StopFollowing();
                endCutscene.Begin();
                StartCoroutine(AudioController.FadeOut(musicPlayer, 0.5f));
                score = 0;
            }
        }

	}

	public void getSeeds()
	{
		seeds++;
		seedCountText.text = "Seeds : " + seeds.ToString();
	}

	public IEnumerator updateLives(float livesAmount)
	{
        lifeAnimation.SetTrigger("LifeLost");
        switch (livesAmount)
        {
            case 2f:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(false);
                break;
            case 1f:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break;
            case 0f:
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break;
        }

        yield return new WaitForSeconds(1f);
        lifeAnimation.SetTrigger("GoBack");
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
        GameObject.FindGameObjectWithTag("AlivePanel").SetActive(false);
        if (Settings.isMusicOn)
            StartCoroutine(AudioController.FadeOut(musicPlayer, 0.5f));
        if(Settings.isSfxOn)
        {
            Music music = audioPlayer.GetComponent<Music>();
            music.playGameOver();
        }
    }

    public void OnExitButtonPress()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


}
