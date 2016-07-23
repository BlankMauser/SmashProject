using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Grab : BaseFSMState
{


		RayCastColliders controller;
		Vector3 LedgeAnchor;
		//public Animation anim; 

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.ATTACK;
				//anim = controller.anima;
				controller.EndAnim = false;
				controller.ClearBuffer ();
				controller.ApplyFriction = true;
				controller.FitAnima.Play ("Grab");
				controller.FitAnima.Update (0);
		if (controller.previousState == CharacterState.WAVEDASHLAND) {
			controller.C_Drag = controller.battle.WavedashFriction;
		} else {
			controller.C_Drag = controller.movement.friction;
		}
		SeedUp ();
				
		}

		public override void Exit()
		{
		EndTerms ();
		}

		public override void Update()
		{

		if (controller.BfAction == BufferedAction.ATTACK) {
			DoTransition (typeof(FitState_AM_GroundAttack));
			return;
		}

		if (controller.EndAnim == true) {
			controller.EndAnim = false;
			DoTransition(typeof(FitState_AM_Idle));
			return;
		}

		//For Testing
		//		controller.transform.position =	controller.LedgeGrabbed.position + controller.LedgeOffset;

		if (controller.IASA == true) {
			CheckIASA ();
		}

		}

	public override void LateUpdate()
	{
		if (controller.GrabOpponent != null) {
			DoTransition(typeof(FitState_AM_Holding));
			return;
		}
	}


	public void CheckIASA() {

		if (controller.BfAction == BufferedAction.JUMP) {

			DoTransition (typeof(FitState_AM_JumpSquat));
			return;
		}

		if (controller.BfAction == BufferedAction.INIT_DASH) {
			DoTransition (typeof(FitState_AM_InitDash));
			return;
		}

		if (controller.BfAction == BufferedAction.SHIELD) {
			DoTransition (typeof(FitState_AM_ShieldEnter));
			return;
		}

		if (controller.BfAction == BufferedAction.SPECIAL) {
			DoTransition (typeof(FitState_AM_GroundSpecial));
			return;
		}

	}

	public void EndTerms() {

		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		controller.Strike.HIT = false;
		controller.Strike.BLOCKED = false;
		return;
	}
		

	void SeedUp()
	{
		controller.Strike.HitComboSeed += 1;
		if (controller.Strike.HitComboSeed > 100000) 
		{
			controller.Strike.HitComboSeed = 1;
		}
	}


}