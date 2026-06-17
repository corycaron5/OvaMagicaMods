using Il2CppOvaMagica;

namespace EggCore.Utils;

public static class EggAudioUtils
{
    public static void MuteAudio(bool mute)
    {
        AudioLogic audioLogic = GameLogic.Current.audioLogic;
        if (audioLogic == null) return;
        if (audioLogic.audioSourceBgm != null)
        {
            audioLogic.audioSourceBgm.mute = mute;
            EggCore.InfoMessage("BGM Muted: " + mute);
        }
        if (audioLogic.audioSourceSound != null)
        {
            audioLogic.audioSourceSound.mute = mute;
            EggCore.InfoMessage("Sounds Muted: " + mute);
        }
        if (audioLogic.audioSourceBackgrounEffect1 != null)
        {
            audioLogic.audioSourceBackgrounEffect1.mute = mute;
            EggCore.InfoMessage("Background Effects Muted: " + mute);
        }
        /*audioLogic.muteSound = mute;*/
    }
}