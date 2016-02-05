using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Run : BaseFSMState
{

		RayCastColliders controller;
		public Animation anim; 
		public int Init_direction;
		public bool DeAccel = false;

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.RUNNING;
				anim = controller.anima;
				anim.Play ("Jab");
				DeAccel = false;
				Init_direction = controller.Inputter.Init_Xdirection;
				controller.x_facing = Init_direction;
				controller.Animator.CorrectColliders ();
				controller.ClearBuffer ();
				controller.ApplyFriction = false;

		}

		public override void Exit()
		{
				controller.previousState = CharacterState.RUNNING;
		}

		public override void Update()
		{

				if (Mathf.Abs (controller.Inputter.x) <= 0.7f && Mathf.Abs (controller.velocity.x) == controller.movement.initDashVelocity) {
						DeAccel = true;
				}

				if (DeAccel == false) {
						controller.velocity.x += controller.movement.acceleration * Init_direction;
						if (Mathf.Abs (controller.velocity.x) >= controller.movement.initDashVelocity) {
								controller.velocity.x = controller.movement.initDashVelocity * Init_direction;
						}
				}


				if (controller.Inputter.x > 0.7f) {
						controller.x_direction = 1;
				} else if (controller.Inputter.x < -0.7f) {
						controller.x_direction = -1;
				}

				if (DeAccel == true) {
						DoTransition (typeof(FitState_AM_RunBrake));
						return;
				}

				if (controller.BfAction == BufferedAction.JUMP) {

						DoTransition (typeof(FitState_AM_JumpSquat));
						return;
				}
				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
						DoTransition (typeof(FitState_AM_Fall));
						return;
				}

				if (Init_direction != controller.x_direction) {
						DoTransition (typeof(FitState_AM_RunTurn));
				}
				if (controller.velocity.x == 0) {

								controller.BfAction = BufferedAction.NONE;
								DoTransition (typeof(FitState_AM_Idle));
								return;

				}


		}


}