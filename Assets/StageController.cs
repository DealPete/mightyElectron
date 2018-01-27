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
		Junction j1 = Instantiate (JunctionPrefab, Vector3.right, Quaternion.identity).GetComponent<Junction> ();
		Junctions.Add (j1);
		Junction j2 = Instantiate (JunctionPrefab, 10*Vector3.left, Quaternion.identity).GetComponent<Junction> ();
		Junctions.Add (j2);
		Junction j3 = Instantiate (JunctionPrefab, 4*Vector3.up, Quaternion.identity).GetComponent<Junction> ();
		Junctions.Add (j3);

		Wire w1 = Instantiate(WirePrefab).GetComponent<Wire>();
		Wires.Add (w1);
		Wire w2 = Instantiate(WirePrefab).GetComponent<Wire>();
		Wires.Add (w2);

		w1.startNode = j1;
		w1.endNode = j2;
		w1.refreshPosition ();
		w2.startNode = j2;
		w2.endNode = j3;
		w2.refreshPosition ();

		j1.addWire (Direction.Left, w1);
		j2.addWire (Direction.Right, w1);

		j2.addWire (Direction.Up, w2);
		j3.addWire (Direction.Left, w2);


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
}

