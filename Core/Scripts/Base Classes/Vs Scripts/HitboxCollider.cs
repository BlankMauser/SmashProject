using UnityEngine;
using System.Collections;

public class HitboxCollider : MonoBehaviour {

		public Hbox MyHbox;
		public HitboxType Type;
		public FitStrike OwnerStrike;
		public int MyOwnerId;
		public int HitboxSeed;
		public float PriorityId;
		public float AttackQueue;
		public float Size;

		public void Start()
		{
				MyOwnerId = OwnerStrike.MyId;
		}
		

		public void LateUpdate()
		{
				foreach(var c in  Physics.OverlapSphere(transform.position, Size)) 
				{
						this.gameObject.SendMessage("OnTriggerDouble", c);
				}
		}

//		void OnTriggerEnter(Collider c) {
//				if (c.gameObject.layer == 20) {
//						Debug.Log ("Colliding Hurtboxes");
//						FitStrike enemy = c.GetComponentInParent<FitStrike> ();
//						if (enemy.MyId != MyOwnerId) 
//						{
//								if (OwnerStrike != null) 
//								{
//										MyHbox = OwnerStrike.AttackBoxes [(int)AttackQueue];
//										HitboxSeed = OwnerStrike.HitComboSeed;
//								}
//								HitboxData hitboxData = MyHbox.hitboxData;
//								hitboxData.ownerID = MyOwnerId;
//								hitboxData.HboxSeed = HitboxSeed;
//								enemy.SendMessage("ApplyHitbox", hitboxData, SendMessageOptions.DontRequireReceiver); // send damage object to target
//						}
//				}
//		}	

		void OnTriggerDouble(Collider c) {
				if (c.gameObject.layer == 20) {
						FitStrike enemy = c.GetComponentInParent<FitStrike> ();
						if (enemy.MyId != MyOwnerId) 
						{
								if (OwnerStrike != null) 
								{
								MyHbox = OwnerStrike.AttackBoxes [(int)AttackQueue];
								HitboxSeed = OwnerStrike.HitComboSeed;
								HitboxData hitboxData = MyHbox.hitboxData;
								hitboxData.ownerID = MyOwnerId;
								HitboxSeed += hitboxData.SeedModifier;
								hitboxData.HboxSeed = HitboxSeed;
								hitboxData.OwnerCollider = OwnerStrike.controller;
										if (OwnerStrike.HitComboSeed < HitboxSeed) 
										{
												OwnerStrike.HitComboSeed = HitboxSeed;
										}
								enemy.SendMessage("ApplyHitbox", hitboxData, SendMessageOptions.DontRequireReceiver); // send damage object to target
								}
						}
				}
		}	
			
}