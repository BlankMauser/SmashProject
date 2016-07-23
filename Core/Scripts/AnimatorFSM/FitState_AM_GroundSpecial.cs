using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_GroundSpecial : BaseFSMState
{


	RayCastColliders controller;
	//public Animation anim; 

	public int GravityTimer;

	public override void Enter()
	{

		FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
		controller = SM.m_GameObject.GetComponent<RayCastColliders>();
		GravityTimer = 2;
		controller.state = CharacterState.ATTACK;
		//anim = controller.anima;
		CheckGroundSpecial();
		SeedUp ();
	}


	public override void Exit()
	{
		EndTerms ();
	}

	public override void Update()
	{
		switch (controller.UpdateSwitch) {
		case 1:
			controller.UpSpecialGroundUpdate ();
			break;
		}

		if (controller.FitAnima.enabled) {
			if (GravityTimer == 0) {
				ApplyGravity ();
			} else {
				GravityTimer -= 1;
			}
		}

		if (controller.IASA) {
			CheckIASAAir ();
		}

		CheckLedge ();

		if (controller.EndAnim == true) {
			controller.EndAnim = false;
			DoTransition (typeof(FitState_AM_Fall));
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

	public void ApplyGravity()
	{
		bool input = (controller.Inputter.y <= -0.7f);
		if (controller.IsGrounded (controller.groundedLookAhead, input) == false) {
			controller.velocity.y += controller.jump.fallGravity;
			if (controller.velocity.y <= controller.jump.MaxFallSpeed) {
				controller.velocity.y = controller.jump.MaxFallSpeed;
			} 
			controller.Animator.PassThroughTimer = 1;

		} 	else	{
			if (controller.Animator.CanLand) {
				controller.Animator.PassThroughTimer = 0;
				DoTransition (typeof(FitState_AM_Land));
				return;
			}

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


	void NextAnimGround(int AID) 
	{
		controller.FitAnima.Play (controller.Strike.AttackBoxes[AID].animationName);
		controller.ClearBuffer ();
		controller.ApplyFriction = true;
		if (controller.previousState == CharacterState.WAVEDASHLAND) {
			controller.C_Drag = controller.battle.WavedashFriction;
		} else {
			controller.C_Drag = controller.movement.friction;
		}
		SeedUp ();
	}

	public void CheckLedge() {
		if (controller.Fledge == true || controller.Bledge == true) {
			if (controller.PreviousBottom.y > controller.CurrentBottom.y && controller.Inputter.y >= -0.5f) {
				DoTransition (typeof(FitState_AM_LedgeCatch));
				return;
			}
		}
	}


	public void EndTerms() {

		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		return;
	}

	public void CheckGroundSpecial() {
		Cardinals AttackDir = controller.Inputter.ReturnAxisAerial();
		switch (AttackDir) {
		case Cardinals.Left:
			controller.x_facing = -1;
			controller.Animator.CorrectColliders ();
			controller.FitAnima.Play ("SideSpecial", 0, 0f);
			controller.FitAnima.Update (0);
			controller.SideSpecialGroundInit ();
			break;
		case Cardinals.Right:
			controller.x_facing = 1;
			controller.Animator.CorrectColliders ();
			controller.FitAnima.Play ("SideSpecial",0,0f);
			controller.FitAnima.Update (0);
			controller.SideSpecialGroundInit ();
			break;

		case Cardinals.Up:
			if (controller.Inputter.buffer_x >= 0.05f) {
				controller.x_facing = 1;
				controller.Animator.CorrectColliders ();
			}
			if (controller.Inputter.buffer_x <= -0.05f) {
				controller.x_facing = -1;
				controller.Animator.CorrectColliders ();
			}
			controller.FitAnima.Play ("UpSpecial",0,0f);
			controller.FitAnima.Update (0);
			controller.UpSpecialGroundInit ();
			break;

		case Cardinals.Down:
			controller.FitAnima.Play ("DownSpecial",0,0f);
			controller.DownSpecialGroundInit ();
			break;

		default:
			controller.FitAnima.Play ("NeutralSpecial",0,0f);
			controller.NeutralSpecialGroundInit ();
			break;
		}



	}



}