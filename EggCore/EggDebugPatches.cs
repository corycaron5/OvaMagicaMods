using HarmonyLib;
using Il2CppInControl;
using Il2CppOvaMagica;
using MelonLoader;

namespace EggCore;

[HarmonyPatch]
public class EggDebugPatches
{
    /*[HarmonyPatch(typeof(GameLogic), "CheckInputWasPressed")]
    private static class CheckInputPressedPatch
    {
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(GameLogic __instance, ref bool __result, ref IInputControl control)
        {
            if (__result)
            {
                EggCore.DebugMessage("Input was pressed: " + control.WasPressed);
                EggCore.DebugMessage("Input is pressed: " + control.IsPressed);
                EggCore.DebugMessage("Input has changed: " + control.HasChanged);
                EggCore.DebugMessage("Input was released: " + control.WasReleased);
            }
        }
    }*/
}