using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Idle : BaseFSMState
{
		
		RayCastColliders controller;
		//public Animation anim; 

		public override void Enter()
		{
				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.IDLE;
				//anim = controller.anima;
				controller.FitAnima.Play ("Idle");
				controller.ApplyFriction = true;
				controller.C_Drag = controller.movement.friction;
		}

		public override void Exit()
		{
				EndTerms();
		}

		public override void Update()
		{
//				controller.Inputter.GetInput ();
//				controller.Inputter.ProcessInput ();

				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
						DoTransition (typeof(FitState_AM_Fall));
						return;
				}

				CheckIASA ();
//				if (controller.BfAction == BufferedAction.JAB) {
//								DoTransition (typeof(FitState_AM_GroundAttack));
//								return;
//						}
//
//						if (controller.BfAction == BufferedAction.JUMP) {
//
//								DoTransition (typeof(FitState_AM_JumpSquat));
//								return;
//						}
//
//
//				if (controller.BfAction == BufferedAction.WALKING) {
//						if (Mathf.Abs (controller.Inputter.x) >= 0.5f) {
//								DoTransition (typeof(FitState_AM_WalkFast));
//								return;
//						} else {
//								DoTransition (typeof(FitState_AM_WalkSlow));
//								return;
//						}
//				}
//
//				if (controller.BfAction == BufferedAction.INIT_DASH) {
//						DoTransition (typeof(FitState_AM_InitDash));
//						return;
//				}

				if (controller.Inputter.y <= -0.9f) {
						DoTransition (typeof(FitState_AM_Crouch));
						return;
				}



		}

		public override void LateUpdate()
		{

				if (controller.Strike.ApplyHitboxFrame == true) 
				{
						Debug.Log ("Got Here");
						HitboxCollision ();
				}
		}

		public void CheckIASA() {
				
				if (controller.BfAction == BufferedAction.JAB) {
						DoTransition (typeof(FitState_AM_GroundAttack));
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

		public void EndTerms() {

				controller.previousState = controller.state;
				controller.EndAnim = false;
				controller.IASA = false;
				return;
		}

		public void HitboxCollision() {
				controller.Strike.DamageCalc ();
				object[] args = new object[1];
				args[0] = true;
				DoTransition (typeof(FitState_AM_HitStop), args);
				return;

		}


}