using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour {

	public GameObject MainMenu;
	public GameObject PlayMenu;
	public GameObject SettingsMenu;

	void Start(){
		PlayMenu = GameObject.Find ("PlayMenu");
		SettingsMenu = GameObject.Find ("SettingsMenu");
		MainMenu = GameObject.Find ("MainMenu");

		PlayMenu.SetActive (false);
		SettingsMenu.SetActive (false);
	}

	public void PlayButton(){
		MainMenu.SetActive (false);
		PlayMenu.SetActive (true);
	}

	public void SettingsButton(){
		MainMenu.SetActive (false);
		SettingsMenu.SetActive (true);
	}

	public void ExitButton(){
		Application.Quit();
	}
}
