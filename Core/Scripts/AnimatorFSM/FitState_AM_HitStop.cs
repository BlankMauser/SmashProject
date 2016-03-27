using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_HitStop : BaseFSMState {

		RayCastColliders controller;
		public int HitStopTimer;
		//public Animation anim; 

		public override void Enter()
		{
				//Check Amount of Knockback
				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.HITSTOP;
				//anim = controller.anima;
				controller.FitAnima.Play ("GDamage2");
				controller.ApplyFriction = true;
				controller.C_Drag = controller.movement.friction;
				controller.FitAnima.Update (0);
				controller.FitAnima.enabled = false;
		}

		public override void Exit()
		{
		}

		// Update is called once per frame
		void Update () {

		}
}
