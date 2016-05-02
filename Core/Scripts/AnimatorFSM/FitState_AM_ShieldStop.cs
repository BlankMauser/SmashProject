using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_ShieldStop : BaseFSMState {

	RayCastColliders controller;
	public int HitStopTimer;
	public HitboxData MyHitboxData;
	public HitboxData SendKnockback;
	public double CalcKB;
	//public Animation anim; 

	public FitState_AM_ShieldStop()
	{
	}
		

	public override void Enter()
	{
		//Check Amount of Knockback
		FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
		controller = SM.m_GameObject.GetComponent<RayCastColliders>();
		MyHitboxData = controller.Strike.CurrentDmg;
		controller.state = CharacterState.SHIELDSTOP;
		controller.FitAnima.Play ("Shielding");
		HitStopTimer = MyHitboxData.Hitlag;
		ApplyShield ();
	}

	public override void Exit()
	{
		EndTerms ();
	}

	// Update is called once per frame
	public override void Update () {


	}

	public override void LateUpdate()
	{

		if (controller.Strike.ApplyHitboxFrame == true) 
		{
			HitboxCollision ();
		}

		if (controller.Animator.HitStopAnim == 0) {
			object[] args = new object[1];
			args[0] = MyHitboxData;
			DoTransition (typeof(FitState_AM_ShieldStun), args);
			return;
		}
	}


	public void ApplyShield() {
		controller.Animator.HitStopAnim = HitStopTimer;
		MyHitboxData.OwnerCollider.Animator.HitStopAnim = (HitStopTimer-1);
		MyHitboxData.OwnerCollider.Strike.BLOCKED = true;
	}

	public void HitboxCollision() {
		controller.Strike.ShieldCalc ();
		MyHitboxData = controller.Strike.CurrentDmg;
		controller.state = CharacterState.SHIELDSTOP;
		HitStopTimer = MyHitboxData.Hitlag;
		ApplyShield ();

	}

	public void EndTerms() {

		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		return;
	}


}
