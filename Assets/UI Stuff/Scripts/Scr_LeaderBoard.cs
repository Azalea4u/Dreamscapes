using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class Scr_LeaderBoard : MonoBehaviour {
    [SerializeField] GameObject createPanel;
    [SerializeField] GameObject continueBtn;
    [SerializeField] GameObject againBtn;
    [SerializeField] GameObject loserpanel;
    [SerializeField] Scr_BoardSlot[] slots;
    [SerializeField] Sprite[] sprites;

    int newScore;
    float time = 3;
    bool countDown = false;

    //good luck =)
    [SerializeField] TextAsset info;
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
        saveSlots = JsonUtility.FromJson<slotList>(info.text);

        for (int i = 0; i < slots.Length; i++) {
            slots[i].scoreTxt.text = "" + saveSlots.slot[i].s;
            switch (saveSlots.slot[i].i) {
                case "finn":
                    slots[i].slotImg.sprite = sprites[0];
                    break;
                case "jake":
                    slots[i].slotImg.sprite = sprites[1];
                    break;
                case "bmo":
                    slots[i].slotImg.sprite = sprites[2];
                    break;
                case "bubblegum":
                    slots[i].slotImg.sprite = sprites[3];
                    break;
                default:
                    slots[i].slotImg.sprite = sprites[4];
                    break;
            }
        }

        //these are just tests do not uncomment them
        //loser();
        //highScore();
    }

    void Update() {
        if (countDown) { 
            time -= Time.deltaTime;
            if (time <= 0) {
                loserpanel.SetActive(false);
                continueBtn.SetActive(true);
                againBtn.SetActive(true);
                countDown = false;
                time = 3;
            }
        }
    }

    public void againClick() {
        switch (info.name) {
            case "Archeology":
                SCR_Loader.Load(SCR_Loader.scenes.SCN_ArcheologyMinigame);
                break;
            case "Find":
				SCR_Loader.Load(SCR_Loader.scenes.SCN_FindDragonLuigi);
				break;
            case "Octopus":
				SCR_Loader.Load(SCR_Loader.scenes.SCN_OctopusShooter);
				break;
            case "Rocket":
				SCR_Loader.Load(SCR_Loader.scenes.SCN_SpaceshipScene);
				break;
        }
    }

    public void continueClick() {
        for (int i = 0; i < slots.Length; i++) {
            saveSlots.slot[i].s = int.Parse(slots[i].scoreTxt.text);

            switch (slots[i].slotImg.sprite.name) {
                case "finn_0":
                    saveSlots.slot[i].i = "finn";
                    break;
                case "jake_0":
                    saveSlots.slot[i].i = "jake";
                    break;
                case "bmo_0":
                    saveSlots.slot[i].i = "bmo";
                    break;
                case "bubblegum_0":
                    saveSlots.slot[i].i = "bubblegum";
                    break;
                case "question_0":
                    saveSlots.slot[i].i = "???";
                    break;
            }
        }

        File.WriteAllText(Application.dataPath + "/JSON-Files(Txt)/" + info.name + ".txt", JsonUtility.ToJson(saveSlots));
		AssetDatabase.Refresh();

		SCR_Loader.Load(SCR_Loader.scenes.SCN_MainMenu);
	}

    public void createClick(Image img) {
        createSlot(img, newScore);

        createPanel.SetActive(false);
        continueBtn.SetActive(true);
        againBtn.SetActive(true);
    }

    void createSlot(Image img, int score, int index = 0) {
        //index; doesn't work so I just do this, there's proably a better way...
		for (index = index; index < slots.Length; index++) {
            Image tempImg;
            int tempScore;
			if (int.Parse(slots[index].scoreTxt.text) < score) {
                tempImg = slots[index].slotImg;
                tempScore = int.Parse(slots[index].scoreTxt.text);
                slots[index].changeSlot(img, score);
                createSlot(tempImg, tempScore, index + 1);
                return;
			}
		}
	}

    void highScore() {
        createPanel.SetActive(true);
    }

    void loser() {
        loserpanel.SetActive(true);
        loserpanel.GetComponentInChildren<TextMeshProUGUI>().text = "Your score: " + newScore;
        countDown = true;
    }

    public void endGame(int score) {
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