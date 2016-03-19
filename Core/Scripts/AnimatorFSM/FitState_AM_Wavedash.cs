using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Wavedash : BaseFSMState
{

		RayCastColliders controller;
		public Animation anim; 
		public int IASA_Timer;

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.WAVEDASH;
				anim = controller.anima;
				controller.ApplyFriction = true;
				controller.C_Drag = controller.movement.friction;
				controller.velocity.x = controller.jump.WavedashVelocity * controller.Inputter.x;
				if (controller.velocity.y > 0) 
				{
						controller.velocity.y *= 0.08f;
				}
				IASA_Timer = 12;

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




}