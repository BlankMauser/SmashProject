using UnityEngine;
using System.Collections;

public class CameraMimic : MonoBehaviour
{
		public Camera myCam;
		public Camera parentCam;

		void Start(){
				myCam = GetComponent<Camera> ();
		}

		void Update()
		{
				myCam.orthographicSize = parentCam.orthographicSize;
			
		}


}