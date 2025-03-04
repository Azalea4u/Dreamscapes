using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System.Collections;
using System;

public class Scr_LeaderBoard : MonoBehaviour {
    [SerializeField] GameObject createPanel;
    [SerializeField] GameObject namePanel;
    [SerializeField] GameObject continueBtn;
    [SerializeField] GameObject againBtn;
    [SerializeField] GameObject loserBtn;
    [SerializeField] GameObject loserpanel;
    [SerializeField] Scr_BoardSlot[] slots;
    [SerializeField] Sprite[] sprites;
    [SerializeField] TextMeshProUGUI[] letters;

    int newScore;
    int actScore;
    string newName;
    bool reverse;

    [SerializeField] string fileName;
    string filePath;

    [System.Serializable]
    public class slotSlot {
        public string i;
        public int s;
        public string n;
    }

    [System.Serializable]
    public class slotList {
        public slotSlot[] slot;
    }
    slotList saveSlots = new slotList();

    void Start() {
        reverse = false;

        //get file path, read it, and convert it to saveSlots  
        filePath = Path.Combine(Application.streamingAssetsPath, fileName + ".txt");
        using (FileStream stream = new FileStream(filePath, FileMode.Open)) {
            using (StreamReader read = new StreamReader(stream)) {
                saveSlots = JsonUtility.FromJson<slotList>(read.ReadToEnd());
            }
        }

        //based on saveSlots information set the leaderboard slots
        for (int i = 0; i < slots.Length; i++) {
            slots[i].scoreTxt.text = "" + saveSlots.slot[i].s;
            slots[i].nameTxt.text = saveSlots.slot[i].n;

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
    }

	void saveStuff() {
        //grab info from leaderboard slots and set save slots
		for (int i = 0; i < slots.Length; i++) {

            actScore = 0;
            if (slots[i].scoreTxt.text.Contains('s'))
            {
                actScore = int.Parse(slots[i].scoreTxt.text.TrimEnd('s'));
            }
            else {
                actScore = int.Parse(slots[i].scoreTxt.text);
            
            }

			saveSlots.slot[i].s = actScore;
            saveSlots.slot[i].n = slots[i].nameTxt.text;

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

        //then write to the file using saveSlots
        using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
            using (StreamWriter write = new StreamWriter(stream)) {
                write.Write(JsonUtility.ToJson(saveSlots, true));
            }
        }
    }

    public void continueClick() {
        GameManager.instance.PauseGame(false);
        SRC_AudioManager.instance.ChangeSceneWithMusic(SCR_Loader.scenes.SCN_MainMenu, "MainTheme_Music");
	}

    public void againClick() {
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
        if (reverse) {
            createSlotReverse(img.sprite, newScore, newName);
        } else {
            createSlot(img.sprite, newScore, newName);
        }

        createPanel.SetActive(false);
        continueBtn.SetActive(true);
		againBtn.SetActive(true);

        saveStuff();
	}
    
    public void doneClick() {
        string result = "";

        foreach (TextMeshProUGUI t in letters) {
            result += t.text;
        }

        newName = result;
        namePanel.SetActive(false);
        createPanel.SetActive(true);
    }

    public void letterUpClick(TextMeshProUGUI letter) {
        letterUpDown(letter, true);
    }

	public void letterDownClick(TextMeshProUGUI letter) {
        letterUpDown(letter, false);
	}

    void letterUpDown(TextMeshProUGUI letter, bool up) {
        int cn = Convert.ToChar(letter.text);

        char c = up ? (cn == 33) ? '~' : (char)(cn - 1) : (cn == 126) ? '!' : (char)(cn + 1);

        letter.text = c.ToString();
    }

	//leaderboard sorting is here, messed up sorting and "recursion" logic here
	void createSlot(Sprite img, int score, string name, int index = 0) {
        //index; doesn't work so I just do this, theres probably an easier way...
		for (index = index; index < slots.Length; index++) {
            Sprite tempSprite;
            int tempScore;
            string tempName;

			if (int.Parse(slots[index].scoreTxt.text) < score) {
                tempSprite = slots[index].slotImg.sprite;
				tempScore = int.Parse(slots[index].scoreTxt.text);
                tempName = slots[index].nameTxt.text;

                switch (img.name) {
                    case "CuteDoor_0":
                        slots[index].changeSlot(sprites[0], score, name);
                        break;
                    case "DwindlingDoor_0":
                        slots[index].changeSlot(sprites[1], score, name);
                        break;
                    case "PaintDoor_0":
                        slots[index].changeSlot(sprites[2], score, name);
                        break;
                    case "SpaceDoor_0":
                        slots[index].changeSlot(sprites[3], score, name);
                        break;
                    case "TrippyDoor_0":
                        slots[index].changeSlot(sprites[4], score, name);
                        break;
                    case "VintageDoor_0":
                        slots[index].changeSlot(sprites[5], score, name);
                        break;
                }

                createSlot(tempSprite, tempScore, tempName, index + 1);
                return;
			}
		}
	}

	void createSlotReverse(Sprite img, int score, string name, int index = 0) {
		//index; doesn't work so I just do this, theres probably an easier way...
		for (index = index; index < slots.Length; index++) {
			Sprite tempSprite;
			int tempScore;
			string tempName;
            actScore = 0;

            if (slots[index].scoreTxt.text.Contains('s'))
            {
                actScore = int.Parse(slots[index].scoreTxt.text.TrimEnd('s'));
            }
            else {
                actScore = int.Parse(slots[index].scoreTxt.text);
            
            }

			if ((actScore > score) 
                || (actScore == 0)) {
				tempSprite = slots[index].slotImg.sprite;
                tempScore = actScore;
				tempName = slots[index].nameTxt.text;

				switch (img.name) {
					case "CuteDoor_0":
						slots[index].changeSlotTime(sprites[0], score, name);
						break;
					case "DwindlingDoor_0":
						slots[index].changeSlotTime(sprites[1], score, name);
						break;
					case "PaintDoor_0":
						slots[index].changeSlotTime(sprites[2], score, name);
						break;
					case "SpaceDoor_0":
						slots[index].changeSlotTime(sprites[3], score, name);
						break;
					case "TrippyDoor_0":
						slots[index].changeSlotTime(sprites[4], score, name);
						break;
					case "VintageDoor_0":
						slots[index].changeSlotTime(sprites[5], score, name);
						break;
				}

				createSlotReverse(tempSprite, tempScore, tempName, index + 1);
				return;
			}
		}
	}

	void highScore() {
        //createPanel.SetActive(true);
        namePanel.SetActive(true);
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

	public void endGameReverse(int score) {
		Start();

		newScore = score;
		foreach (Scr_BoardSlot slot in slots) {
            actScore = 0;
            if (slot.scoreTxt.text.Contains('s'))
            {
                actScore = int.Parse(slot.scoreTxt.text.TrimEnd('s'));
            }
            else {
                actScore = int.Parse(slot.scoreTxt.text);
            
            }
			if ((actScore > score) || (actScore == 0)) {
                reverse = true;
				highScore();
				return;
			}
		}

		loser();
	}
}