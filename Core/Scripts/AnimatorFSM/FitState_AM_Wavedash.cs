using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Wavedash : BaseFSMState
{

		RayCastColliders controller;
		//public Animation anim; 
		public int IASA_Timer;

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.WAVEDASH;
				//anim = controller.anima;
				controller.ApplyFriction = true;
				controller.IASA = false;
				controller.C_Drag = controller.battle.WavedashFriction;
				controller.velocity.x = controller.jump.WavedashVelocity * controller.Inputter.buffer_x;
				if (controller.velocity.y > 0) 
				{
						controller.velocity.y *= 0.08f;
				}
				IASA_Timer = 10;

		}

		public override void Exit()
		{
				controller.previousState = CharacterState.WAVEDASH;
		}

		public override void Update()
		{
				if (IASA_Timer == 0) {
						controller.IASA = true;
				} else 
				{
						IASA_Timer -= 1;
				}

		if (controller.IASA) {
			CheckIASAAir ();
		}

				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
						controller.velocity.y += controller.jump.fallGravity;
						if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
								controller.velocity.y = controller.jump.MaxFallSpeed;
						}
								
				} else 
				{
						//						if (controller.PreviousBottom.y >= controller.CurrentBottom.y) {
						object[] args = new object[1];
						args[0] = IASA_Timer;
						DoTransition(typeof(FitState_AM_WavedashLand), args);
						return;
						//						}

				}




		}

	public void CheckIASAAir() {

//		if (controller.BfAction == BufferedAction.ATTACK) {
//			controller.EndAnim = false;
//			controller.IASA = false;
//			DoTransition (typeof(FitState_AM_AirAttack));
//			return;
//		}

//		if (controller.BfAction == BufferedAction.SPECIAL) {
//			DoTransition (typeof(FitState_AM_AirSpecial));
//			return;
//
//		}

		if (controller.BfAction == BufferedAction.SHIELD)
		{
			if (controller.CanWavedash (controller.jump.AirdashHeight)) 
			{
				DoTransition (typeof(FitState_AM_Wavedash));
				return;
			}
		}

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
			
		if (controller.x_direction != controller.x_facing) {
			DoTransition (typeof(FitState_AM_AirHopBack));
			return;
		} else {
			DoTransition (typeof(FitState_AM_AirJump));
			return;
		}

	}




}