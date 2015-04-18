using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public GameObject StartMenu;
	public GameObject PopupMenu;

	private GameObject activeMenu;

	void Awake()
	{
		activeMenu = StartMenu;

		StartMenu.SetActive(true);
		PopupMenu.SetActive(true);
	}

	void Start()
	{
		PopupMenu.SetActive(false);
	}

	public void ShowPopupMenu()
	{
		PopupMenu.SetActive(true);
	}

	public void StartGame()
	{
		Application.LoadLevel("Game_Board");
	}
}