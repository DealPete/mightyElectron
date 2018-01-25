using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
	float speed = 5.0f;

	// Update is called once per frame
	void Update () {
		float vertical = Time.deltaTime * Input.GetAxis("Vertical") * speed;
		float horizontal = Time.deltaTime * Input.GetAxis("Horizontal") * speed;

		transform.Translate(horizontal, vertical, 0);
	}
}
