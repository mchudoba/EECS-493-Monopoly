using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuInteraction : MonoBehaviour
{
	// Network objects
	NetworkManager networkManager;

	// HostMenu objects
	private InputField serverNameField;
	private Button createServerButton;

	void Awake()
	{
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

		serverNameField = GameObject.Find("ServerNameField").GetComponent<InputField>();
		createServerButton = GameObject.Find("CreateServerButton").GetComponent<Button>();
	}

	public void ServerNameInput()
	{
		if (serverNameField.text == "")
			createServerButton.interactable = false;
		else
			createServerButton.interactable = true;
	}

	public void CreateServer()
	{
		string serverName = serverNameField.text;
		networkManager.StartServer(serverName);
	}

	public void FindServers()
	{
		networkManager.FindServers();
	}
}