using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {
	public int resistance = 0;
	public Junction startNode;
	public Junction endNode;

	public LineRenderer lineRenderer;

	public float tolerance = 0.2f;
	bool triggered = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		refreshPosition();
	}

	public void refreshPosition(){
		lineRenderer.SetPositions(new Vector3[] {startNode.transform.position, endNode.transform.position});
		this.transform.position = Vector3.Lerp (startNode.transform.position, endNode.transform.position, 0.5f);
	}
	public virtual void updateAgent(Agent agent){
		moveAgent (agent);
		if (!triggered){
			if (agent.wirePosition < 0.5 + tolerance && agent.wirePosition > 0.5 - tolerance) {
				DoAction (agent);
				triggered = true;
			}
		}
		if (checkEndpoints (agent)) {
			triggered = false;
		}
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
	public virtual void DoAction(Agent a){
		return;
	}
}
