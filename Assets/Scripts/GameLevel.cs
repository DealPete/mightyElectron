using System.Collections.Generic;

public class GameLevel {
	public int startJunction;
	public List<Junction> Junctions;
	public List<Wire> Wires;
	
	public GameLevel() {
		startJunction = 0;
		Junctions = new List<Junction>();
		Wires = new List<Wire>();
	}
}
