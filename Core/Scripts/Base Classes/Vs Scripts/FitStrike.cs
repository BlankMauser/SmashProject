using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FitStrike : MonoBehaviour {

		public bool ApplyHitboxFrame = false;
		public bool ApplyProjFrame = false;
		public Queue DamageHitboxes = new Queue (10);
		public HitboxCollider[] hitboxes;
		public RayCastColliders controller;
		public FitAnimator animator;
		public int MyId;
		public float Percent = 0f;
		public Hbox[] AttackBoxes;
		public int HitComboSeed = 1;
		public HitboxData CurrentDmg;

		public int kbStackTimer = 0;

		public int ComboReference;
		public int AnimReference;
		public bool CancelWindow = false;
	public bool HIT = false;
	public bool BLOCKED = false;

	//Cancel On:
	//1: Hit
	//2: Block
	//3: Hit/Block
	//4: Whiff
	public int CancelOn = 0;
	//Chains Into:
	//1: Jab
	//2: Jab/Dtilt/Ftilt
	//3: Dtilt
	//4: Ftilt
	public int HunterChain = 0;

		//Deprecated?
//		public void UpdateHitboxSize(int id)
//		{
//				hitboxes [id].UpdateSize();
//		}
//
//		public void UpdateAttributes(int id)
//		{
//				int attackid = (int)hitboxes [id].AttackQueue;
//				hitboxes [id].MyHbox = AttackBoxes [attackid];
//		}
//				
//
//		void OnTriggerEnter(Collider c) 
//		{
//				if (c.gameObject.layer == 19) 
//				{
//						Debug.Log ("Colliding Hitboxes");
//				}
//		}

		public void Update()
		{
				TimersTick ();
		}
				

		void ApplyHitbox(HitboxData hitbox) 
		{

				if (!DamageHitboxes.Contains(hitbox.HboxSeed)) 
						{
								if (CurrentDmg.MyPriority <= hitbox.MyPriority) 
								{
										HitboxData hitboxData = hitbox;
										CurrentDmg = hitboxData;

										ApplyHitboxFrame = true;
								}
						}
		}

		public void DamageCalc()
		{
				Percent += CurrentDmg.Damage;
				DamageHitboxes.Enqueue (CurrentDmg.HboxSeed);
				CurrentDmg.MyPriority = 0;
				ApplyHitboxFrame = false;
		}

		public void ShieldCalc()
		{
		DamageHitboxes.Enqueue (CurrentDmg.HboxSeed);
		CurrentDmg.MyPriority = 0;
		ApplyHitboxFrame = false;
		}

		public void TimersTick()
		{
				if (kbStackTimer > 0) 
				{
						kbStackTimer -= 1;
				}
		}
		
}