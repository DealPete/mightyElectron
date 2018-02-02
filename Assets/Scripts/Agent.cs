using UnityEngine;
using System;

public class Agent : MonoBehaviour {
	public AudioController ac;
	public static int START_ENERGY = 5;
	public GameObject explosion;
	public GameObject sprite;
	public int bearing; //+1 or -1
	public float speed;
	public float wirePosition;
	public ContainerType containerType;
	public GameObject container;
	public Direction MovementIntent;

	public int energy;

	void Start(){
		ac = AudioController.instance;
	}

	public void damage(int d){
		energy -= d;
	}
	public void setEnergy(int e){
		energy = e;
	}
	public void kill(){
		GameObject exp = Instantiate (explosion);
		exp.transform.position = this.transform.position;
		this.transform.position = new Vector3 (100, 100, 0);
		Destroy(sprite);
		ac.onDeath ();
		ac.StopMusic ();

	}
}
