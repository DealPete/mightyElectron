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
		Junction j1 = newJunction(new Vector3(25,0));
		Junction j2 = newJunction(new Vector3(20,0));
		hookup (j1, j2, Direction.Left, WireType.LED);
		Junction j3 = newJunction(new Vector3(20, 5));
		hookup (j2, j3, Direction.Up, WireType.Plain);
		Junction j4 = newJunction(new Vector3( 25, 5));
		hookup (j3, j4, Direction.Right, WireType.Resistor);
		Junction j5 = newJunction(new Vector3 (30, 5));
		hookup (j4, j5, Direction.Right,  WireType.Plain);
		Junction j6 = newJunction(new Vector3 (25, 15));
		hookup (j5, j6, Direction.Up, WireType.LED);
		Junction j7 = newJunction(new Vector3 (20,15));
		hookup (j6, j7, Direction.Left, WireType.Plain);
		Junction j8 = newJunction(new Vector3 (20,10));
		hookup (j7, j8, Direction.Down,WireType.Plain);
		hookup (j8, j4, Direction.Down,  WireType.Resistor);
		Junction j9 = newJunction(new Vector3(15, 10));
		hookup(j8,j9,Direction.Left, WireType.Capacitor);
		Junction j10 = newJunction(new Vector3(15, 15));
		hookup(j9,j10,Direction.Up, WireType.Resistor);
		hookup(j7,j10,Direction.Left, WireType.Plain);
		Junction j11 = newJunction (new Vector3 (15, 5));
		hookup (j9, j11, Direction.Down, WireType.Plain);
		Junction j12 = newJunction (new Vector3 (15, 0));
		hookup (j11, j12, Direction.Down, WireType.LED);
		Junction j13 = newJunction (new Vector3 (10, 0));
		hookup (j12, j13, Direction.Left, WireType.Plain);
		Junction j14 = newJunction (new Vector3 (10, 5));
		hookup (j13, j14, Direction.Up, WireType.Resistor);
		Junction j15 = newJunction (new Vector3 (10, 10));
		hookup (j14, j15, Direction.Up, WireType.LED);
		hookup (j15, j9, Direction.Right, WireType.Resistor);
		Junction j16 = newJunction (new Vector3 (5, 10));
		hookup (j15, j16, Direction.Left, WireType.Plain);
		Junction j17 = newJunction (new Vector3 (5, 5));
		hookup (j16, j17, Direction.Down, WireType.Plain);
		Junction j18 = newJunction (new Vector3 (0, 5));
		hookup (j17, j18, Direction.Left, WireType.Resistor);
		Junction j19 = newJunction (new Vector3 (0, 10));
		hookup (j18, j19, Direction.Up, WireType.Capacitor);
		hookup (j16, j19, Direction.Left, WireType.LED);
		Junction j20 = newJunction (new Vector3 (5, 15));
		Junction j21 = newJunction (new Vector3 (10, 15));

		hookup (j16, j20, Direction.Up, WireType.Resistor);
		hookup (j20, j21, Direction.Right, WireType.Capacitor);


		return j20.gameObject;
	}

	void Start () {
		Agent agent = Instantiate(AgentPrefab).GetComponent<Agent>();
		Agents.Add (agent);


		agent.container = Level1 ();
		agent.containerType = ContainerType.Junction;

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

	Direction opposite(Direction d) {
		switch (d) {
		case Direction.Left:
			return Direction.Right;

		case Direction.Right:
			return Direction.Left;

		case Direction.Up:
			return Direction.Down;

		case Direction.Down:
			return Direction.Up;
		default:
			return Direction.NoDirection;
		}
	}

 	Junction newJunction(Vector3 position) {
		Junction junction = Instantiate (JunctionPrefab, position, Quaternion.identity).GetComponent<Junction> ();
		Junctions.Add(junction);
		return junction;
	}

	Wire hookup(Junction source, Junction target, Direction dirToTarget, WireType wireType) {
		Direction dirFromTarget = opposite(dirToTarget);
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

