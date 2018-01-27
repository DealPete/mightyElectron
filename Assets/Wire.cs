using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {
	public int startNode;
	public int endNode;

	public LineRenderer lineRenderer;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void setEndpoints(Transform p1, Transform p2){
		lineRenderer.SetPositions(new Vector3[] {p1.position, p2.position});
	}
}
