using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SCR_Loader {
	private static scenes TargetScene;

	public enum scenes {
		// Add scenes by scene name such as:
		Loading,
		//Title,
		//Intro,
		//FirstFloor
	}

	public static void Load(scenes targetScene) {
		SCR_Loader.TargetScene = targetScene;

		SceneManager.LoadScene(SCR_Loader.scenes.Loading.ToString());
	}

	public static void LoaderCallback() {
		SceneManager.LoadScene(SCR_Loader.TargetScene.ToString());
	}
}