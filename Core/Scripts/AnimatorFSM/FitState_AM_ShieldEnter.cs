using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_ShieldEnter : BaseFSMState
{

	RayCastColliders controller;
	public int AnimTimer;
	public bool HoldDown;
	//public Animation anim; 

	public override void Enter()
	{
		FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
		controller = SM.m_GameObject.GetComponent<RayCastColliders>();
		controller.state = CharacterState.SHIELDENTER;
		//anim = controller.anima;
		controller.FitAnima.Play ("ShieldEnter");
		controller.ApplyFriction = true;
		controller.NoFall = true;
		controller.C_Drag = controller.movement.Drag;
		controller.velocity.x *= 0.55f;
		AnimTimer = 0;
		if (controller.Inputter.y <= -0.60f && controller.Inputter.y >= -0.85f) {
			HoldDown = true;
		} else {
			HoldDown = false;
		}
	}

	public override void Exit()
	{
		EndTerms();
	}

	public override void Update()
	{
		//				controller.Inputter.GetInput ();
		//				controller.Inputter.ProcessInput ();

			AnimTimer += 1;

		if (controller.IsGrounded (controller.groundedLookAhead) == false) {
			DoTransition (typeof(FitState_AM_Fall));
			return;
		}

		CheckIASA ();

		if (controller.Inputter.y > -0.60f && AnimTimer >= 2) {
			HoldDown = false;
		}

		if (AnimTimer == 2 && HoldDown) {
			controller.FitAnima.Play ("Shielding");
		}

		if (AnimTimer == 3 && HoldDown) {
			if (controller.Inputter.y <= -0.65f && controller.Inputter.y >= -0.85f && controller.OnPassThrough (controller.groundedLookAhead)) {
				DoTransition (typeof(FitState_AM_Pass));
				return;
			} else {

			}
		}

		if (controller.EndAnim == true || AnimTimer == 8) {
			controller.EndAnim = false;
			DoTransition(typeof(FitState_AM_Shield));
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

		controller.NoFall = false;
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