using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_HitStunFly : BaseFSMState {

		RayCastColliders controller;
		public int HitStunTimer;
		public HitboxData MyHitboxData;
		public HitboxData SentKnockback;
		public bool FromGround = false;
		public double CalcKB;
		//public Animation anim; 

		public FitState_AM_HitStunFly()
		{
		}

		public FitState_AM_HitStunFly(HitboxData Sent, double calcKB)
		{
				SentKnockback = Sent;
				CalcKB = calcKB;

		}

		public override void Enter()
		{
				//Check Amount of Knockback
				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.STUNNED;
				controller.ApplyFriction = false;
				HitStunTimer = (SentKnockback.Hitstun-2);
				DICalc ();
				KnockbackCalc ();
		}

		public override void Exit()
		{
		}

		// Update is called once per frame
		public override void Update () {

//				if (controller.EndAnim == true) {
//						controller.EndAnim = false;
//						DoTransition (typeof(FitState_AM_Tumble));
//						return;
//				}

				if (controller.IASA == true) 
				{
						CheckIASA ();
				}

		if (controller.IsGrounded (controller.groundedLookAhead) == false) {

			controller.velocity.y += controller.jump.fallGravity;
			if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
				controller.velocity.y = controller.jump.MaxFallSpeed;
			}
            

		} else {
			if (controller.PreviousBottom.y >= controller.CurrentBottom.y) {
			controller.kbvelocity = Vector3.zero;
			if (controller.Inputter.FramesLPressed <= 10) { 
				if (Mathf.Abs (controller.Inputter.x) >= 0.7f) {
					if (Mathf.Sign (controller.Inputter.x) == controller.x_facing) {
						Teching (1);
						return;
					} else {
						Teching (2);
						return;
					}
				} else {
					Teching (3);
					return;
				}
				


			} else {
				
				DoTransition (typeof(FitState_AM_Land));
				return;
				
				}
			}
		}

		//Teching
		// 1=RollF 2=RollB 3=TechNGround
//		if (controller.Inputter.ShieldButtonDown) {
//			if (controller.Inputter.FramesLPressed <= 10) {
//				if (controller.CanTech((Vector3.up * -1),Vector2.Distance(controller.CurrentLeft,controller.CurrentBottom),true))
//					{
//						if (Mathf.Abs(controller.Inputter.x) >= 0.7f)
//						{
//							if (Mathf.Sign(controller.Inputter.x) == controller.x_facing)
//							{
//								Teching(1);
//								return;
//							} else
//							{
//								Teching(2);
//								return;
//							}
//						} else
//						{
//							Teching(3);
//							return;
//						}
//					}
//
//
//			}
//
//		}


    }

		public override void LateUpdate()
		{

				if (controller.Strike.ApplyHitboxFrame == true) 
				{
						HitboxCollision ();
				}

				if (HitStunTimer == 0) {
				controller.EndAnim = false;
				DoTransition (typeof(FitState_AM_Tumble));
				return;
				} else {
						HitStunTimer -= 1;
				}


		}


		public void KnockbackCalc() 
		{
		controller.kbvelocity.x = Mathf.Cos (SentKnockback.Direction*Mathf.Deg2Rad) * (float)CalcKB;
		controller.kbdecay.x = Mathf.Cos (SentKnockback.Direction*Mathf.Deg2Rad) * 2f;
		controller.kbvelocity.y = Mathf.Sin (SentKnockback.Direction*Mathf.Deg2Rad) * (float)CalcKB;
		controller.kbdecay.y = Mathf.Sin (SentKnockback.Direction*Mathf.Deg2Rad) * 2f;

		if (SentKnockback.Reversible == Reversible.Normal) 
			{
			if (Mathf.Sign (SentKnockback.OwnerCollider.CurrentBottom.x - controller.CurrentBottom.x) == Mathf.Sign(SentKnockback.OwnerCollider.x_facing)) {
				Debug.Log (Mathf.Sign (SentKnockback.OwnerCollider.CurrentBottom.x - controller.CurrentBottom.x));
				controller.kbvelocity.x *= (SentKnockback.OwnerCollider.x_facing*-1);
			} else {
				controller.kbvelocity.x *= SentKnockback.OwnerCollider.x_facing;
			}
			}

		if (SentKnockback.Reversible == Reversible.Forward) {
			controller.kbvelocity.x *= SentKnockback.OwnerCollider.x_facing;
		}

		if (SentKnockback.Reversible == Reversible.Reverse) {
			controller.kbvelocity.x *= (SentKnockback.OwnerCollider.x_facing*-1);
		}

		}

		public void DICalc() {
		Cardinals Ang = controller.Inputter.ReturnAxis();
		Cardinals OptimalP = Circular((int)SentKnockback.OptimalDI + 2);
		Cardinals HalfP = Circular((int)SentKnockback.OptimalDI + 1);
		Cardinals OptimalN = Circular((int)SentKnockback.OptimalDI - 2);
		Cardinals HalfN = Circular((int)SentKnockback.OptimalDI - 1);
		if (Ang != OptimalP && Ang != HalfP && Ang != OptimalN && Ang != HalfN) {
			return;
		} else {
			if (Ang == OptimalP) {
				SentKnockback.Direction += 18;
				if (SentKnockback.Direction > 360) {
					SentKnockback.Direction -= 360;
					Debug.Log (OptimalP);
					}
				return;
				}
			if (Ang == HalfP) {
				SentKnockback.Direction += 9;
				if (SentKnockback.Direction > 360) {
					SentKnockback.Direction -= 360;
				}
				return;
			}
			if (Ang == HalfN) {
				SentKnockback.Direction -= 9;
				if (SentKnockback.Direction < 0) {
					SentKnockback.Direction += 360;
				}
				return;
			}
			if (Ang == OptimalN) {
				SentKnockback.Direction -= 18;
				if (SentKnockback.Direction < 0) {
					SentKnockback.Direction += 360;
				}
				return;
			}
		}
	}

	public Cardinals Circular(int input){
		if (input > 7) {
			input -= 8;
		}
		return (Cardinals)input;
		}

		public void HitboxCollision() {
				controller.Strike.DamageCalc ();
				object[] args = new object[1];
				args[0] = true;
				DoTransition (typeof(FitState_AM_HitStop), args);
				return;

		}

			public void Teching(int animnumber) {
			object[] args = new object[1];
			args[0] = animnumber;
			DoTransition (typeof(FitState_AM_Ukemi), args);
			return;

		}

		public void CheckIASA() {





		}


}
