﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShareMesh : MonoBehaviour { 

		public GameObject target;


		// Use this for initialization 

		void Start () {    
				SkinnedMeshRenderer targetRenderer = target.GetComponent<SkinnedMeshRenderer>();
				Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
				foreach( Transform bone in targetRenderer.bones )
				{
						boneMap[bone.name] = bone;
				}

				SkinnedMeshRenderer thisRenderer = GetComponent<SkinnedMeshRenderer>();
				Transform[] boneArray = thisRenderer.bones;
				for(int idx = 0; idx < boneArray.Length; ++idx )
				{
						string boneName = boneArray[idx].name;
						if( false == boneMap.TryGetValue(boneName, out boneArray[idx]) )
						{
								Debug.LogError("failed to get bone: " + boneName);
								Debug.Break();
						}
				}
				thisRenderer.bones = boneArray; //take effect
		}
} 