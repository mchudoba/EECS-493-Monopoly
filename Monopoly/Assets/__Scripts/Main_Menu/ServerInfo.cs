using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerInfo : MonoBehaviour
{
	public Text nameText;
	public Text playerCountText;

	private HostData serverData;
	private Image thisImage;
	private Button thisButton;
	private MenuInteraction menuInteraction;

	void Awake()
	{
		menuInteraction = GameObject.Find("MenuManager").GetComponent<MenuInteraction>();

		thisImage = GetComponent<Image>();
		thisButton = GetComponent<Button>();
		thisImage.enabled = false;
		thisButton.interactable = false;

		nameText.text = "";
		playerCountText.text = "";
	}

	public void setServerData(HostData _data)
	{
		serverData = _data;

		thisImage.enabled = true;
		thisButton.interactable = true;
		nameText.text = serverData.gameName;
		playerCountText.text = serverData.connectedPlayers.ToString() + "/4";
	}

	public void clearServerData()
	{
		serverData = null;

		thisImage.enabled = false;
		thisButton.interactable = false;
		nameText.text = "";
		playerCountText.text = "";
	}

	public void selectServer()
	{
		menuInteraction.SetServerToConnect(serverData);
	}
}