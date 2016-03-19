using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class ManualEmitFromSkinnedMesh : MonoBehaviour {

		public Transform skinnedMeshTransform;
		public float emitRepeatTime = .5f;
		public int vertexStep = 1;
		public Color32 particleColor = Color.white;
		public Vector3 particleVelocity;

		private PlaygroundParticlesC particles;
		private SkinnedWorldObject swo;

		void Start () {

				// Cache the particle system (make sure you have set enough particle count)
				particles = GetComponent<PlaygroundParticlesC>();

				// Create a new skinned world object
				swo = PlaygroundC.SkinnedWorldObject(skinnedMeshTransform);

				// Start emission routine
				StartCoroutine(EmitOverAllVertices());
		}

		IEnumerator EmitOverAllVertices () {
				while (true)
				{
						yield return new WaitForSeconds(emitRepeatTime);
						swo.BoneUpdate();
						swo.UpdateOnNewThread();
						while (!swo.isDoneThread)
								yield return null;
						for (int i = 0; i<swo.vertexPositions.Length; i+=vertexStep)
								particles.Emit(swo.vertexPositions[i], particleVelocity, particleColor);
				}
		}
}
