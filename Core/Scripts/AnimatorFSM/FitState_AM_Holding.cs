using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Holding : BaseFSMState
{


		RayCastColliders controller;
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
				controller.FitAnima.Play ("Holding");
				
		}

		public override void Exit()
		{
		
		}

		public override void Update()
		{


		if (controller.BfAction == BufferedAction.ATTACK || controller.BfAction == BufferedAction.QA) {
			DoTransition (typeof(FitState_AM_Throwing));
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