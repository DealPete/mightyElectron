using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {
	public Junction startNode;
	public Junction endNode;

	public LineRenderer lineRenderer;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void refreshPosition(){
		lineRenderer.SetPositions(new Vector3[] {startNode.transform.position, endNode.transform.position});
	}
	public virtual void updateAgent(Agent agent){
		moveAgent (agent);
		checkEndpoints (agent);
	}
	public void moveAgent(Agent agent){
		Vector3 startpoint = this.startNode.transform.position;
		Vector3 endpoint = this.endNode.transform.position;
		//move the agent along whatever direction it was going already
		agent.wirePosition += (agent.speed * Time.deltaTime * agent.bearing) / Vector3.Distance (startpoint, endpoint);

		//update the world position of agent
		agent.transform.position = Vector3.Lerp (
			startpoint, endpoint, agent.wirePosition);
		checkEndpoints (agent);
	}
	public bool checkEndpoints (Agent agent) {
		if (agent.wirePosition >= 1.0f) {
			agent.containerType = ContainerType.Junction;
			agent.container = this.endNode.gameObject;
			return true;
		}
		if (agent.wirePosition <= 0) {
			agent.containerType = ContainerType.Junction;
			agent.container = this.startNode.gameObject;
			return true;
		}
		return false;
	}
}
