using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : Wire {
	public bool isOn = false;

	public override void DoAction(Agent agent){
		agent.damage (1);
		isOn = true;
	}
}
