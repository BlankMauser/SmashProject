using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Pivot : BaseFSMState
{

		RayCastColliders controller;
		//public Animation anim; 
		public int Init_direction;
		public int PivotTimer;
		public float TurnVel;

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.PIVOT;
				//anim = controller.anima;
				controller.FitAnima.Play ("Idle");
				controller.BfAction = BufferedAction.PIVOT;
				Init_direction = controller.Inputter.Init_Xdirection;
				controller.x_facing = Init_direction;
				controller.Animator.CorrectColliders ();
				TurnVel = Mathf.Abs(controller.velocity.x);
				controller.velocity.x = 0;
				controller.ApplyFriction = true;
				controller.C_Drag = controller.movement.friction;
				PivotTimer = 0;
		}

		public override void Exit()
		{
		EndTerms ();
		}

		public override void Update()
		{

				if (controller.BfAction == BufferedAction.QA) {
					DoTransition (typeof(FitState_AM_Grab));
					return;
				}

				if (controller.BfAction == BufferedAction.ATTACK) {
						DoTransition (typeof(FitState_AM_GroundAttack));
				}

				if (controller.BfAction == BufferedAction.SPECIAL) {
					DoTransition (typeof(FitState_AM_GroundSpecial));
					return;
				}

				if (controller.BfAction == BufferedAction.JUMP) {

						DoTransition (typeof(FitState_AM_JumpSquat));
				}
				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
						DoTransition (typeof(FitState_AM_Fall));
				}

				if (PivotTimer == 0) {
						if (Mathf.Abs (controller.Inputter.x) >= 0.7f) {
						object[] args = new object[1];
						args[0] = TurnVel;
						DoTransition (typeof(FitState_AM_InitDash), args);
										return;
						} else {
								DoTransition (typeof(FitState_AM_Idle));
						}
						//						if (Mathf.Abs(controller.Inputter.x) >= 0.5f) {
						//								DoTransition (typeof(FitState_AM_WalkFast));
						//								return;
						//						}
						//						if (Mathf.Abs(controller.Inputter.x) < 0.5f) {
						//								DoTransition (typeof(FitState_AM_WalkSlow));
						//								return;
						//						}

				} else {
						PivotTimer -= 1;
				}

		}

	public void EndTerms() {

		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		return;
	}


}
