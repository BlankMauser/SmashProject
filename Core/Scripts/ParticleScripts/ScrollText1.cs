using UnityEngine;
using System.Collections;

public class ScrollText1 : MonoBehaviour {
	public float scrollSpeed = 0.5F;
	public Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
	}
	void Update() {
		float offset = Time.time * scrollSpeed;
		rend.material.SetTextureOffset("_EnvMap", new Vector2(offset, offset*-1));
	}
}