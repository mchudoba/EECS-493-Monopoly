using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuInteraction : MonoBehaviour
{
	// Network objects
	NetworkManager networkManager;
	NetworkView netView;

	// StartMenu objects
	private InputField playerNameField;
	private Button startGameButton;

	// HostMenu objects
	private InputField serverNameField;
	private Button createServerButton;

	// JoinMenu objects
	private GameObject refreshList;
	private Text searchingText;
	private Text noGamesText;
	private HostData serverData;
	private Button joinButton;

	// PopupMenu objects
	private Text messageText;
	private Button okayButton;
	private Button confirmButton;
	private Button cancelButton;

	void Awake()
	{
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		netView = GetComponent<NetworkView>();

		// StartMenu objects
		playerNameField = GameObject.Find("NameField").GetComponent<InputField>();
		startGameButton = GameObject.Find("StartButton").GetComponent<Button>();
		if (PlayerPrefs.HasKey("Player Name"))
		{
			playerNameField.text = PlayerPrefs.GetString("Player Name");
			startGameButton.interactable = true;
		}
		else
			startGameButton.interactable = false;

		// HostMenu objects
		serverNameField = GameObject.Find("ServerNameField").GetComponent<InputField>();
		createServerButton = GameObject.Find("CreateServerButton").GetComponent<Button>();

		// JoinMenu objects
		refreshList = GameObject.Find("RefreshList");
		searchingText = GameObject.Find("SearchingText").GetComponent<Text>();
		noGamesText = GameObject.Find("NoGamesText").GetComponent<Text>();
		joinButton = GameObject.Find("JoinButton").GetComponent<Button>();
		refreshList.SetActive(false);
		searchingText.enabled = true;
		noGamesText.enabled = false;

		// PopupMenu objects
		messageText = GameObject.Find("MessageText").GetComponent<Text>();
		okayButton = GameObject.Find("OkayButton").GetComponent<Button>();
		confirmButton = GameObject.Find("ConfirmButton").GetComponent<Button>();
		cancelButton = GameObject.Find("CancelButton").GetComponent<Button>();
	}

	void Update()
	{
		if (networkManager.isRefreshing)
		{
			refreshList.SetActive(false);
			searchingText.enabled = true;
			noGamesText.enabled = false;
		}
		else
		{
			refreshList.SetActive(true);
			searchingText.enabled = false;

			if (!networkManager.foundGames)
				noGamesText.enabled = true;
		}
	}

	public void PlayerNameInput()
	{
		if (playerNameField.textComponent.text == "")
			startGameButton.interactable = true;//false;
		else
			startGameButton.interactable = true;

		string str = playerNameField.text;
		if (str == "")
			str = "testplayer";
		PlayerPrefs.SetString("Player Name", playerNameField.text);
	}

	public void ServerNameInput()
	{
		if (serverNameField.textComponent.text == "")
			createServerButton.interactable = true;//false;
		else
			createServerButton.interactable = true;
	}

	public void CreateServer()
	{
		string serverName = serverNameField.text;
		if (serverName == "")
			serverName = "test";
		networkManager.StartServer(serverName);
	}

	public void FindServers()
	{
		noGamesText.enabled = false;
		networkManager.FindServers();
	}

	public void SetServerToConnect(HostData _data)
	{
		serverData = _data;
		joinButton.interactable = true;
	}

	public void ConnectToServer()
	{
		networkManager.ConnectToServer(serverData);
	}

	public void LeaveLobbyPopup()
	{
		if (Network.isServer)
			messageText.text = "Leaving the lobby will remove all players from the game. Continue?";
		else if (Network.isClient)
			messageText.text = "Leaving the lobby will remove you from the game. Continue?";
		else
			Debug.LogError("Trying to leave lobby, but not a client or server");

		okayButton.gameObject.SetActive(false);
		confirmButton.gameObject.SetActive(true);
		cancelButton.gameObject.SetActive(true);
	}

	public void LeaveLobbyConfirm()
	{
		if (Network.isServer)
		{
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}
		else if (Network.isClient)
		{
			Network.Disconnect();
		}
		else
		{
			Debug.LogError("Leaving lobby: not a server or client");
		}
	}

	public void ServerDisconnectPopup()
	{
		okayButton.gameObject.SetActive(true);
		confirmButton.gameObject.SetActive(false);
		cancelButton.gameObject.SetActive(false);
		messageText.text = "You have been disconnected from the game";
	}

	public void StartGame()
	{
		Debug.Log("Starting game");
		netView.RPC("startGameRPC", RPCMode.AllBuffered);
	}

	[RPC]
	private void startGameRPC()
	{
		Application.LoadLevel("Game_Board");
	}
}