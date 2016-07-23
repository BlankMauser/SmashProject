using UnityEngine;
using System.Collections;

public class GlowExposer : MonoBehaviour {

	public Color GlowExpose;
	public Color DefaultGlow;
	public bool AnimateGlow;
	public MeshRenderer MyRend;

	void Start () {
		MyRend = this.GetComponent<MeshRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		if (AnimateGlow) {
			MyRend.material.SetColor ("_GlowColor", GlowExpose);
		} else {
			MyRend.material.SetColor ("_GlowColor", DefaultGlow);
		}
}

}