using UnityEngine;
using System.Collections;

public class ParticleGreedSever : MonoBehaviour {

	//Frames before being destroyed
	public int DestroyTimer;

	public Color glowCol;
	public MeshRenderer MYrend;

	void Start () {
		MYrend = this.GetComponent<MeshRenderer> ();
		MYrend.material.SetColor ("_GlowColor", glowCol);
	}
	
	// Update is called once per frame
	void Update () {
		if (DestroyTimer == 0) {
			Destroy (this.gameObject);
		} else {
			DestroyTimer -= 1;
		}



	}
}
