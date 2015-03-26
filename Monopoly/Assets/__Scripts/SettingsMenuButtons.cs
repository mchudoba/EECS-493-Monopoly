using UnityEngine;
using System.Collections;

public class SettingsMenuButtons : MonoBehaviour {

	public GameObject MainMenu;
	public GameObject PlayMenu;
	public GameObject SettingsMenu;

	void Start(){
		SettingsMenu = this.gameObject;
	}

	public void AcceptButton(){
		//save before going back
		SettingsMenu.SetActive (false);
		MainMenu.SetActive (true);
	}

	public void SettingButton(){

	}

	public void BackButton(){
		SettingsMenu.SetActive (false);
		MainMenu.SetActive (true);
	}
}
