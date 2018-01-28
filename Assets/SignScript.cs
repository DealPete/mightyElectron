using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignScript : MonoBehaviour {
	public int energy;
	public bool levelDone = false;
	
	// Update is called once per frame
	void Update () {
		string text;
		if (levelDone) {
			text = "LEVEL COMPLETE!!";
		} else {
			text = "Energy: " + energy;
		}
			
		base.GetComponent<Text>().text = text;
	}
}
