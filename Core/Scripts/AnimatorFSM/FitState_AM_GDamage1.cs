using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_GDamage1 : BaseFSMState {

		RayCastColliders controller;
		//public Animation anim; 

		public override void Enter()
		{
				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.HITSTOP;
				//anim = controller.anima;
				controller.FitAnima.Play ("GDamage1");
				controller.ApplyFriction = true;
				controller.C_Drag = controller.movement.friction;
				controller.FitAnima.Update (0);
		}

		public override void Exit()
		{
		}

		// Update is called once per frame
		void Update () {

		}
}
