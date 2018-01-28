using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacitor : Wire {
	public bool charged = true;

	public override void DoAction(Agent agent) {
		if (charged) {
			agent.setEnergy (Agent.START_ENERGY);
			charged = false;
		}
	}
}
