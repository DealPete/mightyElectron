using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerAnimator : MonoBehaviour {
	public bool animating = false;
	public Vector3 maxScale = new Vector3 (0.10f, 0.10f, 1.0f);
	public Vector3 minScale = new Vector3 (0.08f, 0.08f, 1.0f);
	public float period;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (animating) {
			this.transform.localScale = Vector3.Lerp ( maxScale,minScale, (Time.time % period)/period);
		}
	}
}
