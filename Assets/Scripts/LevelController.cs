using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
	[SerializeField]
	protected GameObject AgentPrefab;
	[SerializeField]
	protected GameObject WirePrefab;
	[SerializeField]
	protected GameObject LEDPrefab;
	[SerializeField]
	protected GameObject ResistorPrefab;
	[SerializeField]
	protected GameObject CapacitorPrefab;
	[SerializeField]
	protected GameObject JunctionPrefab;
	[SerializeField]
	protected GameObject SpeakerPrefab;

	public GameLevel gameLevel;
	protected List<Junction> Junctions;
	protected List<Wire> Wires;

 	public Junction addJunction(Vector3 position) {
		return Instantiate (JunctionPrefab, position, Quaternion.identity).GetComponent<Junction> ();
	}

	public Wire hookup(Junction source, Junction target, WireType wireType) {
		GameObject prefab;

		switch (wireType) {
			case WireType.LED:
				prefab = LEDPrefab;
				break;
			case WireType.Resistor:
				prefab = ResistorPrefab;
				break;
			case WireType.Capacitor:
				prefab = CapacitorPrefab;
			break;
			case WireType.Speaker:
				prefab = SpeakerPrefab;
			break;
			default:
			case WireType.Plain:
				prefab = WirePrefab;
				break;
		}
			
		Wire wire = Instantiate(prefab).GetComponent<Wire>();
		wire.wireType = wireType;

		wire.startNode = source;
		wire.endNode = target;

		source.addWire(wire);
		target.addWire(wire);
		
		return wire;
	}
}
