using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_ShieldStun : BaseFSMState {

	RayCastColliders controller;
	public int BlockStunTimer;
	public HitboxData SentKnockback;
	//public Animation anim; 

	public FitState_AM_ShieldStun()
	{
	}

	public FitState_AM_ShieldStun(HitboxData Sent)
	{
		SentKnockback = Sent;

	}

	public override void Enter()
	{
		//Check Amount of Knockback
		FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
		controller = SM.m_GameObject.GetComponent<RayCastColliders>();
		controller.state = CharacterState.SHIELDSTUN;
		controller.ApplyFriction = true;
		BlockStunTimer = (SentKnockback.Blockstun-2);
		controller.FitAnima.Play ("Shielding");
		controller.C_Drag = 1.2f;
		KnockbackCalc ();
	}

	public override void Exit()
	{
	}

	// Update is called once per frame
	public override void Update () {


		if (controller.IASA == true) 
		{
			CheckIASA ();
			if (controller.Inputter.ShieldButtonHeld) {
				DoTransition (typeof(FitState_AM_Shield));
				return;
			} else {
				DoTransition (typeof(FitState_AM_ShieldOff));
			}
		}



	}

	public override void LateUpdate()
	{

		if (controller.Strike.ApplyHitboxFrame == true) 
		{
			HitboxCollision ();
		}

		if (BlockStunTimer == 0) {
			controller.IASA = true;
		} else {
			BlockStunTimer -= 1;
		}


	}


	public void KnockbackCalc() {
		float Xrel = Mathf.Sign (controller.CurrentBottom.x - SentKnockback.OwnerCollider.CurrentBottom.x);
		controller.velocity.x = SentKnockback.ShieldPush * Xrel;
	}

	public void DICalc() {

	}

	public void HitboxCollision() {
		controller.Strike.ShieldCalc ();
		DoTransition (typeof(FitState_AM_ShieldStop));
		return;

	}

	public void CheckIASA() {


		if (controller.BfAction == BufferedAction.JUMP) {

			DoTransition (typeof(FitState_AM_JumpSquat));
			return;
		}



	}


}
