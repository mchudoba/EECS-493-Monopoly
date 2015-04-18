using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dice : MonoBehaviour {

	public Sprite s1;
	public Sprite s2;
	public Sprite s3;
	public Sprite s4;
	public Sprite s5;
	public Sprite s6;

	Image buttonImage;

	public static float timerVal = 2f;
	public static float timer = 0;
	public static float changeTimerVal = 0.1f;
	public static float changeTimer = 0;

	public static int currentSide = 6;

	public static bool rolling{
		get{
			return timer > 0;
		}
	}

	// Use this for initialization
	void Start () {
		buttonImage = gameObject.GetComponent<Image>();
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
			if(changeTimer > 0.01f){
				changeTimer -= Time.deltaTime;
			} else {
				changeSide(Random.Range (1, 7));
				changeTimer = changeTimerVal;
			}
		} else if (timer > 0) {
			timer -= Time.deltaTime;
			changeSide(currentSide);
		}
	}

	void changeSide(int side){
		switch (side) {
		case 1:
			buttonImage.sprite = s1;
			break;
		case 2:
			buttonImage.sprite = s2;
			break;
		case 3:
			buttonImage.sprite = s3;
			break;
		case 4:
			buttonImage.sprite = s4;
			break;
		case 5:
			buttonImage.sprite = s5;
			break;
		default:
			buttonImage.sprite = s6;
			break;
		}
	}
}
