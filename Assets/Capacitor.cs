using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacitor : Wire {
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	public override void DoAction(Agent agent){
		agent.setEnergy (5);
	}
}
