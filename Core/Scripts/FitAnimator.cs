using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Xft;
using CreativeSpore.SmartColliders;

public class FitAnimator : MonoBehaviour {

	public RayCastColliders controller;
	public SmartRectCollider2D MyContCldr;
	public Transform  lightsource;
	public Transform  lightsource2;
	public Transform  spine;
	public Transform  CorrectScale1;
	public Transform  CorrectScale2;
	public Vector3 spinespin;

	public Vector3 ScaleRight;
	public Vector3 ScaleLeft;
	public Transform ECB_Right;
	public Transform ECB_Left;
	public Quaternion RotateRight;
	public Quaternion RotateRight2;
	public float AnimX;
	public float AnimY;
	public Color ReferenceColor1;

	public SkinnedMeshRenderer[] MyMaterials;
	public bool AnimateMaterials = false;
	public bool AnimateVelocity = false;
	public bool AnimateSpine = false;
	public Color RimColor;
	public float RimSize;
	public Color DefaultRimColor;
	public float DefaultRimSize;

	public int HitStopAnim = 0;
	public int PassThroughTimer = 0;

	public Object[] Effects;
	public Transform FXSpawner1;
	public Transform FXSpawner2;
	public Transform FXSpawner3;

	public bool fading = false;

	#region Special Flags

	public bool Grabbing = false;
	public bool Grazing = false;
	public int InvulTimer = 0;

	public bool CanFastFall = false;
	public bool ContinuousCollision = false;
	public bool CanLand = false;
	public bool NoFallAnim = false;
	public bool ApplyGravity = false;
	public bool CancelWindow = false;

	//Cancel On:
	//1: Hit
	//2: Block
	//3: Hit/Block
	//4: Whiff
	public float CancelOn = 0;
	//Chains Into:
	//1: Jab
	//2: Jab/Dtilt/Ftilt
	//3: Dtilt
	//4: Ftilt
	//5: SpecialsOnly
	//6: Dsmash
	//7: Fsmash
	//8: Jab/Dtilt/Ftilt/Dsmash/Fsmash
	public float HunterChain = 0;


	#endregion


	public XWeaponTrail Trail;

		void Start () {
				RotateRight = lightsource.transform.rotation;
				RotateRight2 = lightsource2.transform.rotation;
				MyMaterials = this.GetComponentsInChildren<SkinnedMeshRenderer>();
		}

		public void LateUpdate()
		{
		if (ContinuousCollision) {
			MyContCldr.enabled = true;
		} else {
			MyContCldr.enabled = false;
		}

				if (AnimateMaterials == true) 
				{
						for (int i = 1; i < MyMaterials.Length; ++i)
						{
								MyMaterials [i].material.SetFloat ("_RimMin", RimSize);
								MyMaterials [i].material.SetColor ("_RimColor", RimColor);
						}
				}

		if (AnimateVelocity == true) 
		{
			controller.velocity.x = AnimX*controller.x_facing;
			controller.velocity.y = AnimY;
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
		if (PassThroughTimer > 0) 
		{
			PassThroughTimer -= 1;
		}

		if (AnimateSpine) {
			spine.eulerAngles = spine.eulerAngles + spinespin;
//			Vector3 newPos = (controller.CurrentTop - controller.PreviousTop);
//			spine.transform.LookAt (spine.transform.position + newPos);

		}

		CorrectScale1.localScale = Vector3.one;
		CorrectScale2.localScale = Vector3.one;

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


	public void MultiComboUp() {
		controller.Strike.HitComboSeed += 1;
		if (controller.Strike.HitComboSeed > 100000) 
		{
			controller.Strike.HitComboSeed = 1;
		}
	}

		public void EndAnim() {
				controller.EndAnim = true;
		}

	public void FadeFall(float t) {
		controller.FitAnima.CrossFade ("Fall", t); 
		fading = true;
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
		controller.velocity.x = x * controller.x_facing;
		}

		public void SetVelocityY(float y) {
				controller.velocity.y = y;
		}

		//For Animating X and Y values through curves
		public void AnimCurves() {
		controller.velocity.x = AnimX*controller.x_facing;
		controller.velocity.y = AnimY*controller.x_facing;
		}

	public void SetComboRef(int AttackID) {
		controller.Strike.ComboReference = AttackID;
	}

//	public void SetHunterChain(int Chain) {
//		controller.Strike.HunterChain = Chain;
//	}

//	public void SetOnHit(int prop) {
//		controller.Strike.CancelOn = prop;
//	}

//	public void SetCancelWindow(int CanCancel) {
//		if (CanCancel == 1) {
//			controller.Strike.CancelWindow = true;
//		} else {
//			controller.Strike.CancelWindow = false;
//		}
//	}

		public void RotateBones() {

		}

	public void SpawnFX1(int fxid) {
		Instantiate (Effects [fxid], FXSpawner1.transform.position, Quaternion.identity);

	}

	public void SpawnFX3Sever(int fxid) {
		GameObject effects = Instantiate (Effects [fxid], FXSpawner3.transform.position, FXSpawner3.transform.rotation) as GameObject;
		if (controller.x_facing == -1) {
			effects.transform.rotation = new Quaternion(effects.transform.rotation.x * -1.0f,
				effects.transform.rotation.y,
				effects.transform.rotation.z,
				effects.transform.rotation.w * -1.0f);

			effects.transform.localScale = new Vector3 (5 * controller.x_facing, 5, 5);
		}
		effects.GetComponent<ParticleGreedSever> ().glowCol = ReferenceColor1;
	}

	public void BulletFX1(int bulletid) {
		GameObject Bullet = Instantiate (Effects [bulletid], FXSpawner1.transform.position, Quaternion.identity) as GameObject;
		ProjCollider Bvar = Bullet.GetComponent<ProjCollider> ();
		Bullet.transform.localScale = new Vector3(controller.x_facing,1,1);
		Bvar.OwnerStrike = controller.Strike;

	}

	public void BulletFX1SeedUp(int Bulletid) {
		MultiComboUp ();
		GameObject Bullet = Instantiate (Effects [Bulletid], FXSpawner1.transform.position, Quaternion.identity) as GameObject;
		ProjCollider Bvar = Bullet.GetComponent<ProjCollider> ();
		Bullet.transform.localScale = new Vector3(controller.x_facing,1,1);
		Bvar.OwnerStrike = controller.Strike;

	}

	public void ReverseDirection() {
		controller.x_facing *= -1;
		CorrectColliders ();
	}



	






}
