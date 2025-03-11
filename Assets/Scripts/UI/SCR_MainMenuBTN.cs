using UnityEngine;

public class SCR_MainMenuBTN : MonoBehaviour
{
    
    public void SpaceShip_BTN()
    {
        Scr_ScreenManager.instance.RocketClick();
    }

    public void Archeology_BTN()
    {
        Scr_ScreenManager.instance.ArcheologyClick();
    }

    public void Finding_BTN()
    {
        Scr_ScreenManager.instance.DragonClick();
    }

    public void Octopus_BTN()
    {
        Scr_ScreenManager.instance.OctopusClick();
    }

    public void Mutebutton()
    {
        SRC_AudioManager.instance.playing = SRC_AudioManager.instance.PauseMusic(SRC_AudioManager.instance.playing);
    }
}
