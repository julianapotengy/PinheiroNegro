﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public GameObject[] Enemies;
	private GameObject player;
	private player playerMedo;
	private Image medoimg;
	private GameObject fadein;
	private bool startfadin, goToNegative, canFade;
	public Transform mainCamera;
   
	void Start ()
	{
		startfadin = true;
		fadein = GameObject.Find ("FadeIn");
		fadein.GetComponent<Image> ().color = new Color (0, 0, 0, 1);
		medoimg = GameObject.Find ("Medo").GetComponent<Image> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerMedo = player.GetComponent<player> ();
		mainCamera = Camera.main.transform;
		goToNegative = false;
		canFade = false;

		for (int i = 0; i < 2; i++)
		{
			Instantiate (Enemies [0], new Vector3 (108.1f,-7.7f,0), Quaternion.Euler (0, 0, 0));
			Instantiate (Enemies [1], new Vector3 (110.4f,-2.8f,0), Quaternion.Euler (0, 0, 0));
			Instantiate (Enemies [2], new Vector3 (113.0456f,-6.216908f,0), Quaternion.Euler (0, 0, 0));
		}
	}

	void Update ()
	{	
		if (startfadin)
		{
			AudioSource audioStart = Object.FindObjectOfType <AudioSource>() as AudioSource;
			audioStart.volume += 0.005f;
			fadein.GetComponent<Image> ().color -= new Color(0, 0, 0, 0.5f * Time.deltaTime);
			if (fadein.GetComponent<Image> ().color.a <= 0 && audioStart.volume >= 1)
				startfadin = false;
		}
		medoimg.fillAmount = playerMedo.medo / 100f;

		if (playerMedo.medo >= 50)
		{
			if(!goToNegative)
			{
				mainCamera.Rotate(0, 0, Time.deltaTime * 1.5f);
				if(mainCamera.rotation.z > 0.1f)
					goToNegative = true;
			}
			else if(goToNegative)
			{
				mainCamera.Rotate(0, 0, Time.deltaTime * -1.5f);
				if(mainCamera.rotation.z < -0.1f)
					goToNegative = false;
			}
		}
		else if(playerMedo.medo <= 49)
		{
			if(mainCamera.rotation.z > 0)
				mainCamera.Rotate(0, 0, Time.deltaTime * -1.5f);
			else if(mainCamera.rotation.z < 0)
				mainCamera.Rotate(0, 0, Time.deltaTime * 1.5f);
		}

		if(playerMedo.medo >= 75)
		{
			if(!canFade)
			{
				fadein.GetComponent<Image> ().color += new Color(0, 0, 0, 2 * Time.deltaTime);
				if(fadein.GetComponent<Image>().color.a >= 1)
					canFade = true;
			}
			else if(canFade)
			{
				fadein.GetComponent<Image> ().color -= new Color(0, 0, 0, 0.5f * Time.deltaTime);
				if (fadein.GetComponent<Image> ().color.a <= 0)
					canFade = false;
			}
		}
		else if(playerMedo.medo <= 74)
		{
			if(fadein.GetComponent<Image>().color.a > 0)
				fadein.GetComponent<Image> ().color -= new Color(0, 0, 0, 0.5f * Time.deltaTime);
		}
	}

	public static void Playsound(AudioClip clip)
	{ 
		AudioSource audio = Object.FindObjectOfType <AudioSource>() as AudioSource;
		audio.PlayOneShot (clip);
	}
	public static void ButtonPaperClip()
	{
		AudioClip sound = Resources.Load ("Sounds/ClickButton") as AudioClip;
		Playsound (sound);
	}
	public static void ButtonHighlightedClip()
	{
		AudioClip sound = Resources.Load ("Sounds/ButtonHighlighted") as AudioClip;
		Playsound (sound);
	}
}
