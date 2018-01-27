using System;
using UnityEngine;
using System.Collections.Generic;

public class Junction : MonoBehaviour {
	public Dictionary<Direction, int> wires = new Dictionary<Direction,int>();
	public void addWire(Direction d, int wireIndex){
		wires.Add (d, wireIndex);
	}
}