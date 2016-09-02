﻿using UnityEngine;
using System.Collections;

public class Enemy2 : MonoBehaviour
{
	private FieldOfVision field;
	private PauseGame isPaused;

	private GameObject player;

	private Vector3 originalPosition;
	private Vector3 originalPositionR;

	private PolyNavAgent pagent;
	public Transform[] places2Walk;
	private bool goTo1, GoTo2;
	private bool[] goTo = new bool[2];

	public GameObject[] places = new GameObject[4];
	private bool[] playerGoTo = new bool[4];

	private bool fadeIn;
	private float fadeNum;

	public AudioClip shock;
	private int rand;
	private bool arrived;

	public Vector3[] Places;
	public GameObject[]temp;

	void Awake()
	{
		field = GetComponentInChildren<FieldOfVision> ();
	}

	void Start ()
	{ 
		temp = GameObject.FindGameObjectsWithTag ("MovE2");
		Places=  new Vector3[temp.Length]; 
		for (int i=0; i<temp.Length; i++)
		{
			Places[i] = temp[i].transform.position;
		}
		places = GameObject.FindGameObjectsWithTag ("Places") ;
		pagent = GetComponent<PolyNavAgent> ();
		rand = Random.Range (0, Places.Length);
		arrived = false;

		isPaused = GameObject.Find ("GameManager").GetComponent<PauseGame> ();

		player = GameObject.FindGameObjectWithTag ("player");

		originalPosition = transform.position;
		originalPositionR = transform.eulerAngles;

		places2Walk = GetComponentsInChildren<Transform> ();
		for (int i = 0; i < goTo.Length; i++)
		{
			goTo[i] = false; 
		}


		transform.DetachChildren ();
		places2Walk[1].gameObject.transform.SetParent(transform);
		transform.position = Places[Random.Range (0, Places.Length)];

		fadeNum = 0;
		fadeIn = false;
	}

	void Update ()
	{
		if (!isPaused.paused)
		{
			FadeIn();
			WalkAndRun();
			for (int i=0; i<temp.Length;i++)
			{
				Destroy(temp[i].gameObject);
			}
		}
	}

	void FadeIn()
	{
		if (fadeIn)
		{
			fadeNum += 5;
			GameObject.Find("FadeIn").GetComponent<SpriteRenderer>().color = new Color(GameObject.Find("FadeIn").GetComponent<SpriteRenderer>().color.r,
			                                                                           GameObject.Find("FadeIn").GetComponent<SpriteRenderer>().color.g,
			                                                                           GameObject.Find("FadeIn").GetComponent<SpriteRenderer>().color.b,
			                                                                           fadeNum/255);
			player.SetActive(false);
			GetComponent<SpriteRenderer>().enabled = false;
			
			GameObject.Find("Stamina").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("OrangeStamina").GetComponent<SpriteRenderer>().enabled = false;
			
			if(this.fadeNum > 255)
			{
				int rand = Random.Range(0, playerGoTo.Length);
				playerGoTo[rand] = true;
				player.transform.position = places[rand].transform.position;
				
				transform.position = originalPosition;
				transform.rotation = Quaternion.Euler(originalPositionR);
				field.saw = false;
				fadeIn = false;
				fadeNum = 0;
				
				player.SetActive(true);
				GameObject.Find("Stamina").GetComponent<SpriteRenderer>().enabled = true;
				GameObject.Find("OrangeStamina").GetComponent<SpriteRenderer>().enabled = true;
				
				GetComponent<SpriteRenderer>().enabled = true ; 
				GameObject.Find("FadeIn").GetComponent<SpriteRenderer>().color = new Color(GameObject.Find("FadeIn").GetComponent<SpriteRenderer>().color.r,
				                                                                           GameObject.Find("FadeIn").GetComponent<SpriteRenderer>().color.g,
				                                                                           GameObject.Find("FadeIn").GetComponent<SpriteRenderer>().color.b,
				                                                                           fadeNum / 255);
			}
			
			
		}
	}

	void WalkAndRun()
	{
		if (field.saw)
		{
			GetComponent<SpriteRenderer>().color = Color.red;
			player.GetComponent<SpriteRenderer>().color = Color.cyan;
			
			Vector2 posiplayer = player.transform.position;
			transform.position = new Vector3(transform.position.x, transform.position.y, -9.2f);
			pagent.SetDestination(posiplayer);
			pagent.maxSpeed = 20;

			if (!field.leaved)
			{
				field.saw = false;
				rand = Random.Range (0, Places.Length);
				arrived = false;
				GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
		
		if (!field.saw)
		{
			pagent.maxSpeed = 10; 

			if (!arrived)
			{
				pagent.SetDestination (Places[rand]);
				if (pagent.remainingDistance <= 0.4f)
					arrived = true; 
			}
			else
			{ 
				rand = Random.Range (0, Places.Length);
				arrived = false; 
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (field.saw)
		{
			if (other.gameObject.name.Equals ("Player"))
			{
				fadeIn = true;
				GameManager.Playsound(shock);
				GetComponent<SpriteRenderer>().color = Color.white;
				player.GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
	}
}