using System;
using UnityEngine;
using System.Collections.Generic;

public class Junction : MonoBehaviour {
	public int id;

	public List<Wire> Wires = new List<Wire>();

	public void addWire(Wire wire) {
		Wires.Add(wire);
	}

	public void removeWire(Wire wire) {
		Wires.Remove(wire);
	}

	public Wire getWireOnDirection(Direction direction) {
		Wire smallestAngleWire = null;
		float smallestAngle = 90.0f;
		Vector3 directionVector;

		switch (direction) {
			case Direction.Up:
			directionVector = Vector3.up;
			break;

			case Direction.Down:
			directionVector = Vector3.down;
			break;

			case Direction.Left:
			directionVector = Vector3.left;
			break;

			case Direction.Right:
			directionVector = Vector3.right;
			break;

			default:
			return null;
		}

		for(int i = 0; i < Wires.Count; i++) {
			Wire wire = Wires[i];
			Vector3 wireDirection = wire.endNode.transform.position
				- wire.startNode.transform.position;
			if (wire.endNode == this)
				wireDirection *= -1;
			float angle = Vector3.Angle(wireDirection, directionVector);
			if (angle < smallestAngle) {
				smallestAngle = angle;
				smallestAngleWire = wire;
			}
		}

		return smallestAngleWire;
	}
}
