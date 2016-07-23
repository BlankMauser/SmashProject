using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Caught : BaseFSMState
{


		RayCastColliders controller;
		//public Animation anim; 
	public Transform GrabPos;
	public Vector3 TrueOffset;
	public int OppFacing;

	public FitState_AM_Caught()
	{
	}

	public FitState_AM_Caught(RayCastColliders GrabOpp)
	{
		GrabPos = GrabOpp.GrabTransform;
		OppFacing = GrabOpp.x_facing;
	}

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.CAUGHT;
				//anim = controller.anima;
				controller.EndAnim = false;
				controller.ClearBuffer ();
				controller.ApplyFriction = false;
				controller.velocity = Vector3.zero;
				controller.kbvelocity = Vector3.zero;
				controller.x_facing = OppFacing * -1;
				controller.Animator.CorrectColliders ();
				controller.FitAnima.Play ("Caught");
				TrueOffset = new Vector3 (controller.GrabbedOffset.x * controller.x_facing, controller.GrabbedOffset.y, controller.GrabbedOffset.z);

				
		}

		public override void Exit()
		{
		
		}

		public override void Update()
		{

		controller.transform.position =	GrabPos.position + TrueOffset;

		if (controller.IASA == true) {
			CheckIASA ();
		}

		}

	public override void LateUpdate()
	{

		if (controller.Strike.ApplyHitboxFrame == true && controller.Strike.CurrentDmg.type == HitboxType.Throw && controller.Strike.CurrentDmg.OwnerCollider == controller.GrabOpponent) 
		{
			ThrowCollision ();
		}


	}

	public void ThrowCollision() {
		controller.Strike.DamageCalc ();
		object[] args = new object[1];
		args[0] = true;
		DoTransition (typeof(FitState_AM_Thrown), args);
		return;

	}


	public void CheckIASA() {


		if (controller.BfAction == BufferedAction.JUMP) {
			CheckJump ();
			return;
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


}