using UnityEngine;
using System.Collections;

public class PlayMenuButtons : MonoBehaviour {

	public GameObject MainMenu;
	public GameObject PlayMenu;
	public GameObject SettingsMenu;

	void Start(){
		PlayMenu = this.gameObject;
	}


	public void BeginButton(){
		Application.LoadLevel("Game_Board");
	}

	public void TokenButton(){

	}

	public void BackButton(){
		PlayMenu.SetActive (false);
		MainMenu.SetActive (true);
	}
}
