using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_Throwing : BaseFSMState
{


		RayCastColliders controller;
		Vector3 LedgeAnchor;
		//public Animation anim; 

		public override void Enter()
		{

				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				controller.state = CharacterState.ATTACK;
				//anim = controller.anima;
				controller.EndAnim = false;
				controller.ClearBuffer ();
				controller.ApplyFriction = true;
				CheckThrow ();
				SeedUp ();

				
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

		//For Testing
		//		controller.transform.position =	controller.LedgeGrabbed.position + controller.LedgeOffset;

		if (controller.IASA == true) {
			CheckIASA ();
		}

		}

	public override void LateUpdate()
	{
		controller.transform.position =	controller.LedgeGrabbed.position + LedgeAnchor;
	}


	public void	CheckThrow() {
		Cardinals ThrowDir = controller.Inputter.ReturnAxisAerial();
		switch (ThrowDir) {
		case Cardinals.Left:

			break;
		case Cardinals.Right:
			break;

		case Cardinals.Up:
			controller.FitAnima.Play ("Uthrow",0,0f);
			controller.FitAnima.Update (0);
			break;

		case Cardinals.Down:
			break;

		default:
			break;
		}
	}

	public void CheckIASA() {



	}

	void SeedUp()
	{
		controller.Strike.HitComboSeed += 1;
		if (controller.Strike.HitComboSeed > 100000) 
		{
			controller.Strike.HitComboSeed = 1;
		}
	}
		

	public void EndTerms() {

		controller.previousState = controller.state;
		controller.EndAnim = false;
		controller.IASA = false;
		controller.Strike.HIT = false;
		controller.Strike.BLOCKED = false;
		return;
	}


}