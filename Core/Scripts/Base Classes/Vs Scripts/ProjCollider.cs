using UnityEngine;
using System.Collections;

public class ProjCollider : HitboxCollider {

	//Frames before becoming active
	public int StartTimer;

	//Frames before becoming inactive
	public int EndTimer;

	//Frames before being destroyed
	public int DestroyTimer;

	//Size Upon Starting
	public float SimpleSize = 0;

		public ProjCollider()
		{

		}


		public void Start()
		{

		if (OwnerStrike == null) {
			ProjCollider MyRoot = transform.root.GetComponent<ProjCollider> ();
			OwnerStrike = MyRoot.OwnerStrike;
			MyOwnerId = MyRoot.OwnerStrike.MyId;
		}

		}

	public void Update(){
		if (StartTimer == 0 && SimpleSize != 0) {
			Size = SimpleSize;
		} else {
			StartTimer -= 1;
		}

		if (EndTimer == 0) {
			Size = 0;
		} else {
			EndTimer -= 1;
		}

		if (DestroyTimer == 0) {
			Destroy (transform.root.gameObject);
		} else {
			DestroyTimer -= 1;
		}

	}

	public override void OnTriggerDouble(Collider c) {
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
		


}