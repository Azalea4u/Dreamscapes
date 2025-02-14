using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SCR_Loader {

	private static scenes TargetScene;

	public enum scenes {
		// Add scenes by scene name such as:
		SCN_MainMenu,
		SCN_Loading,
        SCN_ArcheologyMinigame,
        SCN_SpaceshipScene,
        SCN_FindDragonLuigi,
		SCN_OctopusShooter
    }

	public static void Load(scenes targetScene) {
		SCR_Loader.TargetScene = targetScene;

		SceneManager.LoadScene(SCR_Loader.scenes.SCN_Loading.ToString());
	}

    public static void ContinueToTargetScene()
    {
        SceneManager.LoadScene(TargetScene.ToString());

        if (SRC_AudioManager.instance != null)
        {
            SRC_AudioManager.instance.OnSceneLoaded();
        }
    }
}