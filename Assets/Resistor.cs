using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistor : Wire {
	bool triggered = false;
	public float tolerance = 0.2f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public override void updateAgent(Agent agent){
		base.moveAgent (agent);
		if (!triggered){
			if (agent.wirePosition < 0.5 + tolerance && agent.wirePosition > 0.5 - tolerance) {
				DoAction (agent);
				triggered = true;
			}
		}
		if (base.checkEndpoints (agent)) {
			triggered = false;
		}
	}
	public void DoAction(Agent agent){
		agent.damage (1);
	}
}
