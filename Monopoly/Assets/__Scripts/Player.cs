using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public string playerName;
	public int money = 0;
	public int numPropertyOwned = 0;

	void Awake()
	{
		DontDestroyOnLoad(this);
	}
}