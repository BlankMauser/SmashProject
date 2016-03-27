using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_RunBrake : BaseFSMState
{

		RayCastColliders controller;
		//public Animation anim; 
		public int BrakeTimer;

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.RUNBRAKE;
				//anim = controller.anima;
				controller.FitAnima.Play ("Brake");
				controller.ClearBuffer ();
				BrakeTimer = 2;
				controller.C_Drag = controller.movement.friction;
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
				//				controller.Inputter.GetInput ();
				//				controller.Inputter.ProcessInput ();
				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
						DoTransition (typeof(FitState_AM_Fall));
						return;
				}

				if (controller.EndAnim == true) {
						controller.EndAnim = false;
						DoTransition (typeof(FitState_AM_Idle));
						return;
				}

				if (controller.Inputter.x > 0.7f) {
						controller.x_direction = 1;
				} else if (controller.Inputter.x < -0.7f) {
						controller.x_direction = -1;
				}

				if (controller.x_facing != controller.x_direction) {
						DoTransition (typeof(FitState_AM_RunTurn));
				}

				if (BrakeTimer == 0) {
						controller.IASA = true;
						controller.ApplyFriction = true;
				} else {
						BrakeTimer -= 1;
				}

				if (controller.Inputter.y <= -0.9f) {
						DoTransition (typeof(FitState_AM_Crouch));
						return;
				}

		}


}
