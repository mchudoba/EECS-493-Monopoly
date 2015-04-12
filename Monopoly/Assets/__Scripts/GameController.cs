using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public float initialMoney = 30f;


	public GameObject boardUI;
	public Button tap;
	public Text tapText;

	public GameObject P1;
	public GameObject P2;
	public GameObject P3;
	public GameObject P4;

	public int turn = 0;

	public bool initialroll = true;

	//arrays here are 1-indexed, ignore 0
	public float[] money;
	public int[] diceval;
	public GameObject[] playerorder;


	public void nextTurn(){
		++turn;
		if (turn > 4)
			turn = 1;
	}

	// Use this for initialization
	void Start () {
		money = new float[5];
		diceval = new int[5];
		playerorder = new GameObject[5];
		for (int i = 0; i < 5; ++i) {
			money [i] = initialMoney;
			diceval[i] = 0;
		}

		playerorder [1] = P1;
		playerorder [2] = P2;
		playerorder [3] = P3;
		playerorder [4] = P4;

	}
	
	// Update is called once per frame
	void Update () {
		if (Dice.rolling) {
			tap.gameObject.SetActive (false);
		} else {
			tap.gameObject.SetActive(true);
		}
	}

	public void ButtonRoll(){
		nextTurn ();
		diceval [turn] = Dice.Roll ();
		if (initialroll && turn == 4) {
			initialroll = false;
			//TO DO: sort playerorder based on diceval
		}
	}
}
