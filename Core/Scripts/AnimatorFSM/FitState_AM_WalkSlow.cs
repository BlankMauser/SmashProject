using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_WalkSlow : BaseFSMState
{

		RayCastColliders controller;
		//public Animation anim; 
		public int Init_direction;

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.WALKING;
				//anim = controller.anima;
				controller.FitAnima.Play ("Walk");
				Init_direction = controller.Inputter.Init_Xdirection;
				controller.x_facing = Init_direction;
				controller.Animator.CorrectColliders ();
				controller.ClearBuffer ();
				controller.ApplyFriction = false;

		}

		public override void Exit()
		{
				controller.previousState = CharacterState.WALKING;
		}

		public override void Update()
		{

				controller.velocity.x = controller.movement.WalkSpeed * Init_direction;

				if (controller.Inputter.x > 0.05f) {
						controller.x_direction = 1;
				} else if (controller.Inputter.x < -0.05f) {
						controller.x_direction = -1;
				}

				if (controller.BfAction == BufferedAction.ATTACK) {
						controller.velocity.x = 0;
						DoTransition (typeof(FitState_AM_GroundAttack));
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
						DoTransition (typeof(FitState_AM_Pivot));
				}
				if (Mathf.Abs(controller.Inputter.x) >= 0.7f && controller.Inputter.FramesXNeutral <= 2) {
								DoTransition (typeof(FitState_AM_InitDash));
								return;
				}
				if (Mathf.Abs(controller.Inputter.x) >= 0.5f) {
						DoTransition (typeof(FitState_AM_WalkFast));
						return;
				}


		if (controller.BfAction == BufferedAction.SHIELD) {
			DoTransition (typeof(FitState_AM_ShieldEnter));
			return;
		}

				if (Mathf.Abs(controller.Inputter.x) <= 0.05f) {
						controller.ClearBuffer ();
						DoTransition (typeof(FitState_AM_Idle));
						return;
				}

		if (controller.Inputter.y <= -0.65f) {
			if (controller.Inputter.FramesYNeutral <= 5 && controller.OnPassThrough (controller.groundedLookAhead)) {
				DoTransition (typeof(FitState_AM_Pass));
				return;
			} else {
				DoTransition (typeof(FitState_AM_Crouch));
				return;
			}
		}

		}


}
