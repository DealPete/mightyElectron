using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignScript : MonoBehaviour {
	public string text;
	
	// Update is called once per frame
	void Update () {
		base.GetComponent<Text>().text = text;
	}
}
