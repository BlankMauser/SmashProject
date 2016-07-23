using UnityEngine;
using System.Collections;

public enum HitboxType { Melee, Bullet, Ungraze, Hybrid, Wind, Throw }
public enum Reversible { Normal, Forward, Reverse}
public enum Cardinals { 

	Right = 0,
	UpRight = 1,
	Up = 2,
	UpLeft = 3,
	Left = 4,
	DownLeft = 5,
	Down = 6,
	DownRight = 7,
	Center = 8,
}

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

		public RayCastColliders OwnerCollider;
		public bool SetKnockback;
		public int MyPriority;
		public float Damage;
		public float TrueDamage;
		public int HboxSeed;
		public int SeedModifier;
		public int Direction;
		public Cardinals OptimalDI;
		public Reversible Reversible;
		public float BaseKnockback;
		public float WeightKnockback;
		public float KnockbackGrowth;
		public int Hitlag;
		public int HitlagDisparity;
		public int Blockstun;
		public int ShieldDamage;
		public float ShieldPush;
		public int Hitstun;
		public int HitstunScaling;
		public float AirThreshhold;
		public HitboxType type;
		public GameObject effect;

		public Vector3 BulletCenter;
		public bool Xflipped;
		public bool IsGrab;
		public int ownerID;
		public Vector3 effectspawn;

		public Color cStart;
		public Color cEnd;

		public HitboxData() {}

		public HitboxData(HitboxData hitboxData) { // clone
				CopyData(hitboxData, this);
		}

		private void CopyData(HitboxData source, HitboxData destination) {
				destination.OwnerCollider = source.OwnerCollider;
				destination.SetKnockback = source.SetKnockback;
				destination.MyPriority = source.MyPriority;
				destination.HboxSeed = source.HboxSeed;
				destination.SeedModifier = source.SeedModifier;
				destination.BulletCenter = source.BulletCenter;
				destination.cStart = source.cStart;
				destination.cEnd = source.cEnd;
				destination.ownerID = source.ownerID;
				destination.Xflipped = source.Xflipped;
				destination.IsGrab = source.IsGrab;
				destination.effectspawn = source.effectspawn;
				destination.Damage = source.Damage;
				destination.TrueDamage = source.TrueDamage;
				destination.type = source.type;
				destination.Direction = source.Direction;
				destination.OptimalDI = source.OptimalDI;
				destination.Reversible = source.Reversible;
				destination.BaseKnockback = source.BaseKnockback;
				destination.WeightKnockback = source.WeightKnockback;
				destination.KnockbackGrowth = source.KnockbackGrowth;
				destination.effect = source.effect;
				destination.Hitlag = source.Hitlag;
				destination.HitlagDisparity = source.HitlagDisparity;
				destination.Blockstun = source.Blockstun;
				destination.ShieldPush = source.ShieldPush;
				destination.Hitstun = source.Hitstun;
				destination.HitstunScaling = source.HitstunScaling;
				destination.AirThreshhold = source.AirThreshhold;
		}

		public HitboxData Clone() {
				return new HitboxData(this);
		}
}


