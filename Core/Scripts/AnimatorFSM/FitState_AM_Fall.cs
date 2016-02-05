using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Fall : BaseFSMState
{


		RayCastColliders controller;
		public Animation anim; 

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.FALLING;
				anim = controller.anima;
				anim.Play ("Idle");


		}

		public override void Exit()
		{
				controller.previousState = CharacterState.FALLING;
		}

		public override void Update()
		{
//				controller.Inputter.GetInput ();
//				controller.Inputter.ProcessInput ();
				if (controller.BfAction == BufferedAction.JAB)
				{

						DoTransition(typeof(FitState_AM_GroundAttack));
						return;
				}
				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
						controller.velocity.y += (controller.jump.fallGravity);
				} else 
				{
						DoTransition(typeof(FitState_AM_Land));
						return;
				}
		}


}