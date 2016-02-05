﻿using UnityEngine;
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
				children.Add(typeof(FitState_AM_Crouch));
				children.Add(typeof(FitState_AM_Land));
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