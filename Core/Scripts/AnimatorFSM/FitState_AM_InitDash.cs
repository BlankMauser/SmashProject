using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_InitDash : BaseFSMState
{

		RayCastColliders controller;
		//public Animation anim; 
		public int Init_direction;
		public bool DeAccel = false;

		public float PrevVelocity = 0;

		public FitState_AM_InitDash()
		{
		}

	public FitState_AM_InitDash(float smashturn)
		{
		PrevVelocity = smashturn;

		}
		
		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.DASH_START;
				//anim = controller.anima;
				controller.FitAnima.Play ("Dash");
				DeAccel = false;
				if (controller.previousState == CharacterState.PIVOT) {
						Init_direction = controller.x_facing;
				} else {
						Init_direction = controller.Inputter.Init_Xdirection;
				}
				controller.x_direction = Init_direction;
				controller.x_facing = Init_direction;
				controller.Animator.CorrectColliders ();
				controller.ClearBuffer ();
				controller.ApplyFriction = false;
		if (PrevVelocity > 0) {
			controller.velocity.x = (PrevVelocity * 0.5f) * Init_direction;
		}

		}

		public override void Exit()
		{
		EndTerms ();
		}

		public override void Update()
		{

				if (Mathf.Abs (controller.Inputter.x) <= 0.65f && Mathf.Abs (controller.velocity.x) == controller.movement.initDashVelocity) {
						DeAccel = true;
				}

				if (DeAccel == false) {
						controller.velocity.x += controller.movement.acceleration * Init_direction;
						if (Mathf.Abs (controller.velocity.x) >= controller.movement.initDashVelocity) {
								controller.velocity.x = controller.movement.initDashVelocity * Init_direction;
						}
				}
				

				if (controller.Inputter.x > 0.65f) {
						controller.x_direction = 1;
				} else if (controller.Inputter.x < -0.65f) {
						controller.x_direction = -1;
				}

				if (DeAccel == true) {
						float newVelocity = (Mathf.Abs (controller.velocity.x) - controller.movement.Drag);
						if (newVelocity < 0) {
								newVelocity = 0;
						}
						newVelocity = newVelocity * Init_direction;
						controller.velocity.x = newVelocity;
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

				if (controller.BfAction == BufferedAction.SHIELD) {
					DoTransition (typeof(FitState_AM_ShieldEnter));
					return;
				}

				if (controller.BfAction == BufferedAction.SPECIAL) {
					DoTransition (typeof(FitState_AM_GroundSpecial));
					return;
				}

				if (controller.EndAnim == true || controller.velocity.x == 0) {
						if (DeAccel == true) {
								controller.EndAnim = false;
								DoTransition (typeof(FitState_AM_Idle));
								return;
						}
						if (DeAccel == false) {
								controller.EndAnim = false;
								DoTransition (typeof(FitState_AM_Run));
								return;
						}
				}
						

		}

	public void EndTerms() {

		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		return;
	}




}