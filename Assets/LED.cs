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
	public Sprite GreenOff;
	public Sprite GreenOn;
	public Sprite BlueOff;
	public Sprite BlueOn;
	public override void DoAction(Agent agent){
		agent.damage (1);
		if (!isOn) {
			isOn = true;
			LED.activeCount += 1;

			offGraphic.SetActive(false);
			onGraphic.SetActive(true);
		}
	}

	public void SetColor(clr color){
		switch (color)
		{
			case clr.red:
			onGraphic.GetComponent<SpriteRenderer>().sprite=RedON;
			offGraphic.GetComponent<SpriteRenderer>().sprite=RedOff;
			break;
			case clr.blue:
			onGraphic.GetComponent<SpriteRenderer>().sprite=BlueOn;
			offGraphic.GetComponent<SpriteRenderer>().sprite=BlueOff;
			break;
			case clr.green:
			onGraphic.GetComponent<SpriteRenderer>().sprite=GreenOn;
			offGraphic.GetComponent<SpriteRenderer>().sprite=GreenOff;
			break;
		}
	}

	public enum clr
	{
	red,blue,green
	}
}
