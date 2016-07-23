using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Tumble : BaseFSMState {

		RayCastColliders controller;
		public int localXdir;
		public int localXAxl;
		public float AxisVel;
		public float AnimRot;


		public override void Enter()
		{
				//Check Amount of Knockback
				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.TUMBLE;
				controller.ClearBuffer ();
				controller.EndAnim = false;
				controller.IASA = true;
				controller.ApplyFriction = false;
				controller.Animator.AnimateSpine = true;
				controller.FitAnima.Play ("Tumble");
				AnimRot = 0;
				

		}

		public override void Exit()
		{
		controller.EndAnim = false;
		controller.IASA = false;
		controller.Animator.AnimateSpine = false;
		controller.Animator.spinespin = Vector3.zero;
		}

		// Update is called once per frame
		public override void Update () {

		AnimRot -= 3f;

				if (controller.IASA == true) 
				{
						CheckIASA ();
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

		if (controller.IsGrounded (controller.groundedLookAhead) == false) {

			controller.velocity.y += controller.jump.fallGravity;
			if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
				controller.velocity.y = controller.jump.MaxFallSpeed;
			}
            

		} else {
			if (controller.PreviousBottom.y >= controller.CurrentBottom.y) {
			controller.kbvelocity = Vector3.zero;
			if (controller.Inputter.FramesLPressed <= 10) { 
				if (Mathf.Abs (controller.Inputter.x) >= 0.7f) {
					if (Mathf.Sign (controller.Inputter.x) == controller.x_facing) {
						Teching (1);
						return;
					} else {
						Teching (2);
						return;
					}
				} else {
					Teching (3);
					return;
				}
				


			} else {
				
				DoTransition (typeof(FitState_AM_Land));
				return;
				
				}
			}
		}

		//Teching
		// 1=RollF 2=RollB 3=TechNGround
//		if (controller.Inputter.ShieldButtonDown) {
//			if (controller.Inputter.FramesLPressed <= 10) {
//				if (controller.CanTech((Vector3.up * -1),Vector2.Distance(controller.CurrentLeft,controller.CurrentBottom),true))
//					{
//						if (Mathf.Abs(controller.Inputter.x) >= 0.7f)
//						{
//							if (Mathf.Sign(controller.Inputter.x) == controller.x_facing)
//							{
//								Teching(1);
//								return;
//							} else
//							{
//								Teching(2);
//								return;
//							}
//						} else
//						{
//							Teching(3);
//							return;
//						}
//					}
//
//
//			}
//
//		}


    }

		public override void LateUpdate()
		{

		controller.Animator.spinespin.x = AnimRot;

				if (controller.Strike.ApplyHitboxFrame == true) 
				{
						HitboxCollision ();
				}
			



		}



		public void HitboxCollision() {
				controller.Strike.DamageCalc ();
				object[] args = new object[1];
				args[0] = false;
				DoTransition (typeof(FitState_AM_HitStop), args);
				return;

		}

			public void Teching(int animnumber) {
			controller.IASA = false;
			object[] args = new object[1];
			args[0] = animnumber;
			DoTransition (typeof(FitState_AM_Ukemi), args);
			return;

		}

		public void CheckIASA() {





		}


}
