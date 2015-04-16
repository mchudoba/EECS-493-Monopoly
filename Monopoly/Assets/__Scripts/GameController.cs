using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {
	public float initialMoney = 30f;

	public Text debugText;

	public GameObject RollPanel;
	public Button tap;
	public Text tapText;

	public GameObject actionPanel;
	public Button no;
	public Button okay;
	public Text information;
	public Image property;
	public Image chancecard;

	//Not sure if you need this for something lol:
	public Sprite chance0;
	public Sprite chance1;
	public Sprite chance2;
	public Sprite chance3;
	public Sprite chance4;
	public Sprite chance5;
	public Sprite chance6;
	public Sprite chance7;
	public Sprite chance8;
	public Sprite chance9;
	public Sprite chance10;
	public Sprite chance11;
	public Sprite chance12;
	public Sprite chance13;
	public Sprite chance14;
	public Sprite chance15;
	public Sprite chance16;
	public Sprite chance17;
	public Sprite chance18;
	public Sprite chance19;
	public Sprite chance20;
	public Sprite chance21;
	public Sprite chance22;
	public Sprite chance23;

	public Image[] properties;
	public int[] propertyPair;
	public float[] propertyPrice;
	public int[] propertyOwner;
	public int propertyActionIndex = 0; //used to specify which property is being bought

	public GameObject P1;
	public GameObject P2;
	public GameObject P3;
	public GameObject P4;
	public GameObject[] players;

	public MovePiece P1mp;
	public MovePiece P2mp;
	public MovePiece P3mp;
	public MovePiece P4mp;

	public int turn = 1;

	public bool initialroll = true;

	public static int tuition = 0;

	//arrays here are 1-indexed, ignore 0
	public float[] money;
	public int[] diceval;
	public GameObject[] playerorder; //players in order of turns
	public int[] playerindex;		 //playerindex[turn] gets you the index of the current player in players[]
	public MovePiece[] mps;

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
		mps = new MovePiece[5];
		for (int i = 0; i < 5; ++i) {
			money [i] = initialMoney;
			diceval[i] = 0;
			playerindex[0] = 0;
		}

		propertyOwner = new int[32];
		for (int i = 0; i < 32; ++i) {
			propertyOwner[i] = 0;
		}

		playerorder [1] = P1;
		playerorder [2] = P2;
		playerorder [3] = P3;
		playerorder [4] = P4;

		P1mp = P1.GetComponent<MovePiece> ();
		P2mp = P2.GetComponent<MovePiece> ();
		P3mp = P3.GetComponent<MovePiece> ();
		P4mp = P4.GetComponent<MovePiece> ();

		mps [1] = P1mp;
		mps [2] = P2mp;
		mps [3] = P3mp;
		mps [4] = P4mp;

		initializeChancePile (); 
	}
	
	// Update is called once per frame
	void Update () {

		debugText.text = "P1: $" + money [1].ToString ("F0") + "   P2: $" + money [2].ToString ("F0")
			+ "\nP3: $" + money [3].ToString ("F0") + "   P4: $" + money [4].ToString ("F0");

		if (money [1] <= 0 || money [2] <= 0 || money [3] <= 0 || money [4] <= 0) {
			int maxindex = 1;
			float maxval = 0;
			for(int i = 1; i < 5; ++i){
				if(maxval < money[i]){
					maxval = money[i];
					maxindex = i;
				}
			}

			showMessage("Player " + maxindex + " wins with $" + maxval.ToString("F0") + "!");

		}

		if (Dice.rolling || actionPanel.activeInHierarchy ||
		    (P1mp.NotAtTarget() || P2mp.NotAtTarget() || P3mp.NotAtTarget() || P4mp.NotAtTarget())) {
			//if the die is rolling, or the actionpanel is up, or a piece is moving,
			//	disallow tap to roll
			tap.gameObject.SetActive (false);
		} else {
			if(!(tap.gameObject.activeInHierarchy)){
				if(mps[turn].jail){
					mps[turn].jail = false;
					mps[turn].noCollect = false;
					return;
				}
				tap.gameObject.SetActive(true);
			}

			if(diceval[turn] > 0 && !initialroll){
				mps[turn].moveTowardsTarget(diceval[turn]);
				tap.gameObject.SetActive(false);

				//Debugging Stuff:
				Debug.Log ("CHECK WHERE LAND");
				Debug.Log ("tuition = " + tuition);
				Debug.Log ("target = " + mps[turn].targetIndex);

				//Does something depending on what space it lands:
				switch (mps[turn].targetIndex) {
				case 0: //GO!
					Debug.Log("Land on GO!");
					//Nothing, should Add $200 in MovePiece.cs *Parthiv implemented*
					diceval[turn] = 0;
					break;
				case 3: case 7: case 14: case 18: case 24: case 28: //CHANCE SPACES
					landOnChance(playerindex[turn]);
					diceval[turn] = 0;
					break;
				case 5: case 20: //Pay $2 to Participate in the Hackathon/ Design expo
					Debug.Log ("Pay $2 to Participate");
					//Change money on player 
					changeMoney(playerindex[turn], -2);
					//Add money to "Free Tuition" 
					tuition += 2;
					diceval[turn] = 0;
					break;
				case 6: case 11: case 22: case 30: //ROLL AGAIN! Northwood, Commuter/Souther, Bursley/Baits, Diag
					//Roll again
					Debug.Log ("Roll Again");
					turn--;
					//ButtonRoll ();
					diceval[turn + 1] = 0;
					break;	
				case 10: //Visiting
					//DO NOTHING!
					Debug.Log ("Just Visiting");
					diceval[turn] = 0;
					break;
				case 16: //FREE TUITION
					//Collect all the money that is stocked up on this space
					Debug.Log ("Free Tuition");
					changeMoney(playerindex[turn], tuition);
					tuition = 0; 
					diceval[turn] = 0;
					break;
				case 26: //Go to JAIL
					Debug.Log ("-3$ and Go to JAIL!");
					changeMoney(playerindex[turn],-3f);
					mps[turn].jail = true;
					mps[turn].noCollect = true;
					mps[turn].targetIndex = 10;
					showMessage("Go to GG Brown, get lost, and lose a turn.");
					diceval[turn] = 0;
					nextTurn();
					break;
				default: //Properties:
					Debug.Log ("Property!");
					//INSERT PROPERTY IMPLEMENTATION:
					handleProperty(mps[turn].targetIndex);
					diceval[turn] = 0;
					break;
				}

			}
		}
	}

	public void ButtonRoll(){
		Debug.Log ("&&&&&&&&    &&&&&&&&&&&   &&&&&&&&&&&&&&&& ENTER BUTTON ROLL");
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
					mps[counter] = P1mp;
					break;
				case 2:
					playerorder [counter] = P2;
					playerindex [counter] = 2;
					mps[counter] = P2mp;
					break;
				case 3:
					playerorder [counter] = P3;
					playerindex [counter] = 3;
					mps[counter] = P3mp;
					break;
				case 4:
					playerorder [counter] = P4;
					playerindex [counter] = 4;
					mps[counter] = P4mp;
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
			nextTurn();
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
		Debug.Log ("chanceIndex = " + chanceIndex);
		Debug.Log ("chanceDrawPile.Count = " + chanceDrawPile.Count);
		//chanceIndex = 7; //testing variable

		//Goes to chanceDrawPile, gets corresponding Chance Card, does Action:
		switch(chanceIndex) {
		case 0:
			Debug.Log ("Go to the Art & Architecture Building");
			//Space 13:
			//NEED TO PUT SOMETHING TO MAKE IT STOP/THE INTERFACE STUFF
			showChanceCard(chance0);
			mps[turn].targetIndex = 13;
			//Is the GO thing working? Yes
			Debug.Log ("Add Property interface/function");
			break;
		case 1:
			Debug.Log ("Ride on Bursley-Baits Bus and roll again");
			//Space 22:
			showChanceCard(chance1);
			mps[turn].targetIndex = 22;
			turn--;
			diceval[turn + 1] = 0;
			break;
		case 2:
			Debug.Log ("Ride on Commuter North/South Bus and roll again");
			//Space 11:
			showChanceCard(chance2);
			mps[turn].targetIndex = 11;
			turn--;
			diceval[turn + 1] = 0;
			break;
		case 3:
			Debug.Log ("Go to the Design Expo and pay $2");
			//Space 20:
			showChanceCard(chance3);
			mps[turn].targetIndex = 20;
			Debug.Log ("Pay $2 to Participate");
			//Change money on player 
			changeMoney(playerindex[turn],-2);
			//Add money to "Free Tuition" 
			tuition += 2;
			break;
		case 4:
			Debug.Log ("Ride on Diag-to-Diag Bus and roll again");
			//Space 30:
			showChanceCard(chance4);
			mps[turn].targetIndex = 30;
			turn--;
			diceval[turn + 1] = 0;
			break;
		case 5:
			Debug.Log ("Go to the DUDE");
			//Space 12:
			showChanceCard(chance5);
			mps[turn].targetIndex = 12;
			Debug.Log ("Add Property interface/function");
			break;
		case 6:
			Debug.Log ("Go to the EECS Building");
			//Space 29:
			showChanceCard(chance6);
			mps[turn].targetIndex = 29;
			Debug.Log ("Add Property interface/function");
			break;
		case 7:
			Debug.Log ("Pay $3 to take the bus to GG Brown Laboratory");
			//Space 10:
			showChanceCard(chance7);
			changeMoney(playerindex[turn], -3);
			mps[turn].targetIndex = 10;
			break;
		case 8:
			Debug.Log ("Go to Go Blue! Collect $2 pocket money as you pass");
			//Space 0:
			showChanceCard(chance8);
			mps[turn].targetIndex = 0;
			break;
		case 9:
			Debug.Log ("Go to the Hackathon and pay $2");
			//Space 5:
			showChanceCard(chance9);
			mps[turn].targetIndex = 5;
			Debug.Log ("Pay $2 to Participate");
			//Change money on player 
			changeMoney(playerindex[turn],-2);
			//Add money to "Free Tuition" 
			tuition += 2;
			break;
		case 10:
			Debug.Log ("Ride on Northwood Bus and roll again");
			//Space 6:
			showChanceCard(chance10);
			mps[turn].targetIndex = 6;
			turn--;
			diceval[turn + 1] = 0;
			break;
		case 11: 
			Debug.Log ("Go to the Space Research Building");
			//Space 25:
			showChanceCard(chance11);
			mps[turn].targetIndex = 25;
			Debug.Log ("Add Property interface/function");
			break;
		case 12:
			Debug.Log ("Free Pizza Stand Green"); 
			//INSERT Function for free pizza stands
			showChanceCard(chance12);
			freeProperty(25);
			break;
		case 13:
			Debug.Log ("Free Pizza Stand Light Blue"); 
			//INSERT Function for free pizza stands
			showChanceCard(chance13);
			freeProperty(4);
			break;
		case 14:
			Debug.Log ("Free Pizza Stand Light Blue"); 
			//INSERT Function for free pizza stands
			showChanceCard(chance14);
			freeProperty(4);
			break;
		case 15: 
			Debug.Log ("Free Pizza Stand Orange");
			//INSERT Function for free pizza stands
			showChanceCard(chance15);
			freeProperty(17);
			break;
		case 16: 
			Debug.Log ("Free Pizza Stand Orange"); 
			//INSERT Function for free pizza stands
			showChanceCard(chance16);
			freeProperty(17);
			break;
		case 17: 
			Debug.Log ("Free Pizza Stand Purple"); 
			//INSERT Function for free pizza stands
			showChanceCard(chance17);
			freeProperty(1);
			break;
		case 18: 
			Debug.Log ("Free Pizza Stand Red"); 
			//INSERT Function for free pizza stands
			showChanceCard(chance18);
			freeProperty(13);
			break;
		case 19:
			Debug.Log ("Free Pizza Stand Red");
			//INSERT Function for free pizza stands
			showChanceCard(chance19);
			freeProperty(13);
			break;
		case 20: 
			Debug.Log ("Free Pizza Stand Royal Blue"); 			
			//INSERT Function for free pizza stands
			showChanceCard(chance20);
			freeProperty(29);
			break;
		case 21: 
			Debug.Log ("Free Pizza Stand Royal Blue"); 
			//INSERT Function for free pizza stands
			showChanceCard(chance21);
			freeProperty(29);
			break;
		case 22: 
			Debug.Log ("Free Pizza Stand Yellow"); 			
			//INSERT Function for free pizza stands
			showChanceCard(chance22);
			freeProperty(21);
			break;
		case 23: 
			Debug.Log ("Free Pizza Stand Yellow"); 
			//INSERT Function for free pizza stands
			showChanceCard(chance23);
			freeProperty(21);
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

	public void handleProperty(int index){
		if (propertyPrice [index] == 0)
			return; //not property

		if (propertyOwner [index] == playerindex [turn])
			return; //already owned

		if (propertyOwner [index] != 0) { //someone else owns
			float price = propertyPrice [index];

			if (propertyOwner [index] == propertyOwner [propertyPair [index]])
				price *= 2; //owner has both spaces in pair

			money [playerindex [turn]] -= price;
			money [propertyOwner [index]] += price;

			return;
		}

		//buy a property
		propertyActionIndex = index;
		showProperty (properties[index].sprite);
	}

	public void freeProperty(int index){
		if (propertyOwner [index] != 0
			&& propertyOwner [index] == propertyOwner [propertyPair [index]])
			return; //owned by same person

		if (propertyOwner [index] == 0) { //space is free
			properties [index].color = mps [turn].playercolor;
			propertyOwner [index] = playerindex [turn];
			return;
		} else if (propertyOwner [propertyPair [index]] == 0) { //space isn't free, but space's pair is free
			properties [propertyPair[index]].color = mps [turn].playercolor;
			propertyOwner [propertyPair[index]] = playerindex [turn];
			return;
		}

		//both spaces are owned, but by different people
		int pairindex = propertyPair [index];
		if (money [propertyOwner [index]] >= money [propertyOwner [pairindex]]) {
			properties [index].color = mps [turn].playercolor;
			propertyOwner [index] = playerindex [turn];
			return;
		} else {
			properties [pairindex].color = mps [turn].playercolor;
			propertyOwner [pairindex] = playerindex [turn];
			return;
		}
	}

	public void showChanceCard(Sprite card){
		tap.gameObject.SetActive (false);
		actionPanel.SetActive(true);
		information.text = "Chance Card";
		no.gameObject.SetActive (false);
		property.gameObject.SetActive (false);
		chancecard.gameObject.SetActive (true);
		chancecard.sprite = card;
		Time.timeScale = 0;
	}

	public void showProperty(Sprite card){
		tap.gameObject.SetActive (false);
		actionPanel.SetActive(true);
		information.text = "Buy Property?";
		no.gameObject.SetActive (true);
		property.gameObject.SetActive (true);
		chancecard.gameObject.SetActive (false);
		property.sprite = card;
		Time.timeScale = 0;
	}

	public void showMessage(string msg){
		tap.gameObject.SetActive (false);
		actionPanel.SetActive(true);
		information.text = msg;
		no.gameObject.SetActive (false);
		property.gameObject.SetActive (false);
		chancecard.gameObject.SetActive (false);
		Time.timeScale = 0;
	}

	public void NoButton(){
		actionPanel.SetActive (false);
		Time.timeScale = 1;
	}

	public void OkayButton(){
		if (property.gameObject.activeInHierarchy) {
			properties [propertyActionIndex].color = mps [turn].playercolor;
			propertyOwner [propertyActionIndex] = playerindex [turn];
			money [playerindex [turn]] -= propertyPrice [propertyActionIndex];
			actionPanel.SetActive (false);
			Time.timeScale = 1;
		} else if (chancecard.gameObject.activeInHierarchy) {
			//handle property for chance based movements
			switch (chanceIndex) {
			case 0:
			case 5:
			case 6:
			case 11:
				handleProperty (mps [turn].targetIndex);
				if (!property.gameObject.activeInHierarchy) {
					actionPanel.SetActive (false);
					Time.timeScale = 1;
				}
				break;
			default:
				actionPanel.SetActive (false);
				Time.timeScale = 1;
				break;
			}
			
		} else {
			if (money [1] <= 0 || money [2] <= 0 || money [3] <= 0 || money [4] <= 0) {
				Application.LoadLevel("Main_Menu");
			}
			actionPanel.SetActive (false);
			Time.timeScale = 1;
		}
	}
}
