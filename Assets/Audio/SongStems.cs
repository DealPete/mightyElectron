using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SongStems", menuName = "CustomAudio", order = 1)]
public class SongStems : ScriptableObject {

	public List<AudioClip> stems = new List<AudioClip>(4);

	// Use this for initialization
	void Start () {
		
	}
}
