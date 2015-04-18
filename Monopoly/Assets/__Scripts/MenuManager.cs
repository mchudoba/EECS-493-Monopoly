using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public GameObject StartMenu;
	public GameObject PlayMenu;
	public GameObject HostMenu;
	public GameObject JoinMenu;
	public GameObject LobbyMenu;
	public GameObject PopupMenu;

	private GameObject activeMenu;

	void Awake()
	{
		activeMenu = StartMenu;

		StartMenu.SetActive(true);
		PlayMenu.SetActive(true);
		HostMenu.SetActive(true);
		JoinMenu.SetActive(true);
		LobbyMenu.SetActive(true);
		PopupMenu.SetActive(true);
	}

	void Start()
	{
		PlayMenu.SetActive(false);
		HostMenu.SetActive(false);
		JoinMenu.SetActive(false);
		LobbyMenu.SetActive(false);
		PopupMenu.SetActive(false);
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

	public void ShowPopupMenu()
	{
		PopupMenu.SetActive(true);
	}
}