using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Land : BaseFSMState
{

		RayCastColliders controller;
		//public Animation anim; 

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.LANDING;
				//anim = controller.anima;
				controller.FitAnima.Play ("Land");
				controller.velocity.y = 0;
				controller.kbvelocity.y = 0;
				controller.ClearBuffer ();
				controller.ApplyFriction = true;
				controller.C_Drag = controller.movement.friction;
		}

		public override void Exit()
		{
				EndTerms ();
		}

		public override void Update()
		{
				//				controller.Inputter.GetInput ();
				//				controller.Inputter.ProcessInput ();
				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
						DoTransition (typeof(FitState_AM_Fall));
						return;
				}

				if (controller.IASA == true) 
				{
						CheckIASA ();
				}

				if (controller.EndAnim == true) {
						controller.EndAnim = false;
						DoTransition (typeof(FitState_AM_Idle));
						return;
				}


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


		}

		public void CheckIASA() {

				if (controller.BfAction == BufferedAction.ATTACK) {
						DoTransition (typeof(FitState_AM_GroundAttack));
						return;
				}

				if (controller.BfAction == BufferedAction.SPECIAL) {
					DoTransition (typeof(FitState_AM_GroundSpecial));
					return;
				}

				if (controller.BfAction == BufferedAction.JUMP) {

						DoTransition (typeof(FitState_AM_JumpSquat));
						return;
				}


				if (controller.BfAction == BufferedAction.WALKING) {
						if (Mathf.Abs (controller.Inputter.x) >= 0.5f) {
								DoTransition (typeof(FitState_AM_WalkFast));
								return;
						} else {
								DoTransition (typeof(FitState_AM_WalkSlow));
								return;
						}
				}

				if (controller.BfAction == BufferedAction.INIT_DASH) {
						DoTransition (typeof(FitState_AM_InitDash));
						return;
				}

		}

		public void HitboxCollision() {
			controller.Strike.DamageCalc ();
			object[] args = new object[1];
			args[0] = true;
			DoTransition (typeof(FitState_AM_HitStop), args);
			return;

		}

		public void HitboxCollisionB() {
			controller.Strike.DamageCalcB ();
			object[] args = new object[1];
			args[0] = true;
			DoTransition (typeof(FitState_AM_HitStop), args);
			return;

		}

		public void EndTerms() {

				controller.previousState = controller.state;
				controller.EndAnim = false;
				controller.IASA = false;
				return;
		}


}

