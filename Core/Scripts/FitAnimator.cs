using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FitAnimator : MonoBehaviour {

	public RayCastColliders controller;
	public Transform  lightsource;
	public Transform  lightsource2;

	public Vector3 ScaleRight;
	public Vector3 ScaleLeft;
	public Transform ECB_Right;
	public Transform ECB_Left;
	public Quaternion RotateRight;
	public Quaternion RotateRight2;

		void Start () {
				RotateRight = lightsource.transform.rotation;
				RotateRight2 = lightsource2.transform.rotation;
		}

		public void CorrectColliders() {
				if (controller.x_facing == 1 && ECB_Left.position.x > ECB_Right.position.x) {
						lightsource.transform.localRotation = RotateRight;
						lightsource2.transform.localRotation = RotateRight2;
						transform.localScale = ScaleRight;
						controller.SwitchColliders();
				}
				if (controller.x_facing == -1 && ECB_Left.position.x < ECB_Right.position.x) {
						lightsource.transform.localRotation = new Quaternion(RotateRight.x * -1.0f,
								RotateRight.y,
								RotateRight.z,
								RotateRight.w * -1.0f);
						lightsource2.transform.localRotation = new Quaternion(RotateRight2.x * -1.0f,
								RotateRight2.y,
								RotateRight2.z,
								RotateRight2.w * -1.0f);
						transform.localScale = ScaleLeft;
						controller.SwitchColliders();
				}
		}




	






}
