using UnityEngine;
using System.Collections;

public class DisplayFPS : MonoBehaviour
{
		float deltaTime = 0.0f;
//		public SuperSampling_SSAA Cam;

	void Awake()
	{
		//				 Turn off v-sync
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
		Time.captureFramerate = 60;
	}

		void Start()
		{
//				 Turn off v-sync
				QualitySettings.vSyncCount = 0;
				Application.targetFrameRate = 60;
		}

		void Update()
		{

//				deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

//				if (Input.GetButtonDown("Fire2"))
//				{
////						Cam.TakeHighScaledShot(1920,1080,4f,SSAA.SSAAFilter.NearestNeighbor,"/MyImage/screenshot");
//				}
		}

		void OnGUI()
		{
//				int w = Screen.width, h = Screen.height;
//
//				GUIStyle style = new GUIStyle();
//
//				Rect rect = new Rect(0, 0, w, h * 2 / 100);
//				style.alignment = TextAnchor.UpperLeft;
//				style.fontSize = h * 2 / 100;
//				style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
//				float msec = deltaTime * 1000.0f;
//				float fps = 1.0f / deltaTime;
//				string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
//				GUI.Label(rect, text, style);
		}
}