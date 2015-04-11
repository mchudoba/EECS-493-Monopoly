using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	public bool isRefreshing = false;
	public bool foundGames = false;

	private MenuManager menuManager;
	private MenuInteraction menuInteraction;
	private HostData connectedServer;
	private float refreshRequestLength = 3.0f;
	private HostData[] hostData;
	private ServerInfo[] serverInfo;
	private PlayerLobbyInfo[] lobbyInfo;
	private int numServers = 4;
	private int maxPlayers = 4;
	private string gameName = "UMich-EECS493-Monopoly";

	void Awake()
	{
		DontDestroyOnLoad(this);

		menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
		menuInteraction = GameObject.Find("MenuManager").GetComponent<MenuInteraction>();

		serverInfo = new ServerInfo[numServers];
		for (int i = 0; i < numServers; ++i)
			serverInfo[i] = GameObject.Find("ServerInfo " + (i + 1).ToString()).GetComponent<ServerInfo>();

		lobbyInfo = new PlayerLobbyInfo[maxPlayers];
		for (int i = 0; i < maxPlayers; ++i)
			lobbyInfo[i] = GameObject.Find("PlayerInfo" + (i + 1).ToString()).GetComponent<PlayerLobbyInfo>();
	}

	void OnServerInitialized()
	{
		Debug.Log("Server has been initialized");
	}

	void OnMasterServerEvent(MasterServerEvent serverEvent)
	{
		if (serverEvent == MasterServerEvent.RegistrationSucceeded)
		{
			Debug.Log("Registration successful");
			lobbyInfo[0].setPlayerInfo(PlayerPrefs.GetString("Player Name"));
		}
	}

	void OnConnectedToServer()
	{
		int index = connectedServer.connectedPlayers;
		lobbyInfo[index].setPlayerInfo(PlayerPrefs.GetString("Player Name"));
	}

	void OnDisconnectedFromServer()
	{
		if (Application.loadedLevelName == "Game_Board")
			Application.LoadLevel("Main_Menu");

		menuManager.ShowPlayMenu();
		menuManager.ShowPopupMenu();
		menuInteraction.ServerDisconnectPopup();
	}

	void OnApplicationQuit()
	{
		if (Network.isServer)
		{
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}
		if (Network.isClient)
		{
			Network.Disconnect();
		}
	}

	public void StartServer(string serverName)
	{
		Network.InitializeServer(4, 25002, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameName, serverName, "Monopoly");
	}

	public void FindServers()
	{
		if (isRefreshing) return;
		StartCoroutine("RefreshHostList");
	}

	public IEnumerator RefreshHostList()
	{
		isRefreshing = true;

		for (int i = 0; i < serverInfo.Length; ++i)
			serverInfo[i].clearServerData();

		MasterServer.RequestHostList(gameName);
		float timeEnd = Time.time + refreshRequestLength;

		while (Time.time < timeEnd)
		{
			hostData = MasterServer.PollHostList();
			if (hostData != null && hostData.Length > 0)
			{
				for (int i = 0; i < Mathf.Min(hostData.Length, numServers); ++i)
				{
					serverInfo[i].setServerData(hostData[i]);
				}
			}

			yield return new WaitForEndOfFrame();
		}

		if (hostData == null || hostData.Length == 0)
		{
			foundGames = false;
			Debug.Log("No active servers have been found");
		}
		else
		{
			foundGames = true;
			Debug.Log("Active server(s) found");
		}

		isRefreshing = false;
	}

	public void ConnectToServer(HostData serverData)
	{
		Network.Connect(serverData);
		connectedServer = serverData;
	}
}