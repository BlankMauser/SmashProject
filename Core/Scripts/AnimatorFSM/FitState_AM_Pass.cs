using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Pass : BaseFSMState
{


	RayCastColliders controller;
	//public Animation anim; 
	public bool JuDccel;
	public float InitVel;
	public bool FastFall;
	public int localXdir;
	public int localXAxl;
	public float AxisVel;

	public override void Enter()
	{

		FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
		controller = SM.m_GameObject.GetComponent<RayCastColliders>();
		controller.state = CharacterState.FALLING;
		controller.C_Drag = controller.jump.AirDrag;
		if (controller.previousState != CharacterState.JUMPING && controller.previousState != CharacterState.AIRJUMP) 
		{
			controller.velocity.x = controller.velocity.x * 0.6f;
		}
		controller.Animator.PassThroughTimer = 10;
		controller.IsPassing = true;
		//anim = controller.anima;
		controller.Animator.AnimateSpine = true;
		controller.FitAnima.Play ("Passing");

		controller.Inputter.FramesYNeutral = 6;

	}

	public override void Exit()
	{
		EndTerms ();
	}

	public override void Update()
	{
		//Check for Fast Fall
		if (controller.Inputter.y <= -0.7f && controller.Inputter.FramesYNeutral <= 5 && controller.velocity.y <= 0) {
			FastFall = true;
		}

		// If stick is neutral, apply friction.
		if (controller.Inputter.x > 0.7f) {
			controller.x_direction = 1;
			controller.ApplyFriction = false;
		} else if (controller.Inputter.x < -0.7f) {
			controller.x_direction = -1;
			controller.ApplyFriction = false;
		} else {
			controller.ApplyFriction = true;
		}

		if (controller.ApplyFriction == false) {
			AxisVel = controller.velocity.x + (controller.jump.AirMobility * controller.x_direction);
			if (controller.velocity.x > 0) {
				localXAxl = 1;
			} else if (controller.velocity.x < 0) {
				localXAxl = -1;
			} else if (controller.velocity.x == 0) {
				localXAxl = controller.x_facing;
			}
			if (Mathf.Abs (AxisVel) > controller.jump.jumpMaxHVelocity) {
				AxisVel = controller.jump.jumpMaxHVelocity * localXAxl;
			}
			if (Mathf.Abs (AxisVel) <= controller.jump.jumpMaxHVelocity) {
				controller.velocity.x = AxisVel;
			}
		}


		//				controller.Inputter.GetInput ();
		//				controller.Inputter.ProcessInput ();

		if (controller.IsGrounded (controller.groundedLookAhead) == false) {
			if (FastFall == false) {
				controller.velocity.y += controller.jump.fallGravity;
				if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
					controller.velocity.y = controller.jump.MaxFallSpeed;
				}
			} else {
				controller.velocity.y = controller.jump.fastFallGravity;
				if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
					controller.velocity.y = controller.jump.fastFallGravity;
				}
			} 
		} else 
		{
			DoTransition(typeof(FitState_AM_Land));
			return;
		}

		if (controller.BfAction == BufferedAction.JUMP) {
			CheckJump ();
			return;
		}

		if (controller.BfAction == BufferedAction.SHIELD)
		{
			if (controller.CanWavedash (controller.jump.AirdashHeight)) 
			{
				DoTransition (typeof(FitState_AM_Wavedash));
				return;
			}
		}

		if (controller.BfAction == BufferedAction.ATTACK)
		{
			object[] args = new object[3];
			args[0] = InitVel;
			args[1] = JuDccel;
			args[2] = FastFall;
			DoTransition(typeof(FitState_AM_AirAttack), args);
			return;
		}

		if (controller.BfAction == BufferedAction.SPECIAL) {
			DoTransition (typeof(FitState_AM_AirSpecial));
			return;
		}

		CheckLedge ();

		if (controller.EndAnim == true) {
			controller.EndAnim = false;
			DoTransition(typeof(FitState_AM_Fall));
			return;
		}

	}

	public void CheckLedge() {
		if (controller.Fledge == true) {
			if (controller.PreviousBottom.y > controller.CurrentBottom.y && controller.Inputter.y >= -0.5f) {
				DoTransition (typeof(FitState_AM_LedgeCatch));
				return;
			}
		}
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

		//				if (controller.velocity.x > 0) {
		//						localXdir = 1;
		//				} else if (controller.velocity.x < 0) {
		//						localXdir = -1;
		//				} else if (controller.velocity.x == 0) {
		//						localXdir = controller.x_facing;
		//				}

		if (controller.x_direction != controller.x_facing) {
			DoTransition (typeof(FitState_AM_AirHopBack));
			return;
		} else {
			DoTransition (typeof(FitState_AM_AirJump));
			return;
		}

	}

	public void EndTerms() {

		controller.previousState = controller.state;
		controller.Animator.AnimateSpine = false;
		controller.Animator.spinespin = Vector3.zero;
		controller.EndAnim = false;
		controller.IASA = false;
		return;
	}

}