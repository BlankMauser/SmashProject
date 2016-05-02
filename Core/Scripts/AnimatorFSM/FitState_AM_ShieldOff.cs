using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_ShieldOff : BaseFSMState
{

	RayCastColliders controller;
	//public Animation anim; 

	public override void Enter()
	{
		FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
		controller = SM.m_GameObject.GetComponent<RayCastColliders>();
		controller.state = CharacterState.SHIELDEXIT;
		//anim = controller.anima;
		controller.FitAnima.Play ("ShieldExit");
		controller.ApplyFriction = true;
		controller.C_Drag = controller.movement.Drag;
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

		if (controller.Inputter.y <= -0.65f) {
			if (controller.Inputter.FramesYNeutral <= 4 && controller.OnPassThrough (controller.groundedLookAhead)) {
				DoTransition (typeof(FitState_AM_Pass));
				return;
			} else {
				DoTransition (typeof(FitState_AM_Crouch));
				return;
			}
		}

		if (controller.EndAnim) {
			controller.EndAnim = false;
			DoTransition(typeof(FitState_AM_Idle));
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

		if (controller.BfAction == BufferedAction.JUMP) {

			DoTransition (typeof(FitState_AM_JumpSquat));
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
		controller.Strike.ShieldCalc ();
		DoTransition (typeof(FitState_AM_ShieldStop));
		return;

	}


}