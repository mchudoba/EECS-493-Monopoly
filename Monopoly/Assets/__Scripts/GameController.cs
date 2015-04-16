using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {
	public float initialMoney = 30f;


	public GameObject RollPanel;
	public Button tap;
	public Text tapText;

	public GameObject actionPanel;
	public Button no;
	public Text information;
	public Image property;
	public Image chancecard;

	//Not sure if you need this for something lol:
	public Image chance1;
	public Image chance2;
	public Image chance3;
	public Image chance4;
	public Image chance5;
	public Image chance6;
	public Image chance7;
	public Image chance8;
	public Image chance9;
	public Image chance10;
	public Image chance11;
	public Image chance12;
	public Image chance13;
	public Image chance14;
	public Image chance15;
	public Image chance16;
	public Image chance17;
	public Image chance18;
	public Image chance19;
	public Image chance20;
	public Image chance21;
	public Image chance22;
	public Image chance23;
	public Image chance24;

	public GameObject P1;
	public GameObject P2;
	public GameObject P3;
	public GameObject P4;
	public GameObject[] players;

	public int turn = 0;

	public bool initialroll = true;

	public static int tuition = 0;

	//arrays here are 1-indexed, ignore 0
	public float[] money;
	public int[] diceval;
	public GameObject[] playerorder;
	public int[] playerindex;

	//public Image[] chanceDrawPile; 
	//public Image[] chanceDiscardPile; 
	//Used for deck of chance cards:
	public List<int> chanceDrawPile;
	public List<int> chanceDiscardPile;
	public int chanceIndex;
	public int discardIndex;
	public int numChance;


	public void nextTurn(){
		++turn;
		if (turn > 4)
			turn = 1;
	}

	// Use this for initialization
	void Start () {
		Debug.Log (" ~~~~~~~~~~~~~~~~ GameController");
		players = new GameObject[5];
		players [1] = P1;
		players [2] = P2;
		players [3] = P3;
		players [4] = P4;


		money = new float[5];
		diceval = new int[5];
		playerorder = new GameObject[5];
		playerindex = new int[5];		//playerindex used for input for what player is doing what...essentially an int
		for (int i = 0; i < 5; ++i) {
			money [i] = initialMoney;
			diceval[i] = 0;
			playerindex[0] = 0;
		}

		playerorder [1] = P1;
		playerorder [2] = P2;
		playerorder [3] = P3;
		playerorder [4] = P4;

		initializeChancePile (); 
	}
	
	// Update is called once per frame
	void Update () {
		if (Dice.rolling || actionPanel.activeInHierarchy) {
			tap.gameObject.SetActive (false);
		} else {
			tap.gameObject.SetActive(true);
			if(diceval[turn] > 0 && !initialroll){
				playerorder[turn].GetComponent<MovePiece>().moveTowardsTarget(diceval[turn]);

				//Debugging Stuff:
				Debug.Log ("CHECK WHERE LAND");
				Debug.Log (tuition);
				Debug.Log (playerorder[turn].GetComponent<MovePiece>().targetIndex);

				//Does something depending on what space it lands:
				switch (playerorder[turn].GetComponent<MovePiece>().targetIndex) {
				case 0: //GO!
					Debug.Log("Land on GO!");
					//Nothing, should Add $200 in MovePiece.cs *Parthiv implemented*
					break;
				case 3: case 7: case 14: case 18: case 24: case 28: //CHANCE SPACES
					landOnChance(playerindex[turn]);
					break;
				case 5: case 20: //Pay $2 to Participate in the Hackathon/ Design expo
					Debug.Log ("Pay $2 to Participate");
					//Change money on player 
					changeMoney(playerindex[turn], -2);
					//Add money to "Free Tuition" 
					tuition += 2;
					break;
				case 6: case 11: case 22: case 30: //ROLL AGAIN! Northwood, Commuter/Souther, Bursley/Baits, Diag
					//Roll again
					Debug.Log ("Roll Again");
					turn--;
					//ButtonRoll ();
					break;	
				case 10: //Visiting
					//DO NOTHING!
					Debug.Log ("Just Visiting");
					break;
				case 16: //FREE TUITION
					//Collect all the money that is stocked up on this space
					Debug.Log ("Free Tuition");
					changeMoney(playerindex[turn], tuition);
					tuition = 0; 
					break;
				case 26: //Go to JAIL
					Debug.Log ("-3$ and Go to JAIL!");
					changeMoney(playerindex[turn],-3);
					playerorder[turn].GetComponent<MovePiece>().jail = true;
					playerorder[turn].GetComponent<MovePiece>().targetIndex = 10;
					break;
				default: //Properties:
					Debug.Log ("Property!");
					//INSERT PROPERTY IMPLEMENTATION:

					break;
				}

				diceval[turn] = 0;
			}
		}
	}

	public void ButtonRoll(){
		Debug.Log ("&&&&&&&&    &&&&&&&&&&&   &&&&&&&&&&&&&&&& ENTER BUTTON ROLL");
		nextTurn ();
		diceval [turn] = Dice.Roll ();

		//after initial setup
		if (initialroll && turn == 4) {
			Debug.Log ("^$^$^$^$^$^$^  ^$^$^$^$^$^$^$  ^$^$^$^$^$^$^$^$ ENTER INITIAL ROLL");

			initialroll = false;

			diceval [0] = -1;
			int counter = 1;
			while (diceval[0] < 0) {
				int ind = diceval.ToList ().IndexOf (diceval.Max ());
				switch (ind) {
				case 1:
					playerorder [counter] = P1;
					playerindex [counter] = 1;
					break;
				case 2:
					playerorder [counter] = P2;
					playerindex [counter] = 2;
					break;
				case 3:
					playerorder [counter] = P3;
					playerindex [counter] = 3;
					break;
				case 4:
					playerorder [counter] = P4;
					playerindex [counter] = 4;
					break;
				default:
					break;
				}

				diceval [ind] = 0;
				++counter;
				if (counter > 4)
					diceval [0] = 0;
			}

			return; //no moving on initial roll
		} else if (initialroll == true && turn != 4) {	//Rolling for order, do nothing until all 4 players roll
			Debug.Log ("88888888888888888888888888888 ROLLING FOR ORDER");
			return; 
		} else {
		


			/*call standard functions*/
			//playerorder[turn].move(diceval[turn]);
			Debug.Log ("$$$$$ ^^^^^^^^^^ $$$$$$$$$ ^^^^^^^^^ $$$$$$$$$$$$$$$$$$$ END OF ORDER");
			Debug.Log (playerorder [turn]);
			Debug.Log (diceval[turn]);

			//actual movement occurs in update once dice is no longer moving
			//whatever function occurs on that space will happen here
		}
	}

	//send index of player (P1 is 1) and amount to add. Negative amount subtracts money.
	public void changeMoney(int player, float amount){
		money [player] += amount;
	}


	//Draws a random Chance Card from array and then does the action specified:
	public void landOnChance(int player) {
		Debug.Log ("Land on Chance");
		//Image chanceIndex = chanceDrawPile[Random.Range(0, chanceDrawPile.Length)];
		chanceIndex = Random.Range (0, chanceDrawPile.Count); //Get random card

		//Debugging/Testing Purposes:
		Debug.Log (chanceIndex);
		Debug.Log (chanceDrawPile.Count);
		//chanceIndex = 7; //testing variable

		//Goes to chanceDrawPile, gets corresponding Chance Card, does Action:
		switch(chanceIndex) {
		case 0:
			Debug.Log ("Go to the Art & Architecture Building");
			//Space 13:
			//NEED TO PUT SOMETHING TO MAKE IT STOP/THE INTERFACE STUFF
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 13;
			//Is the GO thing working? Yes
			Debug.Log ("Add Property interface/function");
			break;
		case 1:
			Debug.Log ("Ride on Bursley-Baits Bus and roll again");
			//Space 22:
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 22;
			turn--;
			break;
		case 2:
			Debug.Log ("Ride on Commuter North/South Bus and roll again");
			//Space 11:
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 11;
			turn--;
			break;
		case 3:
			Debug.Log ("Go to the Design Expo and pay $2");
			//Space 20:
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 20;
			Debug.Log ("Pay $2 to Participate");
			//Change money on player 
			changeMoney(playerindex[turn],-2);
			//Add money to "Free Tuition" 
			tuition += 2;
			break;
		case 4:
			Debug.Log ("Ride on Diag-to-Diag Bus and roll again");
			//Space 30:
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 30;
			turn--;
			break;
		case 5:
			Debug.Log ("Go to the DUDE");
			//Space 12:
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 12;
			Debug.Log ("Add Property interface/function");
			break;
		case 6:
			Debug.Log ("Go to the EECS Building");
			//Space 29:
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 29;
			Debug.Log ("Add Property interface/function");
			break;
		case 7:
			Debug.Log ("Pay $3 to take the bus to GG Brown Laboratory");
			//Space JAIL:
			changeMoney(playerindex[turn], -3);
			playerorder[turn].GetComponent<MovePiece>().jail = true;
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 10;
			break;
		case 8:
			Debug.Log ("Go to Go Blue! Collect $2 pocket money as you pass");
			//Space 0:
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 0;
			break;
		case 9:
			Debug.Log ("Go to the Hackathon and pay $2");
			//Space 5:
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 5;
			Debug.Log ("Pay $2 to Participate");
			//Change money on player 
			changeMoney(playerindex[turn],-2);
			//Add money to "Free Tuition" 
			tuition += 2;
			break;
		case 10:
			Debug.Log ("Ride on Northwood Bus and roll again");
			//Space 6:
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 6;
			turn--;
			break;
		case 11: 
			Debug.Log ("Go to the Space Research Building");
			//Space 25:
			playerorder[turn].GetComponent<MovePiece>().targetIndex = 25;
			Debug.Log ("Add Property interface/function");
			break;
		case 12:
			Debug.Log ("Free Pizza Stand Green"); 
			//INSERT Function for free pizza stands
			break;
		case 13:
			Debug.Log ("Free Pizza Stand Light Blue"); 
			//INSERT Function for free pizza stands

			break;
		case 14:
			Debug.Log ("Free Pizza Stand Light Blue"); 
			//INSERT Function for free pizza stands

			break;
		case 15: 
			Debug.Log ("Free Pizza Stand Orange");
			//INSERT Function for free pizza stands

			break;
		case 16: 
			Debug.Log ("Free Pizza Stand Orange"); 
			//INSERT Function for free pizza stands

			break;
		case 17: 
			Debug.Log ("Free Pizza Stand Purple"); 
			//INSERT Function for free pizza stands

			break;
		case 18: 
			Debug.Log ("Free Pizza Stand Red"); 
			//INSERT Function for free pizza stands

			break;
		case 19:
			Debug.Log ("Free Pizza Stand Red");
			//INSERT Function for free pizza stands

			break;
		case 20: 
			Debug.Log ("Free Pizza Stand Light Blue"); 			
			//INSERT Function for free pizza stands

			break;
		case 21: 
			Debug.Log ("Free Pizza Stand Light Blue"); 
			//INSERT Function for free pizza stands

			break;
		case 22: 
			Debug.Log ("Free Pizza Stand Yellow"); 			
			//INSERT Function for free pizza stands

			break;
		case 23: 
			Debug.Log ("Free Pizza Stand Yellow"); 
			//INSERT Function for free pizza stands

			break;
		} //End of conditionals for drawn chance card

		//Now Add Drawn Chance Card to chanceDiscardPile and delete it from chanceDrawPile
		chanceDiscardPile.Add (chanceIndex);
		chanceDrawPile.RemoveAt (chanceIndex);
	
		//If no more draw pile, make discard new draw pile
		if (chanceDrawPile.Count == 0) {
			chanceDrawPile = chanceDiscardPile;
		}

	
	}

	public void initializeChancePile() {
		Debug.Log ("Initialize Chance Pile");
		/*chanceDrawPile = new Image[24];
		chanceDiscardPile = new Image[24];

		chanceDrawPile [0] = chance1;
		chanceDrawPile [1] = chance2;
		chanceDrawPile */
		//Add rest of cards: (can't figure out any other way than hardcode

		//Other way with just indexing an int:
		chanceDrawPile = new List<int>();
		chanceDiscardPile = new List<int>();
		numChance = 24;
		discardIndex = 0;

		for (int i = 0; i < numChance; i++) {
			chanceDrawPile.Add(i);
		}
			

	}
	
}
