using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLobbyInfo : MonoBehaviour
{
	public Text nameText;

	private NetworkView netView;
	private float textTransparent = 0.5f;
	private float textNormal = 1f;

	void Awake()
	{
		netView = GetComponent<NetworkView>();

		nameText.text = "<Empty>";
		Color temp = nameText.color;
		temp.a = textTransparent;
		nameText.color = temp;
	}

	public void setPlayerInfo(string _name)
	{
		netView.RPC("changeText", RPCMode.AllBuffered, _name);
	}

	[RPC]
	void changeText(string _name)
	{
		nameText.text = _name;
		Color temp = nameText.color;
		temp.a = textNormal;
		nameText.color = temp;
	}
}