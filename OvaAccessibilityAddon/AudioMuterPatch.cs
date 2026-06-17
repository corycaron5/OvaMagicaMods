using EggCore.Utils;
using MelonLoader;
using UnityEngine;

namespace OvaAccessibilityAddon;

[RegisterTypeInIl2Cpp]
public class AudioMuterPatch : MonoBehaviour
{
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public AudioMuterPatch(IntPtr ptr) : base(ptr) { }
    
    /// <summary>
    /// Called when the application focus changes. If the mod setting 'Mute on Focus Loss' is enabled, this will mute the audio when the application loses focus.
    /// </summary>
    /// <param name="hasFocus">Whether the application has focus.</param>
    void OnApplicationFocus(bool hasFocus)
    {
        if (Melon<AccessibilityCore>.Instance.MuteOnFocusLoss.Value)
        {
            EggAudioUtils.MuteAudio(!hasFocus);
            EggCore.EggCore.InfoMessage(AccessibilityCore.MelonId, "Audio Muted: " + !hasFocus);
        }
    }
    
}