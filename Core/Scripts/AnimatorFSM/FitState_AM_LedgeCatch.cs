using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_LedgeCatch : BaseFSMState
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
				controller.FitAnima.Play ("LedgeCatch");
				controller.ClearBuffer ();
				controller.ApplyFriction = false;
				controller.velocity = Vector3.zero;
				controller.kbvelocity = Vector3.zero;
				controller.x_facing = (int)Mathf.Sign (controller.LedgeGrabbed.position.x - controller.CurrentBottom.x);
				LedgeAnchor = new Vector3 (controller.LedgeOffset.x * controller.x_facing, controller.LedgeOffset.y);
				controller.transform.position =	controller.LedgeGrabbed.position + LedgeAnchor;
				
		}

		public override void Exit()
		{
		
		}

		public override void Update()
		{



				if (controller.EndAnim == true) {
			controller.FitAnima.Play ("LedgeHang");
				}

		//For Testing
		//		controller.transform.position =	controller.LedgeGrabbed.position + controller.LedgeOffset;

		if (controller.IASA == true) {
			CheckIASA ();
		}

		}

	public override void LateUpdate()
	{
		controller.transform.position =	controller.LedgeGrabbed.position + LedgeAnchor;
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