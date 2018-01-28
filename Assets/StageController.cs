using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour {

	public GameObject AgentPrefab;
	public GameObject WirePrefab;
	public GameObject LEDPrefab;
	public GameObject ResistorPrefab;
	public GameObject CapacitorPrefab;
	public GameObject JunctionPrefab;

	public List<Agent> Agents = new List<Agent>();
	public List<Junction> Junctions = new List<Junction>();
	public List<Wire> Wires = new List<Wire>();

	//returns the object on which the player starts
	public GameObject Level1 () {
		Junction j1 = newJunction(new Vector3(0,0));
		Junction j2 = newJunction(new Vector3(-10,0));
		Junction j3 = newJunction(new Vector3(-10, 5));
		Junction j4 = newJunction(new Vector3( -5, 5));
		Junction j5 = newJunction(new Vector3 (0, 5));

		Wire w1 = hookup(j1, j2, Direction.Left, Direction.Right, WireType.Plain);
		hookupResistor(j2, j3, Direction.Up, Direction.Down, 3);
		hookup (j3, j4, Direction.Right, Direction.Left, WireType.LED);
		hookup (j4, j5, Direction.Right, Direction.Left, WireType.Plain);
		hookup (j5, j1, Direction.Down, Direction.Up, WireType.Capacitor);
		return w1.gameObject;
	}

	void Start () {
		Agent agent = Instantiate(AgentPrefab).GetComponent<Agent>();
		Agents.Add (agent);


		agent.container = Level1 ();
		agent.containerType = ContainerType.Wire;

		this.gameObject.GetComponent<PlayerController> ().avatar = agent;
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Agent agent in Agents) {
			if (agent.containerType == ContainerType.Wire) {
				Wire wire = agent.container.GetComponent<Wire>();
				wire.updateAgent (agent);
			} else {
				Junction junction = agent.container.GetComponent<Junction>();
				agent.transform.position = junction.transform.position;

				if (junction.hasWireOnDirection (agent.MovementIntent)) {
					Wire wire = junction.getWire (agent.MovementIntent);
					agent.containerType = ContainerType.Wire;
					agent.container = wire.gameObject;
					if (wire.endNode == junction) {
						agent.bearing = -1;
						agent.wirePosition = 1.0f;
					} else { //hopefully it was at the other end
						agent.bearing = 1;
						agent.wirePosition = 0;
					}
				}
			}
		}
	}

 	Junction newJunction(Vector3 position) {
		Junction junction = Instantiate (JunctionPrefab, position, Quaternion.identity).GetComponent<Junction> ();
		Junctions.Add(junction);
		return junction;
	}

	Wire hookup(Junction source, Junction target, Direction dirToTarget, Direction dirFromTarget, WireType wireType) {
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
			default:
			case WireType.Plain:
				prefab = WirePrefab;
				break;
		}
			
		Wire wire = Instantiate(prefab).GetComponent<Wire>();

		wire.startNode = source;
		wire.endNode = target;

		source.addWire(dirToTarget, wire);
		target.addWire(dirFromTarget, wire);
		
		wire.refreshPosition();
		
		Wires.Add(wire);
		return wire;
	}

	Wire hookupResistor(Junction source, Junction target, Direction dirToTarget, Direction dirFromTarget, int resistance) {
		Wire wire = hookup(source, target, dirToTarget, dirFromTarget, WireType.Resistor);
		wire.resistance = resistance;
		return wire;
	}
}

