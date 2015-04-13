using UnityEngine;
using System.Collections;

public class MovePiece : MonoBehaviour {

	public Vector3[] boardSpaces; 
	private int spaceTotal;
	public int currentIndex;
	public int targetIndex;
	public float speed;
	public int counter;

	// Use this for initialization
	void Start () {
		spaceTotal = 32;	//sets number of spaces of board
		initBoardArray ();	//Initializes the space positioning into arrays for each piece
		currentIndex = 29;  //sets the piece's current position
		targetIndex = 28;	//sets the piece's target position 
		counter = 0;		//counter used to control how fast pieces move
		speed = 0.5f;		//speed for "Vector3.Lerp
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log (currentIndex);
		Debug.Log (targetIndex);
		Debug.Log ("COUNTER");

		Debug.Log (counter);

		if (counter % 10 == 0) {	//Change number for how fast pieces move
			if (currentIndex != targetIndex) {
				if(currentIndex == 31) {	//If reach end of boardSpaces array
					transform.position = Vector3.Lerp (boardSpaces [currentIndex], boardSpaces [0], speed * Time.deltaTime);
					transform.position = boardSpaces[0];
					currentIndex=0;
				}
				else {	//Traverse one space ahead at a time
					transform.position = Vector3.Lerp (boardSpaces [currentIndex], boardSpaces [currentIndex + 1], speed * Time.deltaTime);
					transform.position = boardSpaces[currentIndex+1];
					currentIndex++;
				}
			}
			else {	//Reach destination, now wait until targetIndex changes
				Debug.Log ("Waiting");
				return;
			}
		}
		counter++;
	}


	//Initialize the array of positions for each piece:
	void initBoardArray() {

		boardSpaces = new Vector3[spaceTotal];

		Vector3 start = new Vector3 ();
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

		Debug.Log (index);
		Debug.Log (currentIndex);
		Debug.Log (boardSpaces[index]);

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

			Debug.Log (index);
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

			Debug.Log (index);
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
			Debug.Log (index);
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
			Debug.Log (index);
			boardSpaces [index] = new Vector3(x,y,z);
		}

		//resets index back to start:
		currentIndex = 0;
		index = 0;

		//DEBUGGING PURPOSES
		Debug.Log ("CHECK IF INITIALIZED");
		for (int i = 0; i < 32; i++) {
			Debug.Log ("CHECK IF INITIALIZED");
			Debug.Log (i);
			Debug.Log (boardSpaces[i]);
		}
	}

}




