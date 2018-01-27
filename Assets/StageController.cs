using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour {

	public GameObject AgentPrefab;
	public GameObject WirePrefab;
	public GameObject JunctionPrefab;

	public List<Agent> Agents = new List<Agent>();
	public List<Junction> Junctions = new List<Junction>();
	public List<Wire> Wires = new List<Wire>();

	// Use this for initialization
	void Start () {
		Agent agent = Instantiate(AgentPrefab).GetComponent<Agent>();
		Agents.Add (agent);

		Junction j1 = newJunction(Vector3.right);
		Junction j2 = newJunction(10*Vector3.left);
		Junction j3 = newJunction(4*Vector3.up);

		Wire w1 = hookup(j1, j2, Direction.Left, Direction.Right);
		hookup(j2, j3, Direction.Up, Direction.Left);

		agent.containerType = ContainerType.Wire;
		agent.container = w1.gameObject;

		this.gameObject.GetComponent<PlayerController> ().avatar = agent;
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Agent agent in Agents) {
			if (agent.containerType == ContainerType.Wire) {
				Wire wire = agent.container.GetComponent<Wire>();
				Vector3 startpoint = wire.startNode.transform.position;
				Vector3 endpoint = wire.endNode.transform.position;
				//move the agent along whatever direction it was going already
				agent.wirePosition += (agent.speed * Time.deltaTime * agent.bearing)/Vector3.Distance(startpoint,endpoint);

				//update the world position of agent
				agent.transform.position = Vector3.Lerp (
					startpoint, endpoint, agent.wirePosition);
				//check to see if we hit an endpoint
				if (agent.wirePosition >= 1.0f) {
					agent.containerType = ContainerType.Junction;
					agent.container = wire.endNode.gameObject;
				}
				if (agent.wirePosition <= 0) {
					agent.containerType = ContainerType.Junction;
					agent.container = wire.startNode.gameObject;
				}
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

	public Wire hookup(Junction source, Junction target, Direction dirToTarget, Direction dirFromTarget) {
		Wire wire = Instantiate(WirePrefab).GetComponent<Wire>();

		wire.startNode = source;
		wire.endNode = target;

		source.addWire(dirToTarget, wire);
		target.addWire(dirFromTarget, wire);
		
		wire.refreshPosition();

		return wire;
	}
}

