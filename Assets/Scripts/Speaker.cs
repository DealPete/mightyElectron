using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : Wire {
	public SpeakerAnimator animator;
	public bool isOn = false;
	public override void DoAction(Agent agent){
		agent.damage (1);
		if (!isOn) {
			isOn = true;
			LED.activeCount += 1;
			animator.animating = true;
			ac.NextStem ();
			ac.playSting ();
		}
	}
}
