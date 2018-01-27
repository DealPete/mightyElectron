using System;
using UnityEngine;
using System.Collections.Generic;

public class Junction : MonoBehaviour {
	public Dictionary<Direction, Wire> wires = new Dictionary<Direction,Wire>();
	public void addWire(Direction d, Wire wire){
		wires.Add (d, wire);
	}
	public bool hasWireOnDirection (Direction d){
		return wires.ContainsKey(d);
	}
	public Wire getWire(Direction d) {
		return wires [d];
	}
}