using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	public bool isRefreshing = false;
	public float refreshRequestLength = 3.0f;
	public HostData[] hostData;

	private string gameName = "UMich-EECS493-Monopoly";

	void OnServerInitialized()
	{
		Debug.Log("Server has been initialized");
	}

	void OnMasterServerEvent(MasterServerEvent serverEvent)
	{
		if (serverEvent == MasterServerEvent.RegistrationSucceeded)
			Debug.Log("Registration successful");
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
		Debug.Log("Refreshing...");
		MasterServer.RequestHostList(gameName);
		float timeEnd = Time.time + refreshRequestLength;

		while (Time.time < timeEnd)
		{
			hostData = MasterServer.PollHostList();
			yield return new WaitForEndOfFrame();
		}

		if (hostData == null || hostData.Length == 0)
			Debug.Log("No active servers have been found");
		else
			Debug.Log("Active server(s) found");

		isRefreshing = false;
	}
}