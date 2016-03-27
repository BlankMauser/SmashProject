using UnityEngine;
using UnityEditor;

public class HitboxAsset
{
	[MenuItem("Assets/Create/Hitbox")]
	public static void CreateAsset ()
	{
				ScriptableObjectUtility.CreateAsset<Hbox> ();
	}
}