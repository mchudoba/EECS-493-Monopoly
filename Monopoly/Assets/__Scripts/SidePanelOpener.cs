using UnityEngine;
using System.Collections;

public class SidePanelOpener : MonoBehaviour
{
	public static bool sidePanelOpen = false;

	private GameObject emptySpace;
	private Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
		emptySpace = GameObject.Find("EmptySpace");
		emptySpace.SetActive(false);
	}

	public void OpenPanel()
	{
		sidePanelOpen = true;
		anim.SetBool("OpenPanel", true);
		emptySpace.SetActive(true);
	}

	public void ClosePanel()
	{
		sidePanelOpen = false;
		anim.SetBool("OpenPanel", false);
		emptySpace.SetActive(false);
	}

	public void RestartLevel()
	{
		sidePanelOpen = false;
		Application.LoadLevel("Game_Board");
	}

	public void ReturnToMenu()
	{
		sidePanelOpen = false;
		Application.LoadLevel("Main_Menu");
	}
}