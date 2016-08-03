﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
	public GameObject[] pauseObjects;

	public Text pressPToPause;
	private float timer;
	
	void Start ()
	{
		Time.timeScale = 1;
		pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
		hidePaused();

		timer = 4;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(Time.timeScale == 1)
			{
				Time.timeScale = 0;
				showPaused();
			}
			else if (Time.timeScale == 0)
			{
				Time.timeScale = 1;
				hidePaused();
			}
		}

		timer -= Time.deltaTime;
		if (timer <= 0)
			Text.Destroy (pressPToPause);
	}
	
	public void Reload()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void showPaused()
	{
		foreach(GameObject g in pauseObjects)
		{
			g.SetActive(true);
		}
	}
	
	public void hidePaused()
	{
		foreach(GameObject g in pauseObjects)
		{
			g.SetActive(false);
		}
	}
	
	public void LoadLevel(string level)
	{
		Application.LoadLevel(level);
	}
}
