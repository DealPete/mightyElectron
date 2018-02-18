using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WireButton : MonoBehaviour {
	[SerializeField]
	EditorController editorController;
	[SerializeField]
	WireType wireType;


	public void SelectWire(){
		editorController.SelectWireType (wireType);
	}

}
