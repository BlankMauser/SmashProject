using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Xft;

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
	public float AnimX;
	public float AnimY;

	public SkinnedMeshRenderer[] MyMaterials;
	public bool AnimateMaterials = false;
	public Color RimColor;
	public float RimSize;
	public Color DefaultRimColor;
	public float DefaultRimSize;

	public int HitStopAnim = 0;

	public XWeaponTrail Trail;

		void Start () {
				RotateRight = lightsource.transform.rotation;
				RotateRight2 = lightsource2.transform.rotation;
				MyMaterials = this.GetComponentsInChildren<SkinnedMeshRenderer>();
		}

		public void LateUpdate()
		{
				if (AnimateMaterials == true) 
				{
						for (int i = 1; i < MyMaterials.Length; ++i)
						{
								MyMaterials [i].material.SetFloat ("_RimMin", RimSize);
								MyMaterials [i].material.SetColor ("_RimColor", RimColor);
						}
				}


				if (HitStopAnim > 0) 
				{
						controller.FitAnima.enabled = false;
						HitStopAnim -= 1;
				}
				if (HitStopAnim == 0) 
				{
						controller.FitAnima.enabled = true;
				}

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

		public void EndAnim() {
				controller.EndAnim = true;
		}

		public void IASAstart() {
				controller.IASA = true;
		}

		public void TrailPlay() {
				Trail.Activate ();
		}

		public void TrailStop() {
				Trail.Deactivate ();
		}

		public void TrailStopSmooth(float time) {
				Trail.StopSmoothly (time);
		}

		public void SetVelocityX(float x) {
				controller.velocity.x = x;
		}

		public void SetVelocityY(float y) {
				controller.velocity.y = y;
		}

		//For Animating X and Y values through curves
		public void AnimCurves() {
				controller.velocity.x = AnimX;
				controller.velocity.y = AnimY;
		}




	






}
