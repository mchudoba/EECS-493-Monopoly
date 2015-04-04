using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextDebug : MonoBehaviour
{
	private Text pos;
	private GameObject cam;

	void Start()
	{
		pos = this.GetComponent<Text>();
		cam = GameObject.Find("Camera");
	}

	void Update()
	{
		float x = cam.transform.position.x;
		float y = cam.transform.position.y;
		pos.text = "Camera X: " + x.ToString("F2") + "\nCamera Y: " + y.ToString("F2");
	}

	public void RestartLevel()
	{
		Application.LoadLevel("Game_Board");
	}
}