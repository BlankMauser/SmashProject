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
		public bool Interp;

		public Vector3 PrevPosition;
		public Vector3 NewPosition;

		public void Start()
		{
				MyOwnerId = OwnerStrike.MyId;
		}

		public void Update()
		{
		PrevPosition = transform.position;
		}
		

		public void LateUpdate()
		{
				foreach(var c in  Physics.OverlapSphere(transform.position, Size)) 
				{
			if (Size > 0) {
				this.gameObject.SendMessage ("OnTriggerDouble", c);
			}
				}

		if (Interp) {
			RaycastHit[] sphereHit = Physics.SphereCastAll(transform.position, Size, (PrevPosition - transform.position).normalized, Vector3.Distance(PrevPosition, transform.position));
				foreach(RaycastHit hit in sphereHit) 
			{
				if (Size > 0) {
					this.gameObject.SendMessage ("OnTriggerDouble", hit.collider);
				}
			}
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

		public virtual void OnTriggerDouble(Collider c) {
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

//Raycast
//								RaycastHit spark;
//								Vector3 fwd = (c.transform.position - transform.position).normalized;
//								Physics.Raycast (transform.position, fwd, out spark, Size, 1 << 20);
//					if (spark.point == Vector3.zero) {
//						hitboxData.effectspawn = c.transform.position;
//					} else {
//						hitboxData.effectspawn = spark.point;
//					}

					hitboxData.effectspawn = transform.position + (Size* (c.transform.position - transform.position).normalized);

										if (OwnerStrike.HitComboSeed < HitboxSeed) 
										{
												OwnerStrike.HitComboSeed = HitboxSeed;
										}
								enemy.SendMessage("ApplyHitbox", hitboxData, SendMessageOptions.DontRequireReceiver); // send damage object to target
								}
						}
				}
		}

	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, 0.25f);
		Gizmos.DrawSphere(transform.position, Size);
	}
			
}