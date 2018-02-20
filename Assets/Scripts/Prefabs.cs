using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour {
	[SerializeField]
	public GameObject AgentPrefab;
	[SerializeField]
	public GameObject WirePrefab;
	[SerializeField]
	public GameObject LEDPrefab;
	[SerializeField]
	public GameObject ResistorPrefab;
	[SerializeField]
	public GameObject CapacitorPrefab;
	[SerializeField]
	public GameObject JunctionPrefab;
	[SerializeField]
	public GameObject SpeakerPrefab;

	public Wire newWire(GameObject prefab) {
		return Instantiate(prefab).GetComponent<Wire>();
	}

	public Junction newJunction(Vector3 position) {
		return Instantiate(JunctionPrefab, position, Quaternion.identity).GetComponent<Junction>();
	}
}
