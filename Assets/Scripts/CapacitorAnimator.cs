using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacitorAnimator : MonoBehaviour {
	public GameObject[] children;
	public float animationtime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void animate(){
		IEnumerator coroutine = destroyballs ();
		StartCoroutine (coroutine);
	}
	IEnumerator destroyballs(){
		float starttime = Time.time;
		while (Time.time < animationtime + starttime) {
			foreach (GameObject child in children) {
				child.GetComponent<SpriteRenderer> ().material.color -= new Color(0,0,0,(Time.time - starttime)/animationtime);
				Destroy (child);
			}
			yield return null;
		}
		foreach (GameObject child in children) {
			Destroy (child);
		}

	}
}
