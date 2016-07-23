using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_HitStop : BaseFSMState {

		RayCastColliders controller;
		public int HitStopTimer;
		public HitboxData MyHitboxData;
		public HitboxData SendKnockback;
		public bool FromGround = false;
		public double CalcKB;
		//public Animation anim; 

		public FitState_AM_HitStop()
		{
		}

		public FitState_AM_HitStop(bool Grounded)
		{
				FromGround = Grounded;
		}

		public override void Enter()
		{
				//Check Amount of Knockback
				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				MyHitboxData = controller.Strike.CurrentDmg;
				controller.state = CharacterState.HITSTOP;
				HitStopTimer = MyHitboxData.Hitlag;
				CheckKnockback ();
		if (MyHitboxData.effect != null) {
			EffectSpawn ();
		}

		}

		public override void Exit()
		{
				EndTerms ();
		}

		// Update is called once per frame
		public override void Update () {


		}

		public override void LateUpdate()
		{

				if (controller.Strike.ApplyHitboxFrame == true) 
				{
						HitboxCollision ();
				}

			if (controller.Strike.ApplyProjFrame == true) 
			{
				HitboxCollisionB ();
			}

				if (controller.Animator.HitStopAnim == 0) {
						object[] args = new object[2];
						args[0] = MyHitboxData;
						args[1] = CalcKB;
						if (FromGround == true) {
								DoTransition (typeof(FitState_AM_HitStun), args);
								return;
						} else {
								DoTransition (typeof(FitState_AM_HitStunFly), args);
								return;
						}
				}
		}
				

		public void CheckKnockback() {
		if (MyHitboxData.IsGrab && MyHitboxData.OwnerCollider.GrabOpponent == null) {
			controller.GrabOpponent = MyHitboxData.OwnerCollider;
			MyHitboxData.OwnerCollider.GrabOpponent = controller;
			object[] args2 = new object[1];
			args2[0] = MyHitboxData.OwnerCollider;
			DoTransition (typeof(FitState_AM_Caught), args2);
			return;
		}
				CalcKB = ((( ((controller.Strike.Percent/10) + ((controller.Strike.Percent*MyHitboxData.Damage)/20)) * (200/(controller.battle.Weight+100)) * 1.4 ) + 18) * (MyHitboxData.KnockbackGrowth/100) ) + MyHitboxData.BaseKnockback;
//		#if UNITY_EDITOR
//		Debug.Log (CalcKB);
//		#endif
				//Check if we will stay Grounded
				if (MyHitboxData.Direction >= 180 && MyHitboxData.Direction <= 360) 
				{
			if ((float)CalcKB >= MyHitboxData.AirThreshhold) {
				FromGround = false;
				controller.FitAnima.SetFloat ("GDamageFly1Spd", 32f/MyHitboxData.Hitstun);
				controller.FitAnima.Play ("GDamageFly1", -1, 0f);
			} else {
				controller.FitAnima.Play ("GDamage2", -1, 0f);
			}

								controller.FitAnima.Update (0);
								controller.Animator.HitStopAnim = HitStopTimer;
								MyHitboxData.OwnerCollider.Animator.HitStopAnim = (HitStopTimer-MyHitboxData.HitlagDisparity);
								MyHitboxData.OwnerCollider.Strike.HIT = true;
						
				}
				if (MyHitboxData.Direction < 180 && MyHitboxData.Direction > 0) 
				{
						FromGround = false;
			controller.FitAnima.SetFloat ("GDamageFly1Spd", 32f/MyHitboxData.Hitstun);
						controller.FitAnima.Play ("GDamageFly1", -1, 0f);
						controller.FitAnima.Update (0);
						controller.Animator.HitStopAnim = HitStopTimer;
						MyHitboxData.OwnerCollider.Animator.HitStopAnim = (HitStopTimer-MyHitboxData.HitlagDisparity);
						MyHitboxData.OwnerCollider.Strike.HIT = true;
				}
				if (MyHitboxData.Direction > 360) 
				{
			if (FromGround) {
				if ((float)CalcKB >= MyHitboxData.AirThreshhold) {
					FromGround = false;
					MyHitboxData.Direction = 44;
					controller.FitAnima.SetFloat ("GDamageFly1Spd", 32f / MyHitboxData.Hitstun);
					controller.FitAnima.Play ("GDamageFly1", -1, 0f);
				} else {
					MyHitboxData.Direction = 0;
					controller.FitAnima.Play ("GDamage2", -1, 0f);
				}
				controller.FitAnima.Update (0);
				controller.Animator.HitStopAnim = HitStopTimer;
				MyHitboxData.OwnerCollider.Animator.HitStopAnim = (HitStopTimer - MyHitboxData.HitlagDisparity);
				MyHitboxData.OwnerCollider.Strike.HIT = true;
			} else {
				MyHitboxData.Direction = 44;
				controller.FitAnima.SetFloat ("GDamageFly1Spd", 32f / MyHitboxData.Hitstun);
				controller.FitAnima.Play ("GDamageFly1", -1, 0f);

				controller.FitAnima.Update (0);
				controller.Animator.HitStopAnim = HitStopTimer;
				MyHitboxData.OwnerCollider.Animator.HitStopAnim = (HitStopTimer - MyHitboxData.HitlagDisparity);
				MyHitboxData.OwnerCollider.Strike.HIT = true;
			}
				}

				if (FromGround == true) 
				{
						controller.FitAnima.Play ("GDamage2", -1, 0f);
						controller.FitAnima.Update (0);
						controller.Animator.HitStopAnim = HitStopTimer;
						MyHitboxData.OwnerCollider.Animator.HitStopAnim = (HitStopTimer-MyHitboxData.HitlagDisparity);
						MyHitboxData.OwnerCollider.Strike.HIT = true;
				}
		}

	public void EffectSpawn()
	{

		GameObject hurtFX = (GameObject)GameObject.Instantiate(MyHitboxData.effect, MyHitboxData.effectspawn, Random.rotation);
	}

		public void HitboxCollision() {
				controller.Strike.DamageCalc ();
				MyHitboxData = controller.Strike.CurrentDmg;
				controller.state = CharacterState.HITSTOP;
				HitStopTimer = MyHitboxData.Hitlag;
				CheckKnockback ();
				EffectSpawn ();

		}

		public void HitboxCollisionB() {
			controller.Strike.DamageCalcB ();
			MyHitboxData = controller.Strike.CurrentDmgB;
			controller.state = CharacterState.HITSTOP;
			HitStopTimer = MyHitboxData.Hitlag;
			CheckKnockback ();
			EffectSpawn ();

		}

		public void EndTerms() {

				controller.previousState = controller.state;
				controller.EndAnim = false;
				controller.IASA = false;
				return;
		}


}
