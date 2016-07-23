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
		public HitboxData CurrentDmgB;

		public int kbStackTimer = 0;

		public int ComboReference;
		public int AnimReference;
	public bool HIT = false;
	public bool BLOCKED = false;

	public EnergyBar MyPercent;



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

		public void LateUpdate()
		{
		SetUI ();
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

		void ApplyProjectile(HitboxData proj) 
		{

		if (!DamageHitboxes.Contains(proj.HboxSeed)) 
			{
			if (CurrentDmg.MyPriority <= proj.MyPriority) 
				{
				HitboxData hitboxData = proj;
					CurrentDmg = hitboxData;

					ApplyProjFrame = true;
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

	public void DamageCalcB()
	{
		Percent += CurrentDmgB.Damage;
		DamageHitboxes.Enqueue (CurrentDmg.HboxSeed);
		CurrentDmg.MyPriority = 0;
		ApplyProjFrame = false;
	}

	public void ShieldCalcB()
	{
		DamageHitboxes.Enqueue (CurrentDmg.HboxSeed);
		CurrentDmgB.MyPriority = 0;
		ApplyProjFrame = false;
	}

		public void TimersTick()
		{
				if (kbStackTimer > 0) 
				{
						kbStackTimer -= 1;
				}
		}

		public void SetUI()
		{
		MyPercent.valueCurrent = (int)Percent;
		}
		
}