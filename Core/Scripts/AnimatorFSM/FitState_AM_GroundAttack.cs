using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_GroundAttack : BaseFSMState
{


		RayCastColliders controller;
		public Animation anim; 

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.ATTACK;
				anim = controller.anima;
				anim.Play ("Jab");
				controller.ClearBuffer ();
				controller.ApplyFriction = true;
				controller.C_Drag = controller.movement.friction;
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


}