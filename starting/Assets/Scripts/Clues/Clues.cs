﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Clues : MonoBehaviour
{
	public GameObject clueObj;
	private GameObject alert;
	[HideInInspector] public bool showAlert;

	private PauseGame isPaused;
	[HideInInspector] public bool showClue;
	string[] possibleKeys = new string[6]{"r","t","y","u","f","g"};
	public static string theKey;
	string[] initialClues = new string[4]{"Procure mais dicas","Fuja dos bate bolas","Ache sua casa","Cameras"};
	string[,] array2d = new string[6,4]{{"Sua casa não é dourada","Sua casa não é laranja","Sua casa é da cor do chocolate"," pra entrar"},
						{"Sua casa não é marrom","Sua casa não é dourada","Sua casa é da cor de uma fruta","T pra entrar"},
						{"Sua casa não é laranja","Sua casa não é marrom","Sua casa é camuflada","C pra entrar"},
						{"Sua casa não é verde","Sua casa não é roxa","Sua casa é da cor do mar","U pra entrar"},
						{"Sua casa não é azul", "Sua casa não é roxa","Sua casa é da cor do mato","H pra entrar"},
						{"Sua casa não é verde","Sua casa não é azul","Sua casa é da cor de um tipo de uva","B pra entrar"}};
	Vector3[] InitialLocation = new Vector3[4]{new Vector3(10,0,0),new Vector3(0,-30,0),new Vector3(-70,-42,0),new Vector3(93,10,0)};

	private GameObject notepad;
	public static List<string> clues2Show = new List<string> ();
	private List<GameObject> clueStart = new List<GameObject>();
	private List<GameObject> cluesTxt = new List<GameObject>();
	public List<Text> notebookClues = new List<Text>();
	public static List<string> cluesColected = new List<string>();

	void Awake()
	{
		alert = GameObject.FindGameObjectWithTag ("Alert");
		theKey = possibleKeys [Random.Range (0, possibleKeys.Length)];
		array2d[RandomHouse.goldenHouse,3] = theKey.ToUpper()+ " para entrar";
	}

	void Start ()
	{
		isPaused = GameObject.Find ("GameManager").GetComponent<PauseGame> ();
		showClue = false;
		notepad = GameObject.Find ("Notepad");

		notebookClues = new List<Text>();
		cluesColected = new List<string>();
		StartCoroutine (WaitClue ());
		clues2Show = new List<string> ();

		for (int i = 0; i < initialClues.Length; i++)
		{
			clueStart.Add(Instantiate(clueObj, GameObject.Find ("Player").transform.position + InitialLocation[i],
			                          Quaternion.identity) as GameObject);
			clueStart[i].GetComponent<ClueObj>().stringClueTxt = initialClues[i];
			clueStart[i].GetComponent<ClueObj>().alert = alert;
			clues2Show.Add(initialClues[i]);
		}
	}

	void Update()
	{
		if(!isPaused.paused)
		{
			Debug.Log(theKey);
			for (int i = 0; i < cluesColected.Count; i++)
			{
				if (cluesColected[i] != null)
					notebookClues[i].text = i + 1 + ". " + cluesColected[i];
			}

			if (Input.GetKeyDown (KeyCode.I))
				ShowClues();
			
			if (showAlert)
				alert.SetActive(true);
			else if(!showAlert)
				alert.SetActive(false);
			
			if(showClue)
				notepad.SetActive(true);
			else if(!showClue)
				notepad.SetActive(false);
		}
	}

	IEnumerator WaitClue()
	{
		yield return new WaitForSeconds (0.001f);
		GameObject[] locais = GameObject.FindGameObjectsWithTag ("enemy");
		for (int i = 0; i < 4; i++)
		{
			cluesTxt.Add(Instantiate (clueObj, locais[i].transform.position,Quaternion.identity) as GameObject);
			cluesTxt[i].GetComponent<ClueObj>().stringClueTxt = array2d[RandomHouse.goldenHouse, i];
			cluesTxt[i].GetComponent<ClueObj>().alert = alert;
			clues2Show.Add(cluesTxt[i].GetComponent<ClueObj>().stringClueTxt);
		}
		getTexts();
	}

	void getTexts()
	{
		foreach (Transform t in notepad.transform)
		{
			foreach (Transform texto in t.transform)
			{
				if (texto.gameObject.name.Substring (0, 4) == "Text")
				{
					notebookClues.Add(texto.gameObject.GetComponent<Text>());
				}
			}
		}
		notepad.SetActive (false);
	}

	public void ShowClues()
	{
		showClue = !showClue;
		showAlert = false;
	}
}
