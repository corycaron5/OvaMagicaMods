using EggCore.Utils;
using MelonLoader;
using UnityEngine;

namespace OvaAccessibilityAddon;

[RegisterTypeInIl2Cpp]
public class GamePauserPatch : MonoBehaviour
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public GamePauserPatch(IntPtr ptr) : base(ptr) { }
    
    private static bool _isPaused = false;
    
    /// <summary>
    /// Called when the application focus changes. If the mod setting 'Pause on Focus Loss' is enabled, this will pause the game when the application loses focus.
    /// </summary>
    /// <param name="hasFocus">Whether the application has focus.</param>
    void OnApplicationFocus(bool hasFocus)
    {
        if (Melon<AccessibilityCore>.Instance.PauseOnFocusLoss.Value)
        {
            if (!hasFocus && EggTimeUtils.IsGamePaused()) return;
            if (hasFocus && !_isPaused) return;
            _isPaused = !hasFocus;
            EggTimeUtils.PauseGame(!hasFocus);
            EggCore.EggCore.InfoMessage(AccessibilityCore.MelonId, "Game Paused: " + !hasFocus);
        }
    }
}