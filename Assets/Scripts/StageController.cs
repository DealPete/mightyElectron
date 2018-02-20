using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState {
	PlayingLevel,
	PlayerDead,
	LevelComplete,
	GameComplete
}

public class StageController : MonoBehaviour {
	[SerializeField]
	Prefabs prefabs;

	public static bool playingEditorLevel = false;
	private static int LAST_LEVEL = 3;
	private static int level = 1;
	private GameLevel gameLevel;
	private List<Junction> Junctions;
	private List<Wire> Wires;

	public SignScript sign;
	public AudioController ac;

	public int LEDtotal = 0;

	private List<Agent> Agents;
	private GameState gameState;

	void Start() {
		gameState = GameState.PlayingLevel;
		LED.activeCount = 0;

		Agents = new List<Agent>();

		Agent agent = Instantiate(prefabs.AgentPrefab).GetComponent<Agent>();
		Agents.Add (agent);

		agent.energy = Agent.START_ENERGY;

		if (playingEditorLevel)
			agent.container = setupEditorLevel();
		else {
			Junctions = new List<Junction>();
			Wires = new List<Wire>();
			agent.container = setupLevel(level);
		}

		foreach (Wire wire in Wires) {
			if (wire.wireType == WireType.LED) {
				LEDtotal += 1;
			}
		}

		agent.containerType = ContainerType.Junction;

		this.gameObject.GetComponent<PlayerController> ().avatar = agent;
	}
	
	// Update is called once per frame
	void Update () {
		switch (gameState) {
			case GameState.GameComplete:
				break;

			case GameState.PlayingLevel:
				foreach (Agent agent in Agents) {
					if (agent.energy <= 0) {
						sign.text = "You are out of energy. Press FIRE to restart level.";
						gameState = GameState.PlayerDead;
						agent.kill ();
					} else {
						sign.text = "Energy: " + agent.energy;

						if (agent.containerType == ContainerType.Wire) {
							Wire wire = agent.container.GetComponent<Wire>();
							wire.updateAgent (agent);
						} else {
							Junction junction = agent.container.GetComponent<Junction>();
							agent.transform.position = junction.transform.position;

							Wire wire = junction.getWireOnDirection (agent.MovementIntent);
							if (wire != null) {
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
					if (LAST_LEVEL < level) {
						gameState = GameState.GameComplete;
						sign.text = "LEVEL COMPLETE! Press Fire to continue";
					} else {
						gameState = GameState.LevelComplete;
						sign.text = "You've won! CONGRATULATIONS";
					}
				}
				break;

			case GameState.PlayerDead:
				if (Input.GetButton("Fire1")) {
					restartScene();
				}
				break;

			case GameState.LevelComplete:
				if (Input.GetButton("Fire1")) {
					StageController.level += 1;
					restartScene();
				}
				break;

			default:
				sign.text = "You're in the level editor.";
				break;
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

	Wire oldHookup(Junction source, Junction target, Direction dirToTarget, WireType wireType) {
		GameObject prefab;

		switch (wireType) {
			case WireType.LED:
				prefab = prefabs.LEDPrefab;
				LEDtotal += 1;
				break;
			case WireType.Resistor:
				prefab = prefabs.ResistorPrefab;
				break;
			case WireType.Capacitor:
				prefab = prefabs.CapacitorPrefab;
			break;
			case WireType.Speaker:
				prefab = prefabs.SpeakerPrefab;
				LEDtotal += 1;
			break;
			default:
			case WireType.Plain:
				prefab = prefabs.WirePrefab;
				break;
		}
			
		Wire wire = Instantiate(prefab).GetComponent<Wire>();

		wire.startNode = source;
		wire.endNode = target;

		source.addWire(wire);
		target.addWire(wire);
		
		wire.refreshPosition();
		
		Wires.Add(wire);
		return wire;
	}

	Wire oldHookup(Junction source, Junction target, Direction dirToTarget, WireType wireType, LED.clr color) {
		LED led = (LED)oldHookup (source, target, dirToTarget, wireType);
		led.SetColor (color);
		return led;
	}

	Wire oldHookupResistor(Junction source, Junction target, Direction dirToTarget, int resistance) {
		Resistor wire = (Resistor)oldHookup(source, target, dirToTarget, WireType.Resistor);
		wire.resistance = resistance;
		wire.setSprite(resistance);
		return wire;
	}

	public GameObject setupEditorLevel() {
		gameLevel = Serializer.LoadTemporaryLevel(prefabs);
		Junctions = gameLevel.Junctions;
		Wires = gameLevel.Wires;
		return Junctions[gameLevel.startJunction].gameObject;
	}

	Junction oldAddJunction(Vector3 position) {
		Junction junction = prefabs.newJunction(position);
		Junctions.Add(junction);
		return junction;
	}

	//returns the object on which the player starts
	public GameObject setupLevel(int level) {
		if (level == 1) {
			Junction j1 = oldAddJunction(new Vector3(0,10));
			Junction j2 = oldAddJunction(new Vector3(0,5));
			Junction j3 = oldAddJunction(new Vector3(0,0));
			Junction j4 = oldAddJunction(new Vector3(5,10));
			Junction j5 = oldAddJunction(new Vector3(5,0));
			Junction j6 = oldAddJunction(new Vector3(10,10));
			Junction j7 = oldAddJunction(new Vector3(10,5));
			Junction j8 = oldAddJunction(new Vector3(10,0));
			oldHookup (j1, j2, Direction.Down, WireType.Speaker);
			oldHookup (j2, j3, Direction.Down, WireType.LED, LED.clr.green);
			oldHookup (j1, j4, Direction.Right, WireType.Speaker);
			oldHookupResistor (j3, j5, Direction.Right,2);
			oldHookup (j4, j6, Direction.Right, WireType.Speaker);
			oldHookup (j5, j8, Direction.Right, WireType.Capacitor);
			oldHookup (j6, j7, Direction.Down, WireType.Speaker);
			oldHookupResistor (j7, j8, Direction.Down,2);
			return j1.gameObject;
		} else if (level == 2) {
			Junction j1 = oldAddJunction(new Vector3(0, 20));
			Junction j2 = oldAddJunction(new Vector3(0, 15));
			Junction j3 = oldAddJunction(new Vector3(0, 10));
			Junction j4 = oldAddJunction(new Vector3(0, 5));
			Junction j5 = oldAddJunction(new Vector3(0, 0));
			Junction j6 = oldAddJunction(new Vector3(5, 20));
			Junction j7 = oldAddJunction(new Vector3(5, 10));
			Junction j8 = oldAddJunction(new Vector3(5, 5));
			Junction j10 = oldAddJunction(new Vector3(10, 20));
			Junction j11 = oldAddJunction(new Vector3(10, 15));
			Junction j12 = oldAddJunction(new Vector3(10, 10));
			Junction j13 = oldAddJunction(new Vector3(10, 5));
			Junction j14 = oldAddJunction(new Vector3(10, 0));
			Junction j15 = oldAddJunction(new Vector3(15, 20));
			Junction j16 = oldAddJunction(new Vector3(15, 15));
			Junction j17 = oldAddJunction(new Vector3(15, 10));
			Junction j18 = oldAddJunction(new Vector3(15, 5));
			Junction j19 = oldAddJunction(new Vector3(15, 0));
			Junction j20 = oldAddJunction(new Vector3(20, 20));
			Junction j21 = oldAddJunction(new Vector3(20, 15));
			Junction j22 = oldAddJunction(new Vector3(20, 10));
			Junction j23 = oldAddJunction(new Vector3(20, 5));
			Junction j24 = oldAddJunction(new Vector3(20, 0));

			oldHookup(j1, j6, Direction.Right, WireType.LED, LED.clr.green);
			oldHookup(j1, j2, Direction.Down, WireType.Plain);
			oldHookupResistor(j2, j3, Direction.Down, 1);
			oldHookup(j3, j7, Direction.Right, WireType.Plain);
			oldHookupResistor(j3, j4, Direction.Down, 2);
			oldHookup(j4, j5, Direction.Down, WireType.Plain);
			oldHookup(j5, j14, Direction.Right, WireType.Plain);
			oldHookup(j6, j10, Direction.Right, WireType.Plain);
			oldHookup(j7, j12, Direction.Right, WireType.Speaker);
			oldHookupResistor(j7, j8, Direction.Down, 2);
			oldHookup(j8, j13, Direction.Right, WireType.Plain);
			oldHookupResistor(j10, j15, Direction.Right, 1);
			oldHookup(j10, j11, Direction.Down, WireType.Plain);
			oldHookupResistor(j11, j16, Direction.Right, 2);
			oldHookup(j11, j12, Direction.Down, WireType.Speaker);
			oldHookup(j12, j17, Direction.Right, WireType.Speaker);
			oldHookupResistor(j12, j13, Direction.Down, 3);
			oldHookupResistor(j13, j18, Direction.Right, 1);
			oldHookupResistor(j14, j19, Direction.Right, 3);
			oldHookupResistor(j15, j20, Direction.Right, 1);
			oldHookup(j16, j21, Direction.Right, WireType.Plain);
			oldHookup(j17, j22, Direction.Right, WireType.Plain);
			oldHookup(j18, j23, Direction.Right, WireType.Plain);
			oldHookup(j19, j24, Direction.Right, WireType.Speaker);
			oldHookupResistor(j20, j21, Direction.Down, 1);
			oldHookupResistor(j21, j22, Direction.Down, 2);
			oldHookup(j22, j23, Direction.Down, WireType.Capacitor);
			oldHookup(j23, j24, Direction.Down, WireType.Plain);

			return j12.gameObject;
		} else if (level == 3) {
			//First Column
			Junction j1 = oldAddJunction (new Vector3 (0, 15));
			Junction j2 = oldAddJunction (new Vector3 (0, 10));
			oldHookup (j1, j2, Direction.Down, WireType.Plain);
			Junction j3 = oldAddJunction (new Vector3 (0, 5));
			oldHookup (j2, j3, Direction.Down, WireType.Plain);
			Junction j4 = oldAddJunction (new Vector3 (0, 0));
			oldHookup (j3, j4, Direction.Down, WireType.Plain);
			//Second Column
			Junction j5 = oldAddJunction (new Vector3 (5, 20));
			Junction j6 = oldAddJunction (new Vector3 (5, 15));
			oldHookup (j5, j6, Direction.Down, WireType.Speaker);
			oldHookup (j1, j6, Direction.Right, WireType.Plain);
			Junction j7 = oldAddJunction (new Vector3 (5, 5));
			oldHookup (j3, j7, Direction.Right, WireType.Capacitor);
			Junction j8 = oldAddJunction (new Vector3 (5, 0));
			oldHookupResistor (j4, j8, Direction.Right, 3);
			//Third Column
			Junction j9 = oldAddJunction (new Vector3 (10, 20));
			oldHookup (j5, j9, Direction.Right, WireType.Speaker);
			Junction j10 = oldAddJunction (new Vector3 (10, 15));
			oldHookupResistor (j9, j10, Direction.Down, 1);
			oldHookupResistor (j6, j10, Direction.Right, 1);
			Junction j11 = oldAddJunction (new Vector3 (10, 10));
			oldHookup (j10, j11, Direction.Down, WireType.Capacitor);
			Junction j12 = oldAddJunction (new Vector3 (10, 5));
			oldHookup (j11, j12, Direction.Down, WireType.LED, LED.clr.green);
			oldHookupResistor (j7, j12, Direction.Right, 1);
			Junction j13 = oldAddJunction (new Vector3 (10, 0));
			oldHookup (j8, j13, Direction.Right, WireType.Plain);
			//Fourth Column
			Junction j14 = oldAddJunction (new Vector3 (15, 15));
			oldHookupResistor (j10, j14, Direction.Right, 1);
			Junction j15 = oldAddJunction (new Vector3 (15, 5));
			oldHookup (j12, j15, Direction.Right, WireType.Plain);
			Junction j16 = oldAddJunction (new Vector3 (15, 0));
			oldHookup (j13, j16, Direction.Right, WireType.Plain);
			//Fifth Column
			Junction j17 = oldAddJunction (new Vector3 (20, 15));
			oldHookup (j14, j17, Direction.Right, WireType.LED, LED.clr.red);
			Junction j18 = oldAddJunction (new Vector3 (20, 10));
			Junction j19 = oldAddJunction (new Vector3 (20, 5));
			oldHookupResistor (j18, j19, Direction.Down, 2);
			oldHookupResistor (j15, j19, Direction.Right, 2);			
			Junction j20 = oldAddJunction (new Vector3 (20, 0));
			oldHookupResistor (j19, j20, Direction.Down, 1);
			oldHookup (j16, j20, Direction.Right, WireType.Capacitor);
			//Sixth Column
			Junction j21 = oldAddJunction (new Vector3 (25, 15));
			oldHookup (j17, j21, Direction.Right, WireType.Plain);
			Junction j22 = oldAddJunction (new Vector3 (25, 10));
			oldHookup (j21, j22, Direction.Down, WireType.Plain);
			oldHookupResistor (j18, j22, Direction.Right, 3);
			Junction j23 = oldAddJunction (new Vector3 (25, 5));
			oldHookup (j22, j23, Direction.Down, WireType.Plain);
			oldHookupResistor (j19, j23, Direction.Right, 2);
			Junction j24 = oldAddJunction (new Vector3 (25, 0));
			oldHookup (j23, j24, Direction.Down, WireType.Speaker);
			oldHookup (j20, j24, Direction.Right, WireType.Speaker);
			//Seventh Column
			Junction j25 = oldAddJunction (new Vector3 (30, 5));
			oldHookupResistor (j23, j25, Direction.Right, 1);
			Junction j26 = oldAddJunction (new Vector3 (30, 0));
			oldHookup (j25, j26, Direction.Down, WireType.Plain);
			oldHookup (j24, j26, Direction.Right, WireType.LED, LED.clr.blue);
			
			return j19.gameObject;
		} else {
			Junction junc = oldAddJunction (new Vector3 (10, 15));
			return junc.gameObject;
		}	
	}
}

