using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistor : Wire {
	public SpriteRenderer renderer;
	public List<Sprite> sprites;

	public override void DoAction(Agent agent) {
		agent.damage (1);
	}

	public void setSprite (int resistance){
		renderer.sprite = sprites[resistance+1];
	}


}
