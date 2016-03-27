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

		void ApplyHitbox(HitboxData hitbox) 
		{

				if (!DamageHitboxes.Contains(CurrentDmg.HboxSeed)) 
						{
								if (CurrentDmg.MyPriority != hitbox.MyPriority) 
								{
										CurrentDmg = hitbox;
										DamageHitboxes.Enqueue (CurrentDmg.HboxSeed);
										ApplyHitboxFrame = true;
								}
						}
		}

		public void DamageCalc()
		{
				Percent += CurrentDmg.Damage;
				CurrentDmg.MyPriority = 0;
				ApplyHitboxFrame = false;
		}

}