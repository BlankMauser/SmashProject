using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Jump : BaseFSMState
{


		RayCastColliders controller;
		//public Animation anim; 
		public bool JuDccel;
		public float InitVel;
		public bool FastFall;
		public int localXdir;
		public int localXAxl;

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.JUMPING;
				//anim = controller.anima;
				FastFall = false;
				controller.FitAnima.Play ("JumpF");


				if (controller.BfAction == BufferedAction.SHIELD) 
				{
						controller.velocity.y = 16;
						controller.BfAction = BufferedAction.SHIELD;
				} 
				else 
				{

			if (controller.Inputter.jumpButtonHeld == true || controller.Inputter.TapjumpButtonHeld == true) 
						{
								controller.velocity.y = controller.jump.jumpVelocity;
						} 
						else 
						{
								controller.velocity.y = controller.jump.hopVelocity;
						}
				}
						
				controller.C_Drag = controller.jump.AirDrag;
				if (Mathf.Abs (controller.velocity.x) >= controller.jump.jumpMaxHVelocity) {
						JuDccel = true;
				}
				InitVel = controller.velocity.x;
		}

		public override void Exit()
		{
		EndTerms ();
		}

		public override void Update()
		{
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
				} 
				else {
						controller.ApplyFriction = true;
				}

				if (controller.ApplyFriction == false) 
				{
						if (JuDccel == true) {
								if (Mathf.Abs (controller.velocity.x) <= controller.jump.jumpMaxHVelocity) 
								{
								JuDccel = false;
								}
								float newVelocity = (Mathf.Abs (controller.velocity.x) - controller.C_Drag);
								int localXdir;
								if (controller.velocity.x > 0) {
										localXdir = 1;
								} else {
										localXdir = -1;
								}
								if (newVelocity <= controller.jump.jumpMaxHVelocity) {
										newVelocity = controller.jump.jumpMaxHVelocity;
										JuDccel = false;
								}
								controller.velocity.x = newVelocity * localXdir;
						}


						float AxisVel = (controller.velocity.x + (controller.jump.AirMobility * controller.x_direction));
						if (controller.velocity.x > 0) {
								localXAxl = 1;
						} else if (controller.velocity.x < 0) {
								localXAxl = -1;
						} else if (controller.velocity.x == 0) {
								localXAxl = controller.x_facing;
						}
						if (Mathf.Abs (AxisVel) >= controller.jump.jumpMaxHVelocity && JuDccel == false) {
								AxisVel = controller.jump.jumpMaxHVelocity * localXAxl;
						}
						if (Mathf.Abs (AxisVel) <= Mathf.Abs(InitVel) || Mathf.Abs (AxisVel) <= controller.jump.jumpMaxHVelocity) {
//								if (JuDccel == false) {
										controller.velocity.x = AxisVel;
//								}
						}
				}


//				controller.Inputter.GetInput ();
//				controller.Inputter.ProcessInput ();
				if (controller.BfAction == BufferedAction.ATTACK)
				{
						object[] args = new object[3];
						args[0] = InitVel;
						args[1] = JuDccel;
						args[2] = FastFall;
						DoTransition(typeof(FitState_AM_AirAttack), args);
						return;
				}

// DEPRECATED
//				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
//						if (FastFall == false) {
//								
//										controller.velocity.y += controller.jump.fallGravity;
//										if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
//												controller.velocity.y = controller.jump.MaxFallSpeed;
//										}
//								
//						} else {
//								
//										controller.velocity.y = controller.jump.fastFallGravity;
//								if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
//										controller.velocity.y = controller.jump.fastFallGravity;
//								}
//						 
//				}
//				} else 
//				{
////						if (controller.PreviousBottom.y >= controller.CurrentBottom.y) {
//								DoTransition (typeof(FitState_AM_Land));
//								return;
////						}
//						
//				}

		if (FastFall) {
			bool input = (controller.Inputter.y <= -0.7f);
			if (controller.IsGrounded (controller.groundedLookAhead, input) == false) {
				controller.velocity.y = controller.jump.fastFallGravity;
				if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
					controller.velocity.y = controller.jump.fastFallGravity;
				}
				controller.Animator.PassThroughTimer = 1;
			} else {
				controller.Animator.PassThroughTimer = 0;
				DoTransition (typeof(FitState_AM_Land));
				return;
			}
		} else {
			if (controller.IsGrounded (controller.groundedLookAhead) == false) {
				controller.velocity.y += controller.jump.fallGravity;
				if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
					controller.velocity.y = controller.jump.MaxFallSpeed;
				}
			} else {
				DoTransition (typeof(FitState_AM_Land));
				return;
			}
		}

				if (controller.BfAction == BufferedAction.JUMP) {
						CheckJump ();
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

				CheckLedge ();

				if (controller.EndAnim == true) {
						controller.EndAnim = false;
						DoTransition (typeof(FitState_AM_Fall));
						return;
				}

		}

		public void CheckLedge() {
		if (controller.Fledge == true) {
			if (controller.PreviousBottom.y > controller.CurrentBottom.y && controller.Inputter.y >= -0.5f) {
				DoTransition (typeof(FitState_AM_LedgeCatch));
				return;
				}
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

	public void EndTerms() {

		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		return;
	}

}