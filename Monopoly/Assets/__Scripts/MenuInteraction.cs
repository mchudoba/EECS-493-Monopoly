using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuInteraction : MonoBehaviour
{
	// StartMenu objects
	private InputField playerNameField;
	private Button startGameButton;
	
	// JoinMenu objects
	private GameObject refreshList;
	private Text searchingText;
	private Text noGamesText;
	private Button joinButton;

	// PopupMenu objects
	private Text messageText;
	private Button okayButton;
	private Button confirmButton;
	private Button cancelButton;

	void Awake()
	{
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

	public void LeaveLobbyPopup()
	{
		messageText.text = "Are you sure you want to quit?";
		okayButton.gameObject.SetActive(false);
		confirmButton.gameObject.SetActive(true);
		cancelButton.gameObject.SetActive(true);
	}
}