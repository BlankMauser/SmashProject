using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSMHelper;

public class FitState_AM_HitStun : BaseFSMState {

		RayCastColliders controller;
		public int HitStopTimer;
		public HitboxData MyHitboxData;
		public HitboxData SentKnockback;
		public bool FromGround = false;
		//public Animation anim; 

		public FitState_AM_HitStun()
		{
		}

		public FitState_AM_HitStun(bool Grounded, HitboxData Sent)
		{
				FromGround = Grounded;
				SentKnockback = Sent;
		}

		public override void Enter()
		{
				//Check Amount of Knockback
				FitAnimatorStateMachine SM = (FitAnimatorStateMachine)GetStateMachine();
				controller = SM.m_GameObject.GetComponent<RayCastColliders>();
				MyHitboxData = controller.Strike.CurrentDmg;
				controller.state = CharacterState.HITSTOP;
				HitStopTimer = MyHitboxData.Hitlag;

				CheckKnockback ();
		}

		public override void Exit()
		{
		}

		// Update is called once per frame
		public override void Update () {


				if (controller.EndAnim == true && controller.Animator.HitStopAnim == 0) {
						controller.EndAnim = false;
						DoTransition (typeof(FitState_AM_Idle));
						return;
				}
		}

		public override void LateUpdate()
		{

				if (controller.Strike.ApplyHitboxFrame == true) 
				{
						Debug.Log ("Got Here");
						HitboxCollision ();
				}
		}
				

		public void CheckKnockback() {
				if (FromGround == true) 
				{
						
						controller.FitAnima.Play ("GDamage2", -1, 0f);
						controller.ApplyFriction = true;
						controller.C_Drag = controller.movement.friction;
						controller.FitAnima.Update (0);
						controller.Animator.HitStopAnim = HitStopTimer;
						MyHitboxData.OwnerCollider.Animator.HitStopAnim = (HitStopTimer-1);
				}
		}

		public void HitboxCollision() {
				controller.Strike.DamageCalc ();
				MyHitboxData = controller.Strike.CurrentDmg;
				controller.state = CharacterState.HITSTOP;
				HitStopTimer = MyHitboxData.Hitlag;

				CheckKnockback ();

		}


}
