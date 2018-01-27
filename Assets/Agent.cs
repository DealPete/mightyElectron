using UnityEngine;
using System;

public class Agent : MonoBehaviour {
	public int bearing; //+1 or -1
	public float speed;
	public float wirePosition;
	public ContainerType containerType;
	public GameObject container;
	public Direction MovementIntent;

	public int energy = 5;
	public void damage(int d){
		energy -= 1;
	}
}