using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {
	public float amp = 0.5f;
	private List<Junction> junctions;
	private Vector2[] positions;
	// Use this for initialization
	void Start () {
		junctions = GetComponent<StageController>().Junctions;
		positions = new Vector2[junctions.Count];
		Debug.Log("Size of ");
		Debug.Log(junctions.Count);
		for (int i = 0; i < junctions.Count; i++)
		{
			positions[i] = junctions[i].transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.O)){
			Debug.Log("Calling Shake");
			junctions = GetComponent<StageController>().Junctions;
			for (int i = 0; i < junctions.Count-1; i++)
			{
				positions[i] = junctions[i].transform.position;
			}
			StartCoroutine(Shake(3));
		}
	}

	IEnumerator Shake(float time){
		Debug.Log("Shake Called");
		//set time
		float startTime = Time.time;
		if (Time.time - startTime < time/2){
			foreach (var node in junctions)
			{
				Debug.Log("shaking");
				Vector3 shake = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0) * amp * Time.deltaTime;
				node.transform.Translate(shake);
				yield return null;
			}
		}
		if (Time.time - startTime < time){
			for (int i = 0; i < junctions.Count-1; i++)
			{
				Debug.Log(i);
				positions[i] = junctions[i].transform.position;
			}
			//todo calculate how to move back
		}
		Debug.Log("Done");
	}
}
