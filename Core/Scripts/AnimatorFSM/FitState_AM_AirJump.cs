using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_AirJump : BaseFSMState
{


		RayCastColliders controller;
		//public Animation anim; 
		public float InitVel;
		public bool FastFall;
		public bool HopWindow;
		public bool HopDccel;
		public int HopTimer;
		public float AxisVel;
		public int localXAxl;

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.AIRJUMP;
				//anim = controller.anima;
				FastFall = false;
				HopWindow = false;
				HopDccel = false;
				controller.FitAnima.Play ("JumpF");
				HopTimer = 2;
				controller.velocity.x = 0;
				if (controller.Inputter.x > 0.7f) {
						controller.x_direction = 1;
						controller.velocity.x = controller.jump.jumpHorizontalVelocity;
				} else if (controller.Inputter.x < -0.7f) {
						controller.x_direction = -1;
						controller.velocity.x = controller.jump.jumpHorizontalVelocity*-1;
				}
			
				AxisVel = 0;
				controller.velocity.y = controller.jump.doubleJumpVelocity;
				controller.ClearBuffer ();
				controller.C_Drag = controller.jump.AirDrag;
		}

		public override void Exit()
		{
				controller.previousState = CharacterState.AIRJUMP;
		}

		public override void Update()
		{

				if (HopTimer == 0 && HopWindow == false) {
						if (controller.velocity.y > 0 && controller.Inputter.jumpButtonHeld == false) {
								HopDccel = true;
						}
						HopWindow = true;
				} 
				if (HopTimer != 0) {
						HopTimer -= 1;
				}

				if (HopDccel == true && controller.velocity.y > 0) {
						controller.velocity.y = controller.velocity.y * controller.jump.AirHopMultiplier;
				}

				//Check for Fast Fall
				if (controller.Inputter.y <= -0.7f && controller.Inputter.FramesYNeutral <= 5 && controller.velocity.y <= 0) {
						FastFall = true;
				}

				// If stick is neutral, apply friction.
				if (controller.Inputter.x > 0.7f) {
						controller.x_direction = 1;
						controller.ApplyFriction = false;
				} else if (controller.Inputter.x < -0.7f) {
						controller.x_direction = -1;
						controller.ApplyFriction = false;
				} else {
						controller.ApplyFriction = true;
				}
						
				if (controller.ApplyFriction == false) {
						AxisVel = controller.velocity.x + (controller.jump.AirMobility * controller.x_direction);
						if (controller.velocity.x > 0) {
								localXAxl = 1;
						} else if (controller.velocity.x < 0) {
								localXAxl = -1;
						} else if (controller.velocity.x == 0) {
								localXAxl = controller.x_facing;
						}
						if (Mathf.Abs (AxisVel) > controller.jump.jumpMaxHVelocity) {
								AxisVel = controller.jump.jumpMaxHVelocity * localXAxl;
						}
						if (Mathf.Abs (AxisVel) <= controller.jump.jumpMaxHVelocity) {
								controller.velocity.x = AxisVel;
						}
				}


				//				controller.Inputter.GetInput ();
				//				controller.Inputter.ProcessInput ();

				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
						if (FastFall == false) {
								controller.velocity.y += controller.jump.fallGravity;
								if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
										controller.velocity.y = controller.jump.MaxFallSpeed;
								}
						} else {
								controller.velocity.y = controller.jump.fastFallGravity;
								if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
										controller.velocity.y = controller.jump.fastFallGravity;
								}
						} 
				} else 
				{
						DoTransition(typeof(FitState_AM_Land));
						return;
				}

				if (controller.BfAction == BufferedAction.SHIELD)
				{
						if (controller.CanWavedash (controller.jump.AirdashHeight)) 
						{
								DoTransition (typeof(FitState_AM_Wavedash));
								return;
						}
				}

				if (controller.EndAnim == true) {
						controller.EndAnim = false;
						DoTransition (typeof(FitState_AM_Fall));
						return;
				}

		}


}
