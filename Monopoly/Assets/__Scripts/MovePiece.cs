using UnityEngine;
using System.Collections;

public class MovePiece : MonoBehaviour {

	public Vector3[] boardSpaces; 
	public Vector3 position;
	// Use this for initialization
	void Start () {

		initBoardArray (1);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void initBoardArray(int tag) {

		Vector3[] boardSpaces = new Vector3[32];

		switch (tag) {
		case 0:	//sphere
			position = new Vector3 (-4.2f, -7.3f, -1f); //GO
			break;
		case 1: //cube
			position = new Vector3 (-5.2f, -7.3f, -1f);
			break;
		case 2: //capsule
			position = new Vector3 (-5.2f, -8.2f, -1f); 
			break;
		case 3: //cylinder
			position = new Vector3 (-4.2f, -8.2f, -1f);
			break;
		}
		
		int index = 0;
		
		Debug.Log (index);
		Debug.Log (position);
		
		boardSpaces [index] = position;
		
		index++;
		
		//left side
		for (; index < 11; index++) {
			if (index != 10) {
				position.y += 1.5f;
			}
			else {
				position.y += 2.0f;
			}
			Debug.Log (index);
			Debug.Log (position);
			boardSpaces [index] = position;
		}
		
		//top side
		for (; index < 17; index++) {
			if (index != 16) {
				position.x += 1.5f;
			}
			else {
				position.x += 2.0f;
			}
			Debug.Log (index);
			Debug.Log (position);
			boardSpaces [index] = position;
		}
		
		//right side 
		for (; index < 27; index++) {
			if (index == 17) {
				position.y -= 2.0f;
			}
			else {
				position.y -= 1.5f;
			}
			Debug.Log (index);
			Debug.Log (position);
			boardSpaces [index] = position;
		}
		
		//bottom side
		for (; index < 32; index++) {
			if (index == 27) {
				position.x -= 2.0f;
			}
			else {
				position.x -= 1.5f;
			}
			Debug.Log (index);
			Debug.Log (position);
			boardSpaces [index] = position;
		}

		//DEBUGGING PURPOSES
		Debug.Log ("CHECK IF INITIALIZED");
		for (int i = 0; i < 32; i++) {
			Debug.Log (i);
			Debug.Log (boardSpaces[i]);
		}
		
	}


}




