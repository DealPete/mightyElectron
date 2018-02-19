using UnityEngine;
using System.Collections.Generic;

public class GameLevel {
	public int startJunction;
	public List<Junction> Junctions;
	public List<Wire> Wires;
	
	public GameLevel() {
		startJunction = 0;
		Junctions = new List<Junction>();
		Wires = new List<Wire>();
	}

	public void remove(Wire wire) {
		Wires.Remove(wire);
		wire.startNode.removeWire(wire);
		if (wire.startNode.Wires.Count == 0) {
			remove(wire.startNode);
		}
		wire.endNode.removeWire(wire);
		if (wire.endNode.Wires.Count == 0) {
			remove(wire.endNode);
		}
		Object.Destroy(wire.gameObject);
	}

	public void remove(Junction junction) {
		Junctions.Remove(junction);
		Object.Destroy(junction.gameObject);
	}
}
