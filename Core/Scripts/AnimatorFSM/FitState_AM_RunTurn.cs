using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_RunTurn : BaseFSMState
{

		RayCastColliders controller;
		public Animation anim; 
		public int Init_direction;
		public int BrakeTimer;

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.RUNTURN;
				anim = controller.anima;
				anim.Play ("Turn");
				controller.BfAction = BufferedAction.PIVOT;
				Init_direction = controller.Inputter.Init_Xdirection;
				controller.ApplyFriction = false;
				BrakeTimer = 4;
				controller.C_Drag = controller.movement.friction;
		}

		public override void Exit()
		{
				controller.x_facing = Init_direction;
				controller.Animator.CorrectColliders ();
				controller.previousState = CharacterState.RUNTURN;
		}

		public override void Update()
		{


				if (controller.BfAction == BufferedAction.JUMP) {

						DoTransition (typeof(FitState_AM_JumpSquat));
				}
				if (controller.IsGrounded (controller.groundedLookAhead) == false) {

						DoTransition (typeof(FitState_AM_Fall));
				}

				if (!anim.isPlaying) {
						if (Mathf.Abs (controller.Inputter.x_prev) >= 0.18f) {
								if (Mathf.Abs(controller.Inputter.x) >= 0.7f) {

										DoTransition (typeof(FitState_AM_Run));
										return;
								}
								if (Mathf.Abs(controller.Inputter.x) >= 0.5f) {

										DoTransition (typeof(FitState_AM_WalkFast));
										return;
								}
								if (Mathf.Abs(controller.Inputter.x) < 0.5f) {

										DoTransition (typeof(FitState_AM_WalkSlow));
										return;
								}
						} else {
								DoTransition (typeof(FitState_AM_Idle));
						}


				}

				if (BrakeTimer == 0) {
						controller.ApplyFriction = true;
				} else {
						BrakeTimer -= 1;
				}

		}


}

