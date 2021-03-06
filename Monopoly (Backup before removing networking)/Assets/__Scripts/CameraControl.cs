﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraControl : MonoBehaviour
{
	public float moveSensitivityX = 1.0f;
	public float moveSensitivityY = 1.0f;
	public bool updateZoomSensitivity = true;
	public float zoomSensitivityFactor = 5f;
	public float zoomSpeed = 0.05f;
	public float minZoom = 2.0f;
	public float maxZoom = 5.5f;
	public bool invertMoveX = false;
	public bool invertMoveY = false;
	public float mapWidth = 40.0f;
	public float mapHeight = 40.0f;
	public float inertiaDuration = 1f;
	public float minimumScrollVelocity = 100f;
	
	private Camera cam;
	private float horizontalExtent, verticalExtent;
	private float minX, maxX, minY, maxY;
	private float scrollVelocity = 0f;
	private float timeTouchPhaseEnded;
	private Vector2 scrollDirection = Vector2.zero;

	void Start()
	{
		cam = GetComponent<Camera>();

		maxZoom = 0.5f * (mapWidth / cam.aspect) - 1f;
		if (mapWidth > mapHeight)
			maxZoom = 0.5f * mapHeight;

		CalculateMapBounds();
	}
	
	void Update()
	{
		if(float.IsNaN(transform.position.x) || float.IsNaN(transform.position.y) || float.IsNaN(transform.position.z)){
			gameObject.transform.position = new Vector3(0,0,-10);
		}

		if (SidePanelOpener.sidePanelOpen) return;

		if (updateZoomSensitivity)
		{
			moveSensitivityX = cam.orthographicSize / zoomSensitivityFactor;
			moveSensitivityY = cam.orthographicSize / zoomSensitivityFactor;
		}

		Touch[] touches = Input.touches;

		if (touches.Length < 1)
		{
			if (scrollVelocity != 0f)
			{
				float timer = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
				float frameVelocity = Mathf.Lerp(scrollVelocity, 0f, timer);
				cam.transform.position += -(Vector3)scrollDirection.normalized * (frameVelocity * 0.005f) * Time.deltaTime;

				if (timer >= 1f)
					scrollVelocity = 0f;
			}
		}

		if (touches.Length > 0)
		{
			// Single touch (move)
			if (touches.Length == 1)
			{
				if (touches[0].phase == TouchPhase.Began)
				{
					scrollVelocity = 0f;
				}
				else if (touches[0].phase == TouchPhase.Moved)
				{
					Vector2 delta = touches[0].deltaPosition;

					float positionX = delta.x * moveSensitivityX * Time.deltaTime;
					positionX = invertMoveX ? positionX : positionX * -1f;

					float positionY = delta.y * moveSensitivityY * Time.deltaTime;
					positionY = invertMoveY ? positionY : positionY * -1f;

					cam.transform.position += new Vector3(positionX, positionY, 0f);

					scrollDirection = touches[0].deltaPosition.normalized;
					scrollVelocity = touches[0].deltaPosition.magnitude / touches[0].deltaTime;

					if (scrollVelocity <= minimumScrollVelocity)
						scrollVelocity = 0f;
				}
				else if (touches[0].phase == TouchPhase.Ended)
				{
					timeTouchPhaseEnded = Time.time;
				}
			}

			// Double touch (zoom)
			if (touches.Length == 2)
			{
				Vector2 cameraViewsize = new Vector2(cam.pixelWidth, cam.pixelHeight);

				Touch touchOne = touches[0];
				Touch touchTwo = touches[1];

				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
				Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;

				float prevTouchDeltaMag = (touchOnePrevPos - touchTwoPrevPos).magnitude;
				float touchDeltaMag = (touchOne.position - touchTwo.position).magnitude;

				float deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;

				cam.transform.position += cam.transform.TransformDirection((
					touchOnePrevPos + touchTwoPrevPos - cameraViewsize) *
					cam.orthographicSize / cameraViewsize.y);

				cam.orthographicSize += deltaMagDiff * zoomSpeed;
				cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom) - 0.001f;

				cam.transform.position -= cam.transform.TransformDirection((
					touchOne.position + touchTwo.position - cameraViewsize) *
					cam.orthographicSize / cameraViewsize.y);

				CalculateMapBounds();
			}
		}
	}

	void LateUpdate()
	{
		if (SidePanelOpener.sidePanelOpen) return;

		Vector3 limitedCameraPosition = cam.transform.position;
		limitedCameraPosition.x = Mathf.Clamp(limitedCameraPosition.x, minX, maxX);
		limitedCameraPosition.y = Mathf.Clamp(limitedCameraPosition.y, minY, maxY);

		cam.transform.position = limitedCameraPosition;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(Vector3.zero, new Vector3(mapWidth, mapHeight, 0f));
	}

	void CalculateMapBounds()
	{
		verticalExtent = cam.orthographicSize;
		horizontalExtent = verticalExtent * cam.aspect;

		minX = horizontalExtent - mapWidth / 2.0f;
		maxX = mapWidth / 2.0f - horizontalExtent;
		minY = verticalExtent - mapHeight / 2.0f;
		maxY = mapHeight / 2.0f - verticalExtent;
	}
}
