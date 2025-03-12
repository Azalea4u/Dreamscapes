using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static Scr_LeaderBoard;

public class LeaderSlot : MonoBehaviour {
	[SerializeField] Scr_BoardSlot[] slots;
	[SerializeField] Sprite[] sprites;
    [SerializeField] string fileName;

    string filePath;
	slotList leadSlots = new slotList();

    void Start() {
		filePath = Path.Combine(Application.streamingAssetsPath, fileName + ".txt");
		using (FileStream stream = new FileStream(filePath, FileMode.Open)) {
			using (StreamReader read = new StreamReader(stream)) {
				leadSlots = JsonUtility.FromJson<slotList>(read.ReadToEnd());
			}
		}

		for (int i = 0; i < slots.Length; i++) {
			switch (leadSlots.slot[i].i) {
				case "cute":
					slots[i].changeSlot(sprites[0], leadSlots.slot[i].s, leadSlots.slot[i].n);
					break;
				case "dwindling":
					slots[i].changeSlot(sprites[1], leadSlots.slot[i].s, leadSlots.slot[i].n);
					break;
				case "paint":
					slots[i].changeSlot(sprites[2], leadSlots.slot[i].s, leadSlots.slot[i].n);
					break;
				case "space":
					slots[i].changeSlot(sprites[3], leadSlots.slot[i].s, leadSlots.slot[i].n);
					break;
				case "trippy":
					slots[i].changeSlot(sprites[4], leadSlots.slot[i].s, leadSlots.slot[i].n);
					break;
				case "vintage":
					slots[i].changeSlot(sprites[5], leadSlots.slot[i].s, leadSlots.slot[i].n);
					break;
				default:
					slots[i].changeSlot(sprites[6], leadSlots.slot[i].s, leadSlots.slot[i].n);
					break;
			}
		}
	}
}