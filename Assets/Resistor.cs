using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistor : Wire {

	public override void DoAction(Agent agent) {
		agent.damage (1);
	}
}
