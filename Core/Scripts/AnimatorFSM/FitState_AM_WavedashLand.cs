using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_WavedashLand : BaseFSMState
{

		RayCastColliders controller;
		public Animation anim; 

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.WAVEDASHLAND;
				anim = controller.anima;
				anim.Play ("WaveDashL");
				controller.ClearBuffer ();
				controller.ApplyFriction = true;
				controller.C_Drag = controller.movement.friction;
		}

		public override void Exit()
		{
				controller.previousState = CharacterState.WAVEDASHLAND;
		}

		public override void Update()
		{
				//				controller.Inputter.GetInput ();
				//				controller.Inputter.ProcessInput ();
				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
						DoTransition (typeof(FitState_AM_Fall));
						return;
				}

				if (!anim.isPlaying) {
						DoTransition (typeof(FitState_AM_Idle));
						return;
				}


		}


}

