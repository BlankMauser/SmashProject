using UnityEngine;
using System.Collections;

public class ParticleExposer : MonoBehaviour {

	ParticleSystem MyParticle;

	public bool IsPlaying;
	// Use this for initialization
	void Start () {
		MyParticle = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (IsPlaying) {
			if (!MyParticle.isPlaying) {
				MyParticle.Play ();
			}
		} else {
			if (MyParticle.isPlaying) {
				MyParticle.Stop ();
			}
		}
	}
}
