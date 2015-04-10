using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public GameObject PlayMenu;
	public GameObject HostMenu;
	public GameObject JoinMenu;
	public GameObject LobbyMenu;

	private GameObject activeMenu;

	void Awake()
	{
		activeMenu = PlayMenu;

		PlayMenu.SetActive(true);
		HostMenu.SetActive(true);
		JoinMenu.SetActive(true);
		LobbyMenu.SetActive(true);
	}

	void Start()
	{
		HostMenu.SetActive(false);
		JoinMenu.SetActive(false);
		LobbyMenu.SetActive(false);
	}

	public void ShowPlayMenu()
	{
		activeMenu.SetActive(false);
		PlayMenu.SetActive(true);
		activeMenu = PlayMenu;
	}

	public void ShowHostMenu()
	{
		activeMenu.SetActive(false);
		HostMenu.SetActive(true);
		activeMenu = HostMenu;
	}

	public void ShowJoinMenu()
	{
		activeMenu.SetActive(false);
		JoinMenu.SetActive(true);
		activeMenu = JoinMenu;
	}

	public void ShowLobbyMenu()
	{
		activeMenu.SetActive(false);
		LobbyMenu.SetActive(true);
		activeMenu = LobbyMenu;
	}

	public void StartGame()
	{
		Application.LoadLevel("Game_Board");
	}
}