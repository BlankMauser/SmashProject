using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM : BaseFSMState
{
		public override void SetupDefinition(ref FSMStateType stateType, ref List<System.Type> children)
		{
				// default is an OR-type state
				// the first child added will be the inital state
				stateType = FSMStateType.Type_OR;
				children.Add(typeof(FitState_AM_Idle));
				children.Add(typeof(FitState_AM_Fall));
				children.Add(typeof(FitState_AM_WalkSlow));
				children.Add(typeof(FitState_AM_WalkFast));
				children.Add(typeof(FitState_AM_InitDash));
				children.Add(typeof(FitState_AM_Run));
				children.Add(typeof(FitState_AM_RunBrake));
				children.Add(typeof(FitState_AM_RunTurn));
				children.Add(typeof(FitState_AM_Pivot));
				children.Add(typeof(FitState_AM_GroundAttack));
				children.Add(typeof(FitState_AM_Jump));
				children.Add(typeof(FitState_AM_JumpBackward));
				children.Add(typeof(FitState_AM_JumpSquat));
				children.Add(typeof(FitState_AM_AirJump));
				children.Add(typeof(FitState_AM_AirHopBack));
				children.Add(typeof(FitState_AM_CrouchSquat));
				children.Add(typeof(FitState_AM_Crouch));
				children.Add(typeof(FitState_AM_Land));
				children.Add(typeof(FitState_AM_Wavedash));
				children.Add(typeof(FitState_AM_WavedashLand));
				children.Add(typeof(FitState_AM_Ukemi));
				children.Add(typeof(FitState_AM_LedgeCatch));
				children.Add(typeof(FitState_AM_AirAction));
				children.Add(typeof(FitState_AM_AirAttack));
				children.Add(typeof(FitState_AM_Pass));
				children.Add(typeof(FitState_AM_HitStop));
				children.Add(typeof(FitState_AM_HitStun));
				children.Add(typeof(FitState_AM_HitStunFly));
//				children.Add(typeof(FitState_AM_GDamage2));
//				children.Add(typeof(FitState_AM_ADamage1));
//				children.Add(typeof(FitState_AM_ADamage2));
				children.Add(typeof(FitState_AM_ShieldEnter));
				children.Add(typeof(FitState_AM_Shield));
				children.Add(typeof(FitState_AM_ShieldOff));

		}

		public override void Enter()
		{

		}

		public override void Exit()
		{
		}

		public override void Update()
		{
				DoTransition(typeof(FitState_AM_Idle));
		}


}