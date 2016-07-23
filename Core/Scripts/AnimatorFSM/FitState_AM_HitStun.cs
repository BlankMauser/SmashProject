using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_HitStun : BaseFSMState {

		RayCastColliders controller;
		public float Center;
		public int CenterXDir;
		public int HitStunTimer;
		public HitboxData MyHitboxData;
		public HitboxData SentKnockback;
		public bool FromGround = false;
		public double CalcKB;
		//public Animation anim; 

		public FitState_AM_HitStun()
		{
		}

		public FitState_AM_HitStun(HitboxData Sent, double calcKB)
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
				controller.ApplyFriction = true;
				HitStunTimer = (SentKnockback.Hitstun-2);
				controller.C_Drag = controller.movement.friction;
				DICalc ();
				KnockbackCalc ();
		}

		public override void Exit()
		{
		}

		// Update is called once per frame
		public override void Update () {

				if (controller.EndAnim == true) {
						controller.EndAnim = false;
						DoTransition(typeof(FitState_AM_Idle));
						return;
				}

				if (controller.IASA == true) 
				{
						CheckIASA ();
				}



		}

		public override void LateUpdate()
		{

				if (controller.Strike.ApplyHitboxFrame == true) 
				{
						HitboxCollision ();
				}

				if (HitStunTimer == 0) {
						controller.IASA = true;
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

		if (SentKnockback.type == HitboxType.Bullet) {
			Center = SentKnockback.BulletCenter.x;
			CenterXDir = (int)SentKnockback.BulletCenter.z;
		} else {
			Center = SentKnockback.OwnerCollider.CurrentBottom.x;
			CenterXDir = SentKnockback.OwnerCollider.x_facing;
		}

		if (SentKnockback.Reversible == Reversible.Normal) 
		{
			Debug.Log (Mathf.Sign (Center - controller.CurrentBottom.x));
			if (Mathf.Sign (Center - controller.CurrentBottom.x) == Mathf.Sign(CenterXDir)) {
				controller.kbvelocity.x *= (CenterXDir*-1);
			} else {
				controller.kbvelocity.x *= CenterXDir;
			}
		}

		if (SentKnockback.Reversible == Reversible.Forward) {
			controller.kbvelocity.x *= CenterXDir;
		}

		if (SentKnockback.Reversible == Reversible.Reverse) {
			controller.kbvelocity.x *= (CenterXDir*-1);
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

		public void CheckIASA() {

				if (controller.BfAction == BufferedAction.ATTACK) {
						DoTransition (typeof(FitState_AM_GroundAttack));
						return;
				}

				if (controller.BfAction == BufferedAction.JUMP) {

						DoTransition (typeof(FitState_AM_JumpSquat));
						return;
				}
						

				if (controller.BfAction == BufferedAction.INIT_DASH) {
						DoTransition (typeof(FitState_AM_InitDash));
						return;
				}

		}


}
