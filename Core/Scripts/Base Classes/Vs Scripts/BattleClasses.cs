﻿using UnityEngine;
using System.Collections;

public enum HitboxType { Melee, Bullet, Hurtbox, Wind }

[System.Serializable]
public class DamageData {
    
    public float amount;
	public float knockback_y;
	public float knockback_x;
    public DamageType type;
	public GameObject effect;
	public float stun;
	public float blockstun;
    
	public string soundname;
	public bool Xflipped;
	public int ownerID;
	public Vector3 effectspawn;

	public Color cStart;
	public Color cEnd;

    public DamageData() {}
    
    public DamageData(DamageData damageData) { // clone
        CopyData(damageData, this);
    }
    
    private void CopyData(DamageData source, DamageData destination) {
		destination.soundname = source.soundname;
		destination.cStart = source.cStart;
		destination.cEnd = source.cEnd;
		destination.ownerID = source.ownerID;
		destination.Xflipped = source.Xflipped;
		destination.effectspawn = source.effectspawn;
        destination.amount = source.amount;
        destination.type = source.type;
		destination.knockback_y = source.knockback_y;
		destination.knockback_x = source.knockback_x;
		destination.effect = source.effect;
		destination.stun = source.stun;
		destination.blockstun = source.blockstun;
    }

    public DamageData Clone() {
        return new DamageData(this);
    }
}

[System.Serializable]
public class HitboxData {

		public float Damage;
		public float TrueDamage;
		public int BaseKnockback;
		public int WeightKnockback;
		public int KnockbackGrowth;
		public int Hitlag;
		public int Blockstun;
		public int Hitstun;
		public HitboxType type;
		public GameObject effect;

		public string soundname;
		public bool Xflipped;
		public int ownerID;
		public Vector3 effectspawn;

		public Color cStart;
		public Color cEnd;

		public HitboxData() {}

		public HitboxData(HitboxData hitboxData) { // clone
				CopyData(hitboxData, this);
		}

		private void CopyData(HitboxData source, HitboxData destination) {
				destination.soundname = source.soundname;
				destination.cStart = source.cStart;
				destination.cEnd = source.cEnd;
				destination.ownerID = source.ownerID;
				destination.Xflipped = source.Xflipped;
				destination.effectspawn = source.effectspawn;
				destination.Damage = source.Damage;
				destination.TrueDamage = source.TrueDamage;
				destination.type = source.type;
				destination.BaseKnockback = source.BaseKnockback;
				destination.WeightKnockback = source.WeightKnockback;
				destination.KnockbackGrowth = source.KnockbackGrowth;
				destination.effect = source.effect;
				destination.Hitlag = source.Hitlag;
				destination.Blockstun = source.Blockstun;
				destination.Hitstun = source.Hitstun;
		}

		public HitboxData Clone() {
				return new HitboxData(this);
		}
}

