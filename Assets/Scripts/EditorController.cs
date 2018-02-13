using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Mode {
	Selecting,
	PlacingWire,
	MovingNode
}

public class EditorController : MonoBehaviour {
	private List<Junction> Junctions;
	private List<Wire> Wires;
	private Mode mode = Mode.Selecting;

	public GameObject JunctionPrefab;
	public GameObject SpeakerPrefab;
	public GameObject WirePrefab;
	public SignScript sign;

	// Use this for initialization
	void Start () {
		Junctions = new List<Junction>();
		Wires = new List<Wire>();
		sign.text = "Click somewhere with the mouse";
	}
	
	// Update is called once per frame
	void Update () {
		switch (mode) {
			case Mode.Selecting:
				if (Input.GetButtonDown("Fire1")) {
					Vector3 position = Input.mousePosition;
					Wire wire = newJunction(position);
					sign.text = string.Format("position of mouse is {0}",
						position.ToString());
				}
				break;
			
			case Mode.PlacingWire:
				break;
		}
	}

 	Wire newJunction(Vector3 position) {
		Junction junction = Instantiate (JunctionPrefab, position, Quaternion.identity).GetComponent<Junction> ();
		junction.id = Junctions.Count;
		Junctions.Add(junction);

		Wire wire = Instantiate(WirePrefab).GetComponent<Wire>();
		wire.startNode = junction;
		wire.id = Wires.Count;
		junction.addWire(wire);
		Wires.Add(wire);

		mode = Mode.PlacingWire;
		return wire;
	}
}
