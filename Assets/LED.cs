using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : Wire {
	public static int activeCount = 0;
	public bool isOn = false;
	public GameObject onGraphic;
	public GameObject offGraphic;
	//LED images
	public Sprite RedOff;
	public Sprite RedON;
	public Material redglow;
	public Sprite GreenOff;
	public Sprite GreenOn;
	public Material greenglow;
	public Sprite BlueOff;
	public Sprite BlueOn;
	public Material blueglow;
	public override void DoAction(Agent agent){
		agent.damage (1);
		if (!isOn) {
			isOn = true;
			LED.activeCount += 1;

			offGraphic.SetActive(false);
			onGraphic.SetActive(true);
			ac.ledOn ();
		}
	}

	public void SetColor(clr color){
		switch (color)
		{
		case clr.red:
			onGraphic.GetComponent<SpriteRenderer> ().sprite = RedON;
			onGraphic.GetComponent<SpriteRenderer> ().material = redglow;
			offGraphic.GetComponent<SpriteRenderer>().sprite=RedOff;
			break;
			case clr.blue:
			onGraphic.GetComponent<SpriteRenderer>().sprite=BlueOn;
			onGraphic.GetComponent<SpriteRenderer> ().material = blueglow;
			offGraphic.GetComponent<SpriteRenderer>().sprite=BlueOff;
			break;
			case clr.green:
			onGraphic.GetComponent<SpriteRenderer>().sprite=GreenOn;
			onGraphic.GetComponent<SpriteRenderer> ().material = greenglow;
			offGraphic.GetComponent<SpriteRenderer>().sprite=GreenOff;
			break;
		}
	}

	public enum clr
	{
	red,blue,green
	}
}
