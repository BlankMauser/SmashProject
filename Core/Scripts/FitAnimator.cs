using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FitAnimator : MonoBehaviour {

	public RayCastColliders controller;

	public Vector3 ScaleRight;
	public Vector3 ScaleLeft;
	public Transform ECB_Right;
	public Transform ECB_Left;


		public void CorrectColliders() {
				if (controller.x_facing == 1 && ECB_Left.position.x > ECB_Right.position.x) {
						transform.localScale = ScaleRight;
						controller.SwitchColliders();
				}
				if (controller.x_facing == -1 && ECB_Left.position.x < ECB_Right.position.x) {
						transform.localScale = ScaleLeft;
						controller.SwitchColliders();
				}
		}




	






}
