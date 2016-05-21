using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_GroundAttack : BaseFSMState
{


		RayCastColliders controller;
		//public Animation anim; 

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.ATTACK;
				//anim = controller.anima;
				controller.FitAnima.Play ("Jab");
				controller.ClearBuffer ();
				controller.ApplyFriction = true;
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
				if (controller.EndAnim == true) {
						controller.EndAnim = false;
						DoTransition(typeof(FitState_AM_Idle));
						return;
				}

		if (controller.BfAction == BufferedAction.ATTACK && controller.FitAnima.enabled) {
			CheckCancel ();
			return;
		}

		if (controller.IASA) {
			CheckIASA ();
		}

		}

		void SeedUp()
		{
				controller.Strike.HitComboSeed += 1;
				if (controller.Strike.HitComboSeed > 100000) 
				{
						controller.Strike.HitComboSeed = 1;
				}
		}

	void CheckCancel()
	{
		switch (controller.Strike.CancelOn) {
		case 1:
			if (controller.Strike.HIT && controller.Strike.CancelWindow) {
				NextAnimGround (controller.Strike.ComboReference);
			}
			break;
		case 3:
			if (controller.Strike.BLOCKED || controller.Strike.HIT) {
				if (controller.Strike.CancelWindow) {
					NextAnimGround (controller.Strike.ComboReference);
				}
			}
			break;
		}
	}

	void NextAnimGround(int AID) 
	{
		controller.FitAnima.Play (controller.Strike.AttackBoxes[AID].animationName);
		controller.ClearBuffer ();
		controller.ApplyFriction = true;
		if (controller.previousState == CharacterState.WAVEDASHLAND) {
			controller.C_Drag = controller.battle.WavedashFriction;
		} else {
			controller.C_Drag = controller.movement.friction;
		}
		SeedUp ();
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
	}

	public void EndTerms() {

		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		return;
	}



}