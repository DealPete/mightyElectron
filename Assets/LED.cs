using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : Wire {
	public static int activeCount = 0;
	public bool isOn = false;
	public GameObject onGraphic;
	public GameObject offGraphic;
	public override void DoAction(Agent agent){
		agent.damage (1);
		if (!isOn) {
			isOn = true;
			LED.activeCount += 1;

			offGraphic.SetActive(false);
			onGraphic.SetActive(true);
		}
	}
}
