using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Ukemi : BaseFSMState
{

		RayCastColliders controller;
		public int TechDirection;

	public FitState_AM_Ukemi()
	{

	}
		
	public FitState_AM_Ukemi(int TechAnim)
		{
		
		TechDirection = TechAnim;

		}


		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.UKEMI;
				//anim = controller.anima;
				DecideTech(TechDirection);
				controller.velocity.x = 0;
				controller.ApplyFriction = false;
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


				
		}

	public void DecideTech(int tech)
	{
		//Teching
		// 1=RollF 2=RollB 3=TechNGround
		switch (tech)
		{
		case 1:
			controller.NoFall = true;
			controller.FitAnima.Play ("UkemiF");
			break;
		
		case 2:
			controller.NoFall = true;
			controller.FitAnima.Play ("UkemiB");
			break;

		case 3:
			controller.NoFall = true;
			controller.FitAnima.Play ("UkemiN");
			break;
		}
	}

	public void EndTerms() {

		controller.Inputter.TechPenalty = 0;
		controller.NoFall = false;
		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		return;
	}


}
