using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour {
	[SerializeField]
	EditorController editorController;

	const int SCENE_GAMEPLAY = 2;

	public void Quit() {
		Application.Quit();
	}

	public void LoadByIndex(int sceneIndex) {
		StageController.playingEditorLevel = false;
		SceneManager.LoadScene(sceneIndex);
	}

	public void TestLevel() {
		editorController.SaveTemporaryLevel();
		StageController.playingEditorLevel = true;
		SceneManager.LoadScene(SCENE_GAMEPLAY);
	}
}
