using UnityEngine;
using System.Collections;

public class PlayMenuButtons : MonoBehaviour {

	public GameObject MainMenu;
	public GameObject PlayMenu;
	public GameObject SettingsMenu;

	void Start(){
		PlayMenu = GameObject.Find ("PlayMenu");
		SettingsMenu = GameObject.Find ("SettingsMenu");
		MainMenu = GameObject.Find ("MainMenu");
	}

	public void BeginButton(){
		Application.LoadLevel("_Game_Board");
	}

	public void TokenButton(){

	}

	public void BackButton(){
		PlayMenu.SetActive (false);
		MainMenu.SetActive (true);
	}
}
