using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour {
	public float freq=1f;
	public float amp=0.1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float val = Mathf.Sin(Time.fixedTime * Mathf.PI  * freq)+ 2;
		val *=amp;
		Vector3 bounce = new Vector3(val,val,val);
		transform.localScale = bounce;
	}
}
