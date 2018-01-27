using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Each musical file consists of 4 stems, each of which can be triggered with NextStem
 */
public class AudioController : MonoBehaviour {
	//BGM 
	public AudioSource[] stems;
	public float musicVolume;
	private int currentStem=0;
	//Stings
	public AudioSource stingPlayer;
	public AudioClip[] stings;
	public float stingVolume;

	void Start () {
		//mute all audio tracks (turned up in nextStem)
		foreach (var stem in stems)
		{
			stem.volume = 0;
		}
		stingPlayer.volume = stingVolume;
	}

	public void Update(){

		if (Input.GetKeyDown(KeyCode.U)){
			PlaySting();
		}

		if (Input.GetKeyDown(KeyCode.I)){
			NextStem();
		}
	}

	/// <summary>
	/// Turns up the volume on the next BG music track (does nothing if all tracks are playing)
	/// </summary>
	public void NextStem(){
		if (currentStem >= stems.Length) return; //already at max
		stems[currentStem].volume=musicVolume;
		currentStem++;
	}

	/// <summary>
	/// Plays a random audio sting
	/// </summary>
	public void PlaySting(){
		int sel = Random.Range(0,stings.Length);
		// stingPlayer.clip = stings[sel];
		stingPlayer.PlayOneShot(stings[sel]);
	}

}
