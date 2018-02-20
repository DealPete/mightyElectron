using UnityEngine;
using System.Collections.Generic;

public class GameLevel {
	Prefabs prefabs;
	public int startJunction;
	public List<Junction> Junctions;
	public List<Wire> Wires;
	
	public GameLevel(Prefabs prefabsSingleton) {
		prefabs = prefabsSingleton;
		startJunction = 0;
		Junctions = new List<Junction>();
		Wires = new List<Wire>();
	}

 	public Junction addJunction(Vector3 position) {
		Junction junction = prefabs.newJunction(position);
		Junctions.Add(junction);
		return junction;
	}

	public void hookup(Junction source, Junction target, WireType wireType) {
		GameObject prefab;

		switch (wireType) {
			case WireType.LED:
				prefab = prefabs.LEDPrefab;
				break;
			case WireType.Resistor:
				prefab = prefabs.ResistorPrefab;
				break;
			case WireType.Capacitor:
				prefab = prefabs.CapacitorPrefab;
			break;
			case WireType.Speaker:
				prefab = prefabs.SpeakerPrefab;
			break;
			default:
			case WireType.Plain:
				prefab = prefabs.WirePrefab;
				break;
		}
			
		Wire wire = prefabs.newWire(prefab);
		wire.wireType = wireType;

		wire.startNode = source;
		wire.endNode = target;

		source.addWire(wire);
		target.addWire(wire);

		Wires.Add(wire);
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
