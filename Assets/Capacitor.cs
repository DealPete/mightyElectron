using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacitor : Wire {
	public override void DoAction(Agent agent){
		agent.setEnergy (5);
	}
}
