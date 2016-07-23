using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_AirAttack : BaseFSMState
{


		RayCastColliders controller;
		//public Animation anim; 
		public bool JuDccel;
		public float InitVel;
		public bool FastFall;
		public int localXdir;
		public int localXAxl;

		public FitState_AM_AirAttack()
		{
		}

		public FitState_AM_AirAttack(float init, bool JuDccl, bool FaFl)
		{
				InitVel = init;
				JuDccel = JuDccl;
				FastFall = FaFl;
		}

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.AIRATTACK;
				//anim = controller.anima;
				CheckAerial ();
				SeedUp ();
				controller.C_Drag = controller.jump.AirDrag;
				controller.ClearBuffer ();
		}

		public override void Exit()
		{
		EndTerms ();
		}

		public override void Update()
		{

		if (controller.FitAnima.enabled) {
			ApplyGravity ();
		}

		if (controller.IASA) {
			CheckIASAAir ();
		}

		if (controller.BfAction == BufferedAction.SHIELD)
		{
			if (controller.CanWavedash (controller.jump.AirdashHeight)) 
			{
				DoTransition (typeof(FitState_AM_Wavedash));
				return;
			}
		}

		if (controller.BfAction == BufferedAction.BULLET) {
			CheckBulletCancel ();

		}

		if (controller.BfAction == BufferedAction.SPECIAL) {
			CheckSpecialCancel ();

		}

				if (controller.EndAnim == true) {
						controller.EndAnim = false;
						DoTransition (typeof(FitState_AM_Fall));
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

	public void CheckAerial() {
		Cardinals AttackDir = controller.Inputter.ReturnAxisAerial();
		switch (AttackDir) {
		case Cardinals.Left:
			if (controller.x_facing == 1) {
				controller.FitAnima.Play ("Bair",0,0f);
				break;
			} else {
				controller.FitAnima.Play ("Fair",0,0f);
				break;
			}

		case Cardinals.Right:
			if (controller.x_facing == 1) {
				controller.FitAnima.Play ("Fair",0,0f);
				break;
			} else {
				controller.FitAnima.Play ("Bair",0,0f);
				break;
			}

		case Cardinals.Up:
			controller.FitAnima.Play ("Uair",0,0f);
			break;

		case Cardinals.Down:
			controller.FitAnima.Play ("Dair",0,0f);
			break;

		default:
			controller.FitAnima.Play ("Nair",0,0f);
			break;
		}



	}
		
	void CheckBulletCancel()
	{
		switch ((int)controller.Animator.CancelOn) {
		case 1:
			if (controller.Strike.HIT && controller.Animator.CancelWindow) {
				if (controller.FitAnima.enabled) {
					DoTransition (typeof(FitState_AM_AirBullet));
					return;
				}
			}
			break;
		case 3:
			if (controller.Strike.BLOCKED || controller.Strike.HIT) {
				if (controller.Animator.CancelWindow) {
					if (controller.FitAnima.enabled) {
						DoTransition (typeof(FitState_AM_AirBullet));
						return;
					}
				}
			}
			break;
		}
	}

	void CheckSpecialCancel()
	{
		switch ((int)controller.Animator.CancelOn) {
		case 1:
			if (controller.Strike.HIT && controller.Animator.CancelWindow) {
				if (controller.FitAnima.enabled) {
					DoTransition (typeof(FitState_AM_AirSpecial));
					return;
				}
			}
			break;
		case 3:
			if (controller.Strike.BLOCKED || controller.Strike.HIT) {
				if (controller.Animator.CancelWindow) {
					if (controller.FitAnima.enabled) {
						DoTransition (typeof(FitState_AM_AirSpecial));
						return;
					}
				}
			}
			break;
		}
	}

	public void ApplyGravity()
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
			//						if (controller.PreviousBottom.y >= controller.CurrentBottom.y) {
			DoTransition (typeof(FitState_AM_Land));
			return;
			//						}

		}

	}

	public void CheckIASAAir() {

		if (controller.BfAction == BufferedAction.ATTACK) {
			CheckAerial ();
			SeedUp ();
			controller.ClearBuffer ();
			controller.EndAnim = false;
			controller.IASA = false;
		}

		if (controller.BfAction == BufferedAction.BULLET)
		{
			object[] args = new object[3];
			args[0] = InitVel;
			args[1] = false;
			args[2] = FastFall;
			DoTransition(typeof(FitState_AM_AirBullet), args);
			return;
		}

		if (controller.BfAction == BufferedAction.SPECIAL) {
			if (controller.FitAnima.enabled) {
				DoTransition (typeof(FitState_AM_AirSpecial));
				return;
			}

		}
			
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

	void SeedUp()
	{
		controller.Strike.HitComboSeed += 1;
		if (controller.Strike.HitComboSeed > 100000) 
		{
			controller.Strike.HitComboSeed = 1;
		}
	}

	public void EndTerms() {
		controller.Strike.HIT = false;
		controller.Strike.BLOCKED = false;
		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		return;
	}

}