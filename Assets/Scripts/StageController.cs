using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour {
	static int LAST_LEVEL = 3;
	public AudioController ac;
	static int level = 1;

	public SignScript sign;

	public GameObject AgentPrefab;
	public GameObject WirePrefab;
	public GameObject LEDPrefab;
	public GameObject ResistorPrefab;
	public GameObject CapacitorPrefab;
	public GameObject JunctionPrefab;
	public GameObject SpeakerPrefab;

	bool playerJustDied = false;
	bool levelDone = false;
	public int LEDtotal = 0;
	public List<Agent> Agents;
	public List<Junction> Junctions;
	public List<Wire> Wires;

	void Start() {
		LED.activeCount = 0;

		Agents = new List<Agent>();
		Junctions = new List<Junction>();
		Wires = new List<Wire>();

		Agent agent = Instantiate(AgentPrefab).GetComponent<Agent>();
		Agents.Add (agent);

		agent.energy = Agent.START_ENERGY;

		agent.container = setupLevel(level);
		agent.containerType = ContainerType.Junction;

		this.gameObject.GetComponent<PlayerController> ().avatar = agent;
	}
	
	// Update is called once per frame
	void Update () {
		if (!levelDone) {
			foreach (Agent agent in Agents) {
				if (agent.energy <= 0) {
					sign.text = "You are out of energy. Press FIRE to restart level.";
					levelDone = true;
					playerJustDied = true;
					agent.kill ();
				} else {
					sign.text = "Energy: " + agent.energy;

					if (agent.containerType == ContainerType.Wire) {
						Wire wire = agent.container.GetComponent<Wire>();
						wire.updateAgent (agent);
					} else {
						Junction junction = agent.container.GetComponent<Junction>();
						agent.transform.position = junction.transform.position;

						if (junction.hasWireOnDirection (agent.MovementIntent)) {
							Wire wire = junction.getWire (agent.MovementIntent);
							agent.containerType = ContainerType.Wire;
							agent.container = wire.gameObject;
							if (wire.endNode == junction) {
								agent.bearing = -1;
								agent.wirePosition = 1.0f;
							} else { //hopefully it was at the other end
								agent.bearing = 1;
								agent.wirePosition = 0;
							}
						}
					}
				}
			}

			if (LED.activeCount == LEDtotal) {
				sign.text = "LEVEL COMPLETE! Press Fire to continue";
				levelDone = true;
				playerJustDied = false;
			}
			if (LAST_LEVEL < level) {
				sign.text = "You've won! CONGRATULATIONS";
			}
		} else {
			if (Input.GetButton("Fire1")) {
				if (!playerJustDied) {
					StageController.level += 1;
				} 
				restartScene();
			}
		}
	}

	void restartScene() {
		string currentSceneName = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(currentSceneName);
	}

	Direction opposite(Direction d) {
		switch (d) {
		case Direction.Left:
			return Direction.Right;

		case Direction.Right:
			return Direction.Left;

		case Direction.Up:
			return Direction.Down;

		case Direction.Down:
			return Direction.Up;
		default:
			return Direction.NoDirection;
		}
	}

 	Junction newJunction(Vector3 position) {
		Junction junction = Instantiate (JunctionPrefab, position, Quaternion.identity).GetComponent<Junction> ();
		Junctions.Add(junction);
		return junction;
	}

	Wire hookup(Junction source, Junction target, Direction dirToTarget, WireType wireType) {
		Direction dirFromTarget = opposite(dirToTarget);
		GameObject prefab;

		switch (wireType) {
			case WireType.LED:
				prefab = LEDPrefab;
				LEDtotal += 1;
				break;
			case WireType.Resistor:
				prefab = ResistorPrefab;
				break;
			case WireType.Capacitor:
				prefab = CapacitorPrefab;
			break;
			case WireType.Speaker:
				prefab = SpeakerPrefab;
				LEDtotal += 1;
			break;
			default:
			case WireType.Plain:
				prefab = WirePrefab;
				break;
		}
			
		Wire wire = Instantiate(prefab).GetComponent<Wire>();

		wire.startNode = source;
		wire.endNode = target;

		source.addWire(dirToTarget, wire);
		target.addWire(dirFromTarget, wire);
		
		wire.refreshPosition();
		
		Wires.Add(wire);
		return wire;
	}

	Wire hookup(Junction source, Junction target, Direction dirToTarget, WireType wireType, LED.clr color) {
		LED led = (LED)hookup (source, target, dirToTarget, wireType);
		led.SetColor (color);
		return led;
	}

	Wire hookupResistor(Junction source, Junction target, Direction dirToTarget, int resistance) {
		Resistor wire = (Resistor)hookup(source, target, dirToTarget, WireType.Resistor);
		wire.resistance = resistance;
		wire.setSprite(resistance);
		return wire;
	}

	//returns the object on which the player starts
	public GameObject setupLevel(int level) {
		if (level == 1) {
			Junction j1 = newJunction(new Vector3(0,10));
			Junction j2 = newJunction(new Vector3(0,5));
			Junction j3 = newJunction(new Vector3(0,0));
			Junction j4 = newJunction(new Vector3(5,10));
			Junction j5 = newJunction(new Vector3(5,0));
			Junction j6 = newJunction(new Vector3(10,10));
			Junction j7 = newJunction(new Vector3(10,5));
			Junction j8 = newJunction(new Vector3(10,0));
			hookup (j1, j2, Direction.Down, WireType.Speaker);
			hookup (j2, j3, Direction.Down, WireType.LED, LED.clr.green);
			hookup (j1, j4, Direction.Right, WireType.Speaker);
			hookupResistor (j3, j5, Direction.Right,2);
			hookup (j4, j6, Direction.Right, WireType.Speaker);
			hookup (j5, j8, Direction.Right, WireType.Capacitor);
			hookup (j6, j7, Direction.Down, WireType.Speaker);
			hookupResistor (j7, j8, Direction.Down,2);
			return j1.gameObject;
		} else if (level == 2) {
			Junction j1 = newJunction(new Vector3(0, 20));
			Junction j2 = newJunction(new Vector3(0, 15));
			Junction j3 = newJunction(new Vector3(0, 10));
			Junction j4 = newJunction(new Vector3(0, 5));
			Junction j5 = newJunction(new Vector3(0, 0));
			Junction j6 = newJunction(new Vector3(5, 20));
			Junction j7 = newJunction(new Vector3(5, 10));
			Junction j8 = newJunction(new Vector3(5, 5));
			Junction j10 = newJunction(new Vector3(10, 20));
			Junction j11 = newJunction(new Vector3(10, 15));
			Junction j12 = newJunction(new Vector3(10, 10));
			Junction j13 = newJunction(new Vector3(10, 5));
			Junction j14 = newJunction(new Vector3(10, 0));
			Junction j15 = newJunction(new Vector3(15, 20));
			Junction j16 = newJunction(new Vector3(15, 15));
			Junction j17 = newJunction(new Vector3(15, 10));
			Junction j18 = newJunction(new Vector3(15, 5));
			Junction j19 = newJunction(new Vector3(15, 0));
			Junction j20 = newJunction(new Vector3(20, 20));
			Junction j21 = newJunction(new Vector3(20, 15));
			Junction j22 = newJunction(new Vector3(20, 10));
			Junction j23 = newJunction(new Vector3(20, 5));
			Junction j24 = newJunction(new Vector3(20, 0));

			hookup(j1, j6, Direction.Right, WireType.LED, LED.clr.green);
			hookup(j1, j2, Direction.Down, WireType.Plain);
			hookupResistor(j2, j3, Direction.Down, 1);
			hookup(j3, j7, Direction.Right, WireType.Plain);
			hookupResistor(j3, j4, Direction.Down, 2);
			hookup(j4, j5, Direction.Down, WireType.Plain);
			hookup(j5, j14, Direction.Right, WireType.Plain);
			hookup(j6, j10, Direction.Right, WireType.Plain);
			hookup(j7, j12, Direction.Right, WireType.Speaker);
			hookupResistor(j7, j8, Direction.Down, 2);
			hookup(j8, j13, Direction.Right, WireType.Plain);
			hookupResistor(j10, j15, Direction.Right, 1);
			hookup(j10, j11, Direction.Down, WireType.Plain);
			hookupResistor(j11, j16, Direction.Right, 2);
			hookup(j11, j12, Direction.Down, WireType.Speaker);
			hookup(j12, j17, Direction.Right, WireType.Speaker);
			hookupResistor(j12, j13, Direction.Down, 3);
			hookupResistor(j13, j18, Direction.Right, 1);
			hookupResistor(j14, j19, Direction.Right, 3);
			hookupResistor(j15, j20, Direction.Right, 1);
			hookup(j16, j21, Direction.Right, WireType.Plain);
			hookup(j17, j22, Direction.Right, WireType.Plain);
			hookup(j18, j23, Direction.Right, WireType.Plain);
			hookup(j19, j24, Direction.Right, WireType.Speaker);
			hookupResistor(j20, j21, Direction.Down, 1);
			hookupResistor(j21, j22, Direction.Down, 2);
			hookup(j22, j23, Direction.Down, WireType.Capacitor);
			hookup(j23, j24, Direction.Down, WireType.Plain);

			return j12.gameObject;
		} else if (level == 3) {
			//First Column
			Junction j1 = newJunction (new Vector3 (0, 15));
			Junction j2 = newJunction (new Vector3 (0, 10));
			hookup (j1, j2, Direction.Down, WireType.Plain);
			Junction j3 = newJunction (new Vector3 (0, 5));
			hookup (j2, j3, Direction.Down, WireType.Plain);
			Junction j4 = newJunction (new Vector3 (0, 0));
			hookup (j3, j4, Direction.Down, WireType.Plain);
			//Second Column
			Junction j5 = newJunction (new Vector3 (5, 20));
			Junction j6 = newJunction (new Vector3 (5, 15));
			hookup (j5, j6, Direction.Down, WireType.Speaker);
			hookup (j1, j6, Direction.Right, WireType.Plain);
			Junction j7 = newJunction (new Vector3 (5, 5));
			hookup (j3, j7, Direction.Right, WireType.Capacitor);
			Junction j8 = newJunction (new Vector3 (5, 0));
			hookupResistor (j4, j8, Direction.Right, 3);
			//Third Column
			Junction j9 = newJunction (new Vector3 (10, 20));
			hookup (j5, j9, Direction.Right, WireType.Speaker);
			Junction j10 = newJunction (new Vector3 (10, 15));
			hookupResistor (j9, j10, Direction.Down, 1);
			hookupResistor (j6, j10, Direction.Right, 1);
			Junction j11 = newJunction (new Vector3 (10, 10));
			hookup (j10, j11, Direction.Down, WireType.Capacitor);
			Junction j12 = newJunction (new Vector3 (10, 5));
			hookup (j11, j12, Direction.Down, WireType.LED, LED.clr.green);
			hookupResistor (j7, j12, Direction.Right, 1);
			Junction j13 = newJunction (new Vector3 (10, 0));
			hookup (j8, j13, Direction.Right, WireType.Plain);
			//Fourth Column
			Junction j14 = newJunction (new Vector3 (15, 15));
			hookupResistor (j10, j14, Direction.Right, 1);
			Junction j15 = newJunction (new Vector3 (15, 5));
			hookup (j12, j15, Direction.Right, WireType.Plain);
			Junction j16 = newJunction (new Vector3 (15, 0));
			hookup (j13, j16, Direction.Right, WireType.Plain);
			//Fifth Column
			Junction j17 = newJunction (new Vector3 (20, 15));
			hookup (j14, j17, Direction.Right, WireType.LED, LED.clr.red);
			Junction j18 = newJunction (new Vector3 (20, 10));
			Junction j19 = newJunction (new Vector3 (20, 5));
			hookupResistor (j18, j19, Direction.Down, 2);
			hookupResistor (j15, j19, Direction.Right, 2);			
			Junction j20 = newJunction (new Vector3 (20, 0));
			hookupResistor (j19, j20, Direction.Down, 1);
			hookup (j16, j20, Direction.Right, WireType.Capacitor);
			//Sixth Column
			Junction j21 = newJunction (new Vector3 (25, 15));
			hookup (j17, j21, Direction.Right, WireType.Plain);
			Junction j22 = newJunction (new Vector3 (25, 10));
			hookup (j21, j22, Direction.Down, WireType.Plain);
			hookupResistor (j18, j22, Direction.Right, 3);
			Junction j23 = newJunction (new Vector3 (25, 5));
			hookup (j22, j23, Direction.Down, WireType.Plain);
			hookupResistor (j19, j23, Direction.Right, 2);
			Junction j24 = newJunction (new Vector3 (25, 0));
			hookup (j23, j24, Direction.Down, WireType.Speaker);
			hookup (j20, j24, Direction.Right, WireType.Speaker);
			//Seventh Column
			Junction j25 = newJunction (new Vector3 (30, 5));
			hookupResistor (j23, j25, Direction.Right, 1);
			Junction j26 = newJunction (new Vector3 (30, 0));
			hookup (j25, j26, Direction.Down, WireType.Plain);
			hookup (j24, j26, Direction.Right, WireType.LED, LED.clr.blue);
			
			return j19.gameObject;
		} else {
			Junction junc = newJunction (new Vector3 (10, 15));
			return junc.gameObject;
		}	
	}
}

