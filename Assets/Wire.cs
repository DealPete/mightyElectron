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
}
