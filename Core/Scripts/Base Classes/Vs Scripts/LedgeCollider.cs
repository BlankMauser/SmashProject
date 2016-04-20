using UnityEngine;
using System.Collections;

public class LedgeCollider : MonoBehaviour {

	public bool front;
	public RayCastColliders controller;

		public void Start()
		{
		controller = GetComponentInParent<RayCastColliders> ();
		}

		void OnTriggerStay(Collider c) {
		if (front == true) {
			controller.Fledge = true;
		} else {
			controller.Bledge = true;
		}

		if (controller.LedgeGrabbed == null) {
			controller.LedgeGrabbed = c.transform;
		}
		}

	void OnTriggerExit(Collider c) {
		if (front == true) {
			controller.Fledge = false;
		} else {
			controller.Bledge = false;
		}

		if (controller.LedgeGrabbed == c.transform) {
			controller.LedgeGrabbed = null;
		}
	}	


			
}