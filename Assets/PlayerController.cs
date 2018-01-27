using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float inputThreshold;
	public Agent avatar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float horiz = Input.GetAxisRaw ("Horizontal");
		float vert = Input.GetAxisRaw ("Vertical");
		if (Mathf.Abs (horiz) < inputThreshold && Mathf.Abs (vert) < inputThreshold) {
			return;
		}
		if (Mathf.Abs (horiz) > Mathf.Abs (vert)) {
			avatar.MovementIntent = horiz < 0 ? Direction.Left : Direction.Right;
		} else {
			avatar.MovementIntent = vert < 0 ? Direction.Down : Direction.Up;
		}
	}
}
