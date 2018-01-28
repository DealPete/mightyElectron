using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : Wire {
	public AudioController ac;
	public SpeakerAnimator animator;
	public bool isOn = false;
	public void Start(){
		this.ac = AudioController.instance;
	}
	public override void DoAction(Agent agent){
		agent.damage (1);
		if (!isOn) {
			isOn = true;
			LED.activeCount += 1;
			animator.animating = true;
			ac.NextStem ();
		}
	}
}
