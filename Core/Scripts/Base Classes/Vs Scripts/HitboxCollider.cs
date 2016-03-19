using UnityEngine;
using System.Collections;

public class HitboxCollider : MonoBehaviour {

		public Hbox MyHbox;
		public SphereCollider Hitbox;
		public HitboxType Type;
		public int MyOwnerId;
		public int HitboxSeed;
		public float PriorityId;
		public float AttackQueue;
		public float Size;

		public void UpdateSize()
		{
				Hitbox.radius = Size;

		}
				

}