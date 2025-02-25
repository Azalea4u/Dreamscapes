using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System.Collections;

public class Scr_LeaderBoard : MonoBehaviour {
    [SerializeField] GameObject createPanel;
    [SerializeField] GameObject continueBtn;
    [SerializeField] GameObject againBtn;
    [SerializeField] GameObject loserBtn;
    [SerializeField] GameObject loserpanel;
    [SerializeField] Scr_BoardSlot[] slots;
    [SerializeField] Sprite[] sprites;

    int newScore;

    //good luck
    [SerializeField] string fileName;
    string filePath;

    [System.Serializable]
    class slotSlot {
        public string i;
        public int s;
    }

    [System.Serializable]
    class slotList {
        public slotSlot[] slot;
    }
    slotList saveSlots = new slotList();

    void Start() {
        //Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Boards", fileName + ".txt"));
        //Directory.CreateDirectory(filePath);
        //print(Application.streamingAssetsPath);
        //print(Application.persistentDataPath);

        filePath = Path.Combine(Application.streamingAssetsPath, fileName + ".txt");
        using (FileStream stream = new FileStream(filePath, FileMode.Open)) {
            using (StreamReader read = new StreamReader(stream)) {
                saveSlots = JsonUtility.FromJson<slotList>(read.ReadToEnd());
            }
        }
        
        //saveSlots = JsonUtility.FromJson<slotList>(filePath);

        for (int i = 0; i < slots.Length; i++) {
            slots[i].scoreTxt.text = "" + saveSlots.slot[i].s;
            switch (saveSlots.slot[i].i) {
                case "cute":
                    slots[i].slotImg.sprite = sprites[0];
                    break;
                case "dwindling":
                    slots[i].slotImg.sprite = sprites[1];
                    break;
                case "paint":
                    slots[i].slotImg.sprite = sprites[2];
                    break;
                case "space":
                    slots[i].slotImg.sprite = sprites[3];
                    break;
				case "trippy":
					slots[i].slotImg.sprite = sprites[4];
					break;
				case "vintage":
					slots[i].slotImg.sprite = sprites[5];
					break;
				default:
                    slots[i].slotImg.sprite = sprites[6];
                    break;
            }
        }

        //these are just tests do not uncomment them
        //loser();
        //highScore();
    }

	void saveStuff() {
		for (int i = 0; i < slots.Length; i++) {
			saveSlots.slot[i].s = int.Parse(slots[i].scoreTxt.text);

			switch (slots[i].slotImg.sprite.name) {
				case "CuteDoor_0":
					saveSlots.slot[i].i = "cute";
					break;
				case "DwindlingDoor_0":
					saveSlots.slot[i].i = "dwindling";
					break;
				case "PaintDoor_0":
					saveSlots.slot[i].i = "paint";
					break;
				case "SpaceDoor_0":
					saveSlots.slot[i].i = "space";
					break;
				case "TrippyDoor_0":
					saveSlots.slot[i].i = "trippy";
					break;
				case "VintageDoor_0":
					saveSlots.slot[i].i = "vintage";
					break;
                case "question_0":
                    saveSlots.slot[i].i = "???";
                    break;
			}
		}

        using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
            using (StreamWriter write = new StreamWriter(stream)) {
                write.Write(JsonUtility.ToJson(saveSlots, true));
            }
        }

        //File.WriteAllText(Application.dataPath + "/StreamingAssets/" + fileName + ".txt", JsonUtility.ToJson(saveSlots));
        //File.WriteAllText(Application.dataPath + "/JSON-Files(Txt)/" + info.name + ".txt", JsonUtility.ToJson(saveSlots));
        //AssetDatabase.Refresh();
    }

    public void continueClick() {
        saveStuff();
        
        GameManager.instance.PauseGame(false);
        SRC_AudioManager.instance.ChangeSceneWithMusic(SCR_Loader.scenes.SCN_MainMenu, "MainTheme_Music");
	}

    public void againClick() {
        saveStuff();

        switch (fileName) {
            case "Archeology":
                Scr_ScreenManager.instance.ArcheologyClick();
				break;
            case "Find":
                Scr_ScreenManager.instance.DragonClick();
				break;
            case "Octopus":
                Scr_ScreenManager.instance.OctopusClick();
				break;
            case "Rocket":
                Scr_ScreenManager.instance.RocketClick();
				break;
        }
    }

    public void loserQuitClick() {
		loserpanel.SetActive(false);
        loserBtn.SetActive(false);
		continueBtn.SetActive(true);
		againBtn.SetActive(true);
	}

    public void createClick(Image img) {
        createSlot(img.sprite, newScore);

        createPanel.SetActive(false);
        continueBtn.SetActive(true);
		againBtn.SetActive(true);
	}

    //oh yeah keyboard...
    public void letterClick(TextMeshProUGUI letter) {
        switch (letter.text) {
            case "Enter":
                break;
            case "Backspace":
                break;
            default:
                break;
        }
    }

    //leaderboard sorting is here
    void createSlot(Sprite img, int score, int index = 0) {
        //index; doesn't work so I just do this, theres probably an easier way...
		for (index = index; index < slots.Length; index++) {
            Sprite tempSprite;
            int tempScore;

			if (int.Parse(slots[index].scoreTxt.text) < score) {
                tempSprite = slots[index].slotImg.sprite;
				tempScore = int.Parse(slots[index].scoreTxt.text);

                switch (img.name) {
                    case "CuteDoor_0":
                        slots[index].changeSlot(sprites[0], score);
                        break;
                    case "DwindlingDoor_0":
                        slots[index].changeSlot(sprites[1], score);
                        break;
                    case "PaintDoor_0":
                        slots[index].changeSlot(sprites[2], score);
                        break;
                    case "SpaceDoor_0":
                        slots[index].changeSlot(sprites[3], score);
                        break;
                    case "TrippyDoor_0":
                        slots[index].changeSlot(sprites[4], score);
                        break;
                    case "VintageDoor_0":
                        slots[index].changeSlot(sprites[5], score);
                        break;
                }

                //slots[index].changeSlot(img, score);

                createSlot(tempSprite, tempScore, index + 1);
                return;
			}
		}
	}

    void highScore() {
        createPanel.SetActive(true);
        continueBtn.SetActive(false);
        againBtn.SetActive(false);
    }

    void loser() {
        loserpanel.SetActive(true);
        loserpanel.GetComponentInChildren<TextMeshProUGUI>().text = "Your score: " + newScore;
        loserBtn.SetActive(true);
    }

    public void endGame(int score) {
        Start();

        newScore = score;
        foreach (Scr_BoardSlot slot in slots) {
            if (int.Parse(slot.scoreTxt.text) < score) {
                highScore();
                return;
            }
        }

        loser();
    }
}