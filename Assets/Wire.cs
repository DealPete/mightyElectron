using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {
	public AudioController ac;
	public int resistance = 0;
	public Junction startNode;
	public Junction endNode;

	public LineRenderer lineRenderer;

	public float tolerance = 0.2f;
	bool triggered = false;
	
	// Update is called once per frame
	public void Start(){
		this.ac = AudioController.instance;
	}
	void Update () {
		refreshPosition();
	}

	public void refreshPosition(){
		Vector3 startpos = startNode.transform.position;
		Vector3 endpos = endNode.transform.position;

		lineRenderer.SetPositions(new Vector3[] {startpos, endpos});
		this.transform.position = Vector3.Lerp (startpos,endpos, 0.5f);
		this.transform.rotation = Quaternion.FromToRotation (Vector3.left, startpos - endpos);


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
