using UnityEngine;
using System.Collections;

public class FitStrike : MonoBehaviour {

		public HitboxCollider[] hitboxes;
		public RayCastColliders controller;
		public FitAnimator animator;
		public float Percent = 0f;
		public Hbox[] Hitboxes;
		public int HitSeed;
		public int currentCSeed;
		public int currentCPriority;
		public int currentCHurtbox;


		public void UpdateHitboxSize(int id)
		{
				hitboxes [id].UpdateSize();
		}

		public void UpdateAttributes(int id)
		{
				int attackid = (int)hitboxes [id].AttackQueue;
				hitboxes [id].MyHbox = Hitboxes [attackid];
		}

		void OnTriggerEnter(Collider c) {
				if (c.gameObject.layer == 19) 
				{
						Debug.Log ("Colliding Hitboxes");
				}
		}

}