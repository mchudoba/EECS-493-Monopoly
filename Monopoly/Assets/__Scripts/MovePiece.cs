using UnityEngine;
using System.Collections;

public class MovePiece : MonoBehaviour {

	public GameController gc;
	public Vector3[] boardSpaces; 
	public Vector3 inJail;
	private int spaceTotal = 32;	//sets number of spaces of board
	public int currentIndex = 0;	//sets the piece's current position
	public int targetIndex = 0;		//sets the piece's target position
	public float speed = 5f; 		//speed for "Vector3.MoveTowards
	public int counter = 0; 		//counter used to control how fast pieces move
	public bool jail = false; 			//true = in jail, false = free
	public bool noCollect = false;		//true = event where you don't collect on go (chance, etc.), false = can collect
	public bool rolled = false;
	public bool initialRoll = false;		//initial roll still has not been rolled

	public float timer = 0;

	int player = 0; //which player this is; set in Start

	public bool NotAtTarget(){
		return (!jail && transform.position != boardSpaces [targetIndex]) || (jail && transform.position != inJail);
	}

	// Use this for initialization
	void Start () {
		gc = GameObject.Find ("Interface").GetComponent<GameController> ();
		initBoardArray ();	//Initializes the space positioning into arrays for each piece

		if(gameObject.tag == "P1") player = 1;
		else if (gameObject.tag == "P2") player = 2;
		else if (gameObject.tag == "P3") player = 3;
		else player = 4;

		transform.position = boardSpaces [0];
	}
	
	// Update is called once per frame
	void Update () {

		/*Debug.Log (currentIndex);
		Debug.Log (targetIndex);
		Debug.Log ("COUNTER");*/
	
		/*Debug.Log (counter);
		if (Dice.currentSide == 6) {
			Debug.Log ("IS THIS WORKING LOL?");
		}
	
		Debug.Log (Dice.rolling == false);
		Debug.Log (rolled);
		Debug.Log (initialRoll);
		if (Dice.rolling) { //dice is still rolling/initial, so wait
			Debug.Log ("Waiting for end of roll/first roll");
			initialRoll = false;
			return;
		} else if (Dice.rolling == false && rolled == true && initialRoll == false) { //dice finished rolling and was rolled
			Debug.Log ("@@@@@@@@@ MOVE TOWARDS TARGET LOCATION! @@@@@@@@");
			targetIndex = currentIndex + Dice.currentSide;
			moveTowardsTarget ();
			rolled = false;
		} else { //dice is finished rolling and got to target location and is waiting
			Debug.Log ("Waiting for next roll");
			return;
		}*/

		//moveTowardsTarget ();


		if (NotAtTarget()) {
			if(currentIndex == 31) {	//If reach end of boardSpaces array
				transform.position = Vector3.MoveTowards (transform.position, boardSpaces [0], speed * Time.deltaTime);
				//transform.position = boardSpaces[0];
			}
			else if (currentIndex == 9 && jail == true) {	//going to jail
				Debug.Log ("JAIL!");
				transform.position = Vector3.MoveTowards (transform.position, inJail, speed * Time.deltaTime);
				Debug.Log (inJail);
				Debug.Log (transform.position);
				//targetIndex = 10; //for space landing:
				gc.changeMoney(player, -3);
				//transform.position = inJail;
			}
			else {	//Traverse one space ahead at a time
				transform.position = Vector3.MoveTowards (transform.position, boardSpaces [currentIndex + 1], speed * Time.deltaTime);
				//transform.position = boardSpaces[currentIndex+1];
				
			}

			if(transform.position == boardSpaces[(currentIndex + 1) % 32]){
				if ((jail == false || noCollect == false) && ((currentIndex + 1) % 32 == 0)) {
					Debug.Log ("GO!");

					gc.changeMoney(player, 2);
					
				} 

				currentIndex = (currentIndex + 1) % 32;

				//decide if turn is over
				if(currentIndex == targetIndex) gc.nextTurn();
			}

			if(transform.position == inJail && jail){
				currentIndex = 9;
			}
		}
	}


	//Initialize the array of positions for each piece:
	void initBoardArray() {

		boardSpaces = new Vector3[spaceTotal];

		int index = 0;
		int currentIndex = 0;
		float x = -5.3f;
		float y = -7.3f;
		float z = -1f;

		if (gameObject.tag == "P1") {	//sphere
			Debug.Log ("SPHERE");
			boardSpaces[index] = new Vector3 (x, y, z); //GO
		}
		else if(gameObject.tag == "P2") {//cube
			Debug.Log ("CUBE");
			x = -4.2f;
			y = -7.3f;
			boardSpaces[index] = new Vector3 (x, y, z);
		}
		else if(gameObject.tag == "P3") {//capsule
			Debug.Log ("CAPSULE");
			x = -5.3f;
			y = -8.2f;
			boardSpaces[index] = new Vector3 (x, y, z); 
		}
		else if(gameObject.tag == "P4") {//cylinder
			Debug.Log ("CYLINDER");
			x = -4.2f;
			y = -8.2f;
			boardSpaces[index] = new Vector3 (x, y, z);
		}

		/*Debug.Log (index);
		Debug.Log (currentIndex);
		Debug.Log (boardSpaces[index]);*/

		index++;
		
		//left side
		for (; index < 11; index++) {
			if (index == 1) {	//Sets first space after corner
				if(gameObject.tag == "P1") {
					x = -5.3f;
					y = -5.7f;
				}
				else if (gameObject.tag == "P2") {
					x = -4.3f;
					y = -5.7f;
				}
				else if (gameObject.tag == "P3") {
					x = -5.3f;
					y = -6.3f;
				}
				else if (gameObject.tag == "P4") {
					x = -4.3f;
					y = -6.3f;
				}
			}
			else if (index == 10) {	//Sets corner space
				if(gameObject.tag == "P1") {
					x = -5.0f;
					y = 8.4f;
				}
				else if (gameObject.tag == "P2") {
					x = -4.2f;
					y = 8.4f;
				}
				else if (gameObject.tag == "P3") {
					x = -5.4f;
					y = 8.0f;
				}
				else if (gameObject.tag == "P4") {
					x = -5.4f;
					y = 7.3f;
				}
			}
			else if (index != 10) {	//Increment non-corner spaces
				y += 1.5f;
			}

			//Debug.Log (index);
			boardSpaces [index] = new Vector3(x,y,z);	//sets new position at index
		}
		
		//top side
		for (; index < 17; index++) {
			if (index == 11) {	//sets space after corner
				if(gameObject.tag == "P1") {
					x = -3.3f;
					y = 8.2f;
				}
				else if (gameObject.tag == "P2") {
					x = -2.7f;	
					y = 8.2f;
				}
				else if (gameObject.tag == "P3") {
					x = -3.3f;
					y = 7.3f;
				}
				else if (gameObject.tag == "P4") {
					x = -2.7f;
					y = 7.3f;
				}
			}
			else if (index != 16) { //sets non-corner spaces
				x += 1.5f;
			}
			else {	//sets corner space
				if(gameObject.tag == "P1" || gameObject.tag == "P3") {
					x = 4.3f;
				}
				else if (gameObject.tag == "P2" || gameObject.tag == "P4") {
					x = 5.3f;	
				}
			}

			//Debug.Log (index);
			boardSpaces [index] = new Vector3(x,y,z);
		}
		
		//right side 
		for (; index < 27; index++) {
			if (index == 17) {	//sets space after corner
				if(gameObject.tag == "P1") {
					x = 4.3f;
					y = 6.3f;
				}
				else if (gameObject.tag == "P2") {
					x = 5.3f;	
					y = 6.3f;
				}
				else if (gameObject.tag == "P3") {
					x = 4.3f;
					y = 5.7f;
				}
				else if (gameObject.tag == "P4") {
					x = 5.3f;
					y = 5.7f;
				}
			}
			else if (index != 26){ //sets non-corner spaces
				y -= 1.5f;
			}
			else { //sets corner space
				if(gameObject.tag == "P1" || gameObject.tag == "P2") {
					y = -7.3f;
				}
				else if (gameObject.tag == "P3" || gameObject.tag == "P4") {
					y = -8.3f;	
				}
			}
			//Debug.Log (index);
			boardSpaces [index] = new Vector3(x,y,z);
		}
		
		//bottom side
		for (; index < 32; index++) {
			if (index == 27) { //sets space after corner
				if(gameObject.tag == "P1") {
					x = 2.7f;
					y = -7.3f;
				}
				else if (gameObject.tag == "P2") {
					x = 3.3f;	
					y = -7.3f;
				}
				else if (gameObject.tag == "P3") {
					x = 2.7f;
					y = -8.2f;
				}
				else if (gameObject.tag == "P4") {
					x = 3.3f;
					y = -8.2f;
				}
			}
			else { //sets non-corner spaces
				x -= 1.5f;
			}
			//Debug.Log (index);
			boardSpaces [index] = new Vector3(x,y,z);
		}

		//SETS THE INJAIL SPACE ON BOARD
		if (gameObject.tag == "P1") {	//sphere
			Debug.Log ("SPHERE");
			inJail = new Vector3(-4.8f, 7.9f, -1f);
		}
		else if(gameObject.tag == "P2") {//cube
			Debug.Log ("CUBE");
			inJail = new Vector3(-4.8f, 7.2f, -1f);
			
		}
		else if(gameObject.tag == "P3") {//capsule
			Debug.Log ("CAPSULE");
			inJail = new Vector3(-4.2f, 7.9f, -1f);
		}
		else if(gameObject.tag == "P4") {//cylinder
			Debug.Log ("CYLINDER");
			inJail = new Vector3(-4.2f, 7.2f, -1f);
		}

		//resets index back to start:
		currentIndex = 0;
		index = 0;

		//DEBUGGING PURPOSES
		/*Debug.Log ("CHECK IF INITIALIZED");
		for (int i = 0; i < 32; i++) {
			Debug.Log ("CHECK IF INITIALIZED");
			Debug.Log (i);
			Debug.Log (boardSpaces[i]);
		}*/
	} //END OF initBoardArray FUNCTION

	//Moves Object Towards Target Location
	public void moveTowardsTarget(int roll)  {
		targetIndex += roll;
		targetIndex %= 32;

	} // End of moveTowardsFunction

	IEnumerator Begin()
	{
		yield return StartCoroutine(Wait(5f));
		//3.1415 seconds later
	}

	IEnumerator Wait(float delay)
	{
		yield return new WaitForSeconds(delay);
		Debug.Log ("Waited a sec");
	}



}




