using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_HitStun : BaseFSMState {

		RayCastColliders controller;
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
				

		public void KnockbackCalc() {
		controller.kbvelocity.x = Mathf.Cos (SentKnockback.Direction*Mathf.Deg2Rad) * (float)CalcKB;
		controller.kbdecay.x = Mathf.Cos (SentKnockback.Direction*Mathf.Deg2Rad);
		}

		public void DICalc() {

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
