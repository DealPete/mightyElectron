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
		Wire wire = Instantiate(WirePrefab).GetComponent<Wire>();
		wire.setEndpoints (j1.transform, j2.transform);
		Wires.Add (wire);
		wire.startNode = 0;
		wire.endNode = 1;

		j1.addWire (Direction.Left, 0);
		j2.addWire (Direction.Right, 0);

		agent.containerType = ContainerType.Wire;
		agent.containerIndex = 0;

	}
	
	// Update is called once per frame
	void Update () {
		foreach (Agent agent in Agents) {
			if (agent.containerType == ContainerType.Wire) {
				Wire wire = Wires[agent.containerIndex];
				Vector3 startpoint = Junctions [wire.startNode].transform.position;
				Vector3 endpoint = Junctions [wire.endNode].transform.position;
				//move the agent along whatever direction it was going already
				agent.wirePosition += (agent.speed * Time.deltaTime * agent.bearing)/Vector3.Distance(startpoint,endpoint);

				//update the world position of agent
				agent.transform.position = Vector3.Lerp (
					startpoint, endpoint, agent.wirePosition);
				//check to see if we hit an endpoint
				if (agent.wirePosition >= 1.0f) {
					agent.containerType = ContainerType.Junction;
					agent.containerIndex = wire.endNode;
				}
				if (agent.wirePosition <= 0) {
					agent.containerType = ContainerType.Junction;
					agent.containerIndex = wire.startNode;
				}
			} else {
				Junction junction = Junctions[agent.containerIndex];
				agent.transform.position = junction.transform.position;
			}
		}
	}
}

