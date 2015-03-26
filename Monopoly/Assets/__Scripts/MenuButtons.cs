using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour {

	public GameObject MainMenu;
	public GameObject PlayMenu;
	public GameObject SettingsMenu;

	void Start(){
		MainMenu = this.gameObject;
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
