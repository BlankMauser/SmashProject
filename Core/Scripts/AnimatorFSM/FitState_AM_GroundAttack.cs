using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_GroundAttack : BaseFSMState
{


		RayCastColliders controller;
		//public Animation anim; 

	public List<string> UsedAttacks = new List<string> (8);
	public int DtiltRef = 0;
	public int FtiltRef = 0;
	public int JabRef = 0;

	//1: Dtilt
	//2: Ftilt
	//3: Jab
	public int CurrentAttack = 0;

		public override void Enter()
		{
				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.ATTACK;
				//anim = controller.anima;
				DtiltRef = 41; 
				FtiltRef = 42;
				JabRef = 43; 
				CheckGroundAttack();
				controller.ClearBuffer ();
				controller.ApplyFriction = true;
		if (controller.previousState == CharacterState.WAVEDASHLAND) {
			controller.C_Drag = controller.battle.WavedashFriction;
		} else {
			controller.C_Drag = controller.movement.friction;
		}
				SeedUp ();
		}

		public override void Exit()
		{
		EndTerms ();
		}

		public override void Update()
		{

		if (controller.EndAnim == true) {
			controller.EndAnim = false;
			DoTransition(typeof(FitState_AM_Idle));
			return;
		}

		if (controller.BfAction == BufferedAction.SPECIAL) {
			CheckSpecialCancel ();
			return;
		}


		if (controller.BfAction == BufferedAction.ATTACK && controller.FitAnima.enabled) {
			CheckCancel ();
			return;
		}

		if (controller.IASA) {
			CheckIASA ();
		}

		}

		public override void LateUpdate()
	{
		switch (CurrentAttack) {
		case 1:
			DtiltRef = controller.Strike.ComboReference;
			break;
		case 2:
			FtiltRef = controller.Strike.ComboReference;
			break;
		case 3:
			JabRef = controller.Strike.ComboReference;
			break;
		default:
			break;
		}
	}

	public void CheckGroundAttack() {
		Cardinals AttackDir = controller.Inputter.ReturnAxisAerial();
		switch (AttackDir) {
		case Cardinals.Left:
			controller.x_facing = -1;
			controller.Animator.CorrectColliders ();
			if (controller.Inputter.buffer_x >= -0.8f || controller.Inputter.FramesXNeutral > 5) {
				controller.FitAnima.Play ("Utilt", 0, 0f);
			} else { 
				controller.FitAnima.Play ("Fsmash", 0, 0f);
			}
			break;
		case Cardinals.Right:
			controller.x_facing = 1;
			controller.Animator.CorrectColliders ();
			if (controller.Inputter.buffer_x <= 0.8f || controller.Inputter.FramesXNeutral > 5) {
				controller.FitAnima.Play ("Utilt", 0, 0f);
			} else { 
				controller.FitAnima.Play ("Fsmash", 0, 0f);
			}
			break;

		case Cardinals.Up:
			if (controller.Inputter.buffer_x >= 0.06f) {
				controller.x_facing = 1;
				controller.Animator.CorrectColliders ();
			}
			if (controller.Inputter.buffer_x <= -0.06f) {
				controller.x_facing = -1;
				controller.Animator.CorrectColliders ();
			}
			if (controller.Inputter.buffer_y <= 0.8f || controller.Inputter.FramesYNeutral > 5) {
				controller.FitAnima.Play ("Utilt", 0, 0f);
			} else { 
				controller.FitAnima.Play ("Utilt", 0, 0f);
			}
			break;

		case Cardinals.Down:
			if (controller.Inputter.buffer_x >= 0.06f) {
				controller.x_facing = 1;
				controller.Animator.CorrectColliders ();
			}
			if (controller.Inputter.buffer_x <= -0.06f) {
				controller.x_facing = -1;
				controller.Animator.CorrectColliders ();
			}
			if (controller.Inputter.buffer_y >= -0.8f || controller.Inputter.FramesYNeutral > 5) {
				controller.FitAnima.Play ("Dtilt", 0, 0f);
				CurrentAttack = 1;
			} else { 
				controller.FitAnima.Play ("Utilt", 0, 0f);
			}
			break;

		default:
			controller.FitAnima.Play ("Jab", 0, 0f);
			CurrentAttack = 3;
			break;
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

	void CheckCancel()
	{
		switch ((int)controller.Animator.CancelOn) {
		case 1:
			if (controller.Strike.HIT && controller.Animator.CancelWindow) {
				NextAnimGround (controller.Strike.ComboReference);
			}
			break;
		case 3:
			if (controller.Strike.BLOCKED || controller.Strike.HIT) {
				if (controller.Animator.CancelWindow) {
					NextAnimGround (controller.Strike.ComboReference);
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
				DoTransition (typeof(FitState_AM_GroundSpecial));
				return;
			}
			break;
		case 3:
			if (controller.Strike.BLOCKED || controller.Strike.HIT) {
				if (controller.Animator.CancelWindow) {
					DoTransition (typeof(FitState_AM_GroundSpecial));
					return;
				}
			}
			break;
		}
	}



	void NextAnimGround(int AID) 
	{
		Cardinals ComboDir = controller.Inputter.ReturnAxisAerial();
		switch (ComboDir) {
		case Cardinals.Left:
			if (controller.Animator.HunterChain == 2 || controller.Animator.HunterChain == 4 || controller.Animator.HunterChain == 7 || controller.Animator.HunterChain == 8) {
				controller.x_facing = -1;
				controller.Animator.CorrectColliders ();
				if (controller.Inputter.buffer_x >= -0.8f || controller.Inputter.FramesXNeutral > 5) {
					if (CurrentAttack == 2) {
						if (!UsedAttacks.Contains (controller.Strike.AttackBoxes [AID].animationName)) {
							controller.FitAnima.Play (controller.Strike.AttackBoxes [AID].animationName);
							controller.FitAnima.Update (0);
							ClearFlags ();
						} else {
							break;
						}
					} else {
						if (!UsedAttacks.Contains (controller.Strike.AttackBoxes [FtiltRef].animationName)) {
							controller.FitAnima.Play (controller.Strike.AttackBoxes [FtiltRef].animationName);
							controller.FitAnima.Update (0);
							ClearFlags ();
						} else {
							break;
						}
					}
				} else {
					if (controller.Animator.HunterChain == 7 || controller.Animator.HunterChain == 8)
						controller.FitAnima.Play ("Fsmash", 0, 0f);
				}
			}
			break;
		case Cardinals.Right:
			if (controller.Animator.HunterChain == 2 || controller.Animator.HunterChain == 4 || controller.Animator.HunterChain == 7 || controller.Animator.HunterChain == 8) {
				controller.x_facing = 1;
				controller.Animator.CorrectColliders ();
				if (controller.Inputter.buffer_x >= -0.8f || controller.Inputter.FramesXNeutral > 5) {
					if (CurrentAttack == 2) {
						if (!UsedAttacks.Contains (controller.Strike.AttackBoxes [AID].animationName)) {
							controller.FitAnima.Play (controller.Strike.AttackBoxes [AID].animationName);
							controller.FitAnima.Update (0);
							CurrentAttack = 2;
							ClearFlags ();
						} else {
							break;
						}
					} else {
						if (!UsedAttacks.Contains (controller.Strike.AttackBoxes [FtiltRef].animationName)) {
							controller.FitAnima.Play (controller.Strike.AttackBoxes [FtiltRef].animationName);
							controller.FitAnima.Update (0);
							CurrentAttack = 2;
							ClearFlags ();
						} else {
							break;
						}
					}
				} else {
					if (controller.Animator.HunterChain == 7 || controller.Animator.HunterChain == 8)
						controller.FitAnima.Play ("Fsmash", 0, 0f);
				}
			}
			break;

		case Cardinals.Up:
			break;

		case Cardinals.Down:
			if (controller.Animator.HunterChain == 2 || controller.Animator.HunterChain == 3 || controller.Animator.HunterChain == 6 || controller.Animator.HunterChain == 8) {
				if (controller.Inputter.buffer_x >= 0.06f) {
					controller.x_facing = 1;
					controller.Animator.CorrectColliders ();
				}
				if (controller.Inputter.buffer_x <= -0.06f) {
					controller.x_facing = -1;
					controller.Animator.CorrectColliders ();
				}
				if (controller.Inputter.buffer_y >= -0.8f || controller.Inputter.FramesYNeutral > 5) {
					if (CurrentAttack == 1) {
						if (!UsedAttacks.Contains (controller.Strike.AttackBoxes [AID].animationName)) {
							controller.FitAnima.Play (controller.Strike.AttackBoxes [AID].animationName);
							controller.FitAnima.Update (0);
							CurrentAttack = 1;
							ClearFlags ();
						} else {
							break;
						}
					} else {
						if (!UsedAttacks.Contains (controller.Strike.AttackBoxes [DtiltRef].animationName)) {
							controller.FitAnima.Play (controller.Strike.AttackBoxes [DtiltRef].animationName);
							controller.FitAnima.Update (0);
							CurrentAttack = 1;
							ClearFlags ();
						} else {
							break;
						}
					}
				} else {
					if (controller.Animator.HunterChain == 7 || controller.Animator.HunterChain == 8)
						controller.FitAnima.Play ("Dsmash", 0, 0f);
				}
			}
			break;

		default:
			if (controller.Animator.HunterChain == 1 || controller.Animator.HunterChain == 2 || controller.Animator.HunterChain == 8) {
						if (CurrentAttack == 3) {
						if (!UsedAttacks.Contains (controller.Strike.AttackBoxes [AID].animationName)) {
							controller.FitAnima.Play (controller.Strike.AttackBoxes [AID].animationName);
							controller.FitAnima.Update (0);
							CurrentAttack = 3;
							ClearFlags ();
						} else {
							break;
						}
					} else {
						if (!UsedAttacks.Contains (controller.Strike.AttackBoxes [JabRef].animationName)) {
							controller.FitAnima.Play (controller.Strike.AttackBoxes [JabRef].animationName);
							controller.FitAnima.Update (0);
							CurrentAttack = 3;
							ClearFlags ();
						} else {
							break;
						}
					}
			}
			break;

		}
			
	}

	public void ClearFlags() {
		controller.EndAnim = false;
		controller.IASA = false;
		controller.Strike.HIT = false;
		controller.Strike.BLOCKED = false;
		controller.ClearBuffer ();
		controller.ApplyFriction = true;
		if (controller.previousState == CharacterState.WAVEDASHLAND) {
			controller.C_Drag = controller.battle.WavedashFriction;
		} else {
			controller.C_Drag = controller.movement.friction;
		}
		SeedUp ();
	}

	public void CheckIASA() {

		if (controller.BfAction == BufferedAction.JUMP) {

			DoTransition (typeof(FitState_AM_JumpSquat));
			return;
		}

		if (controller.BfAction == BufferedAction.INIT_DASH) {
			DoTransition (typeof(FitState_AM_InitDash));
			return;
		}

		if (controller.BfAction == BufferedAction.SHIELD) {
			DoTransition (typeof(FitState_AM_ShieldEnter));
			return;
		}

		if (controller.BfAction == BufferedAction.SPECIAL) {
			DoTransition (typeof(FitState_AM_GroundSpecial));
			return;
		}

	}

	public void EndTerms() {

		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		controller.Strike.HIT = false;
		controller.Strike.BLOCKED = false;
		return;
	}



}