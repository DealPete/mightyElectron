using UnityEngine;
using System;

public class Agent : MonoBehaviour {
	public static int START_ENERGY = 5;

	public int bearing; //+1 or -1
	public float speed;
	public float wirePosition;
	public ContainerType containerType;
	public GameObject container;
	public Direction MovementIntent;

	public Wire lastWire;
	public int energy;

	public void damage(int d){
		energy -= 1;
	}
	public void setEnergy(int e){
		energy = e;
	}
}
