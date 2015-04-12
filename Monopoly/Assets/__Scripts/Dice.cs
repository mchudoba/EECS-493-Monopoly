using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

	public Material side1;
	public Material side2;
	public Material side3;
	public Material side4;
	public Material side5;
	public Material side6;

	Renderer r;

	public static float timerVal = 2f;
	public static float timer = 0;

	public static int currentSide = 6;

	public static bool rolling{
		get{
			return timer > 0;
		}
	}

	// Use this for initialization
	void Start () {
		r = gameObject.GetComponent<Renderer> ();
	}

	public static int Roll(){
		timer = timerVal;
		currentSide = Random.Range (1, 7);
		return currentSide;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0.1f) {
			timer -= Time.deltaTime;
			int side = Random.Range (1, 7);
			changeSide (side);
		} else if (timer > 0) {
			timer -= Time.deltaTime;
			changeSide(currentSide);
		}
	}

	void changeSide(int side){
		switch (side) {
		case 1:
			r.material = side1;
			break;
		case 2:
			r.material = side2;
			break;
		case 3:
			r.material = side3;
			break;
		case 4:
			r.material = side4;
			break;
		case 5:
			r.material = side5;
			break;
		default:
			r.material = side6;
			break;
		}
	}
}
