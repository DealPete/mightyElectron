using System;
using System.IO;
using UnityEngine;

public class Serializer {
	public static void SaveTemporaryLevel(GameLevel gameLevel) {
		BinaryWriter bw;

		try {
			bw = new BinaryWriter(new FileStream("level.tmp", FileMode.Create));
		} catch (IOException e) {
			Debug.Log(e.Message + "Couldn't write to level.tmp!");
			return;
		}

		bw.Write(gameLevel.startJunction);
		bw.Write(gameLevel.Junctions.Count);
		foreach (var junction in gameLevel.Junctions) {
			bw.Write(junction.transform.position.x);
			bw.Write(junction.transform.position.y);
		}
		bw.Write(gameLevel.Wires.Count);
		foreach (var wire in gameLevel.Wires) {
			bw.Write(gameLevel.Wires.FindIndex( w => w == wire.startNode ));
			bw.Write(gameLevel.Wires.FindIndex( w => w == wire.endNode ));
		}

		bw.Close();
	}
}
