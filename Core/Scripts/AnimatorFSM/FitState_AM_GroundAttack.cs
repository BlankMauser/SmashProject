using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_GroundAttack : BaseFSMState
{


		RayCastColliders controller;
		//public Animation anim; 

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.ATTACK;
				//anim = controller.anima;
				controller.FitAnima.Play ("Jab");
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
				controller.previousState = CharacterState.ATTACK;
		}

		public override void Update()
		{
				if (controller.EndAnim == true) {
						controller.EndAnim = false;
						DoTransition(typeof(FitState_AM_Idle));
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


}