using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_WavedashLand : BaseFSMState
{

		RayCastColliders controller;
		//public Animation anim; 
		int I_Timer;

		public FitState_AM_WavedashLand()
		{
		}

		public FitState_AM_WavedashLand(int IASA)
		{
				I_Timer = IASA;
		}

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.WAVEDASHLAND;
				//anim = controller.anima;
				controller.FitAnima.Play ("WaveDashL");
				controller.ClearBuffer ();
				controller.ApplyFriction = true;
				controller.C_Drag = controller.battle.WavedashFriction;
		}

		public override void Exit()
		{
				EndTerms ();
		}

		public override void Update()
		{
				if (I_Timer == 0) {
						controller.IASA = true;
				} else 
				{
						I_Timer -= 1;
				}
				//				controller.Inputter.GetInput ();
				//				controller.Inputter.ProcessInput ();
				if (controller.IsGrounded (controller.groundedLookAhead) == false) {
						DoTransition (typeof(FitState_AM_Fall));
						return;
				}

				if (controller.IASA == true) 
				{
						CheckIASA ();
				}

				if (controller.EndAnim == true) {
						controller.EndAnim = false;
						DoTransition (typeof(FitState_AM_Idle));
						return;
				}


		}

		public void CheckIASA() {

				if (controller.BfAction == BufferedAction.JAB) {
						DoTransition (typeof(FitState_AM_GroundAttack));
						return;
				}

				if (controller.BfAction == BufferedAction.JUMP) {

						DoTransition (typeof(FitState_AM_JumpSquat));
						return;
				}


				if (controller.BfAction == BufferedAction.INIT_DASH) {
						DoTransition (typeof(FitState_AM_InitDash));
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

