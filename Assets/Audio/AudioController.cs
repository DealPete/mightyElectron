using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Each musical file consists of 4 stems, each of which can be triggered with NextStem
 */
public class AudioController : MonoBehaviour {
	//setup music files
	public AudioSource[] stems;
	public float volume;
	private int currentStem=0;
	// Use this for initialization
	void Start () {
		foreach (var stem in stems)
		{
			stem.volume = 0;
		}
	}
	
	//Adds the next stem
	public void NextStem(){
		if (currentStem >= stems.Length) return; //already at max
		stems[currentStem].volume=volume;
		currentStem++;
	}

}
