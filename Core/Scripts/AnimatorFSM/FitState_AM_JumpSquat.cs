using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_JumpSquat : BaseFSMState
{

		RayCastColliders controller;
		public Animation anim; 
		public int localXdir;

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.PRE_JUMP;
				anim = controller.anima;
				anim.Play ("JumpSq");
				controller.ClearBuffer ();
				controller.ApplyFriction = false;

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

				if (controller.BfAction == BufferedAction.JAB) {
						DoTransition (typeof(FitState_AM_GroundAttack));
						return;
				}

				if (controller.EndAnim == true) {
						controller.EndAnim = false;
						CheckJump ();
				}


		}

		public override void LateUpdate()
		{

		}

		public void CheckJump() {
				if (controller.Inputter.x > 0) {
						controller.x_direction = 1;
				} 
				if (controller.Inputter.x < 0) {
						controller.x_direction = -1;
				}
				if (controller.Inputter.x == 0) {
						controller.x_direction = controller.x_facing;
				}
						

				if (controller.x_direction != controller.x_facing) {
						if (controller.previousState != CharacterState.RUNTURN) {
								controller.velocity.x = controller.velocity.x * 0.05f;
						}
						DoTransition (typeof(FitState_AM_JumpBackward));
						return;
				} else {
						controller.velocity.x = controller.velocity.x * controller.movement.JumpMomentum;
						DoTransition (typeof(FitState_AM_Jump));
						return;
				}

		}

		public void EndTerms() {

				controller.previousState = controller.state;
				controller.EndAnim = false;
				controller.IASA = false;
				return;
		}


}
