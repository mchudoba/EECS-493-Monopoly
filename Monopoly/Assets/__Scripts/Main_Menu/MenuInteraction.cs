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
		if (playerNameField.text == "")
			startGameButton.interactable = false;
		else
			startGameButton.interactable = true;

		PlayerPrefs.SetString("Player Name", playerNameField.text);
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

	public void StartGame()
	{
		netView.RPC("startLevelRPC", RPCMode.AllBuffered);
	}

	[RPC]
	private void startGameRPC()
	{
		Application.LoadLevel("Game_Board");
	}
}