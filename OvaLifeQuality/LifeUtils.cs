using EggCore.Utils;
using Il2CppOvaMagica;
using MelonLoader;

namespace OvaLifeQuality;

public static class LifeUtils
{
    /// <summary>
    /// Returns the cost reduction for a given tool type.
    /// </summary>
    /// <param name="toolType">The tool type to get the cost reduction for.</param>
    /// <returns>The cost reduction for the given tool type.</returns>
    public static int GetCostReduction(PlayerTool toolType)
    {
        if (toolType == null) return 0;
        if (toolType.itemData.itemKey.Contains("Pickaxe"))return Melon<LifeCore>.Instance.PickaxeActionCostReduction.Value;
        if (toolType.itemData.itemKey.Contains("Axe"))return Melon<LifeCore>.Instance.AxeActionCostReduction.Value;
        if (toolType.itemData.itemKey.Contains("Shovel"))return Melon<LifeCore>.Instance.ShovelActionCostReduction.Value;
        if (toolType.itemData.itemKey.Contains("Hoe"))return Melon<LifeCore>.Instance.HoeActionCostReduction.Value;
        if (toolType.itemData.itemKey.Contains("Net"))return Melon<LifeCore>.Instance.BugNetActionCostReduction.Value;
        if (toolType.itemData.itemKey.Contains("Pole"))return Melon<LifeCore>.Instance.FishingActionCostReduction.Value;
        if (toolType.itemData.itemKey.Contains("Seed"))return Melon<LifeCore>.Instance.PlantActionCostReduction.Value;
        if (toolType.itemData.itemKey.Contains("Can"))return Melon<LifeCore>.Instance.WaterActionCostReduction.Value;
        return 0;
    }
    
    public const string EnergyModId = "LifeQualityEnergyMod";
    
    /// <summary>
    /// Updates the player's maximum energy based on the current value of MaxEnergyMod.
    /// </summary>
    /// <remarks>
    /// This method checks if the current maximum energy modifier is different from the previously stored value.
    /// If it is, the player's maximum energy is updated accordingly.
    /// The value of the modifier is stored in a GameVariable with key <see cref="EnergyModId"/>.
    /// </remarks>
    public static void EnergyMod()
    {
        if (GameData.current == null) return;
        if (GameData.current.playerData == null) return;
        int newMod = Melon<LifeCore>.Instance.MaxEnergyMod.Value;
        int oldMod = EggGameplayUtils.GetGameVariableValue(EnergyModId);
        if (oldMod != newMod)
        {
            GameData.current.playerData.maxEnergy += newMod - oldMod;
        }
        EggGameplayUtils.SetGameVariable(EnergyModId, newMod);
    }
    
    /// <summary>
    /// Sets the dialog text speed based on the value in the config
    /// </summary>
    public static void SetTextSpeed()
    {
        if (Melon<LifeCore>.Instance.DialogTextSpeed.Value <= 0) return;
        DialogLogic dialog = DialogLogic.current;
        if(dialog == null) return;
        dialog.typeWriterEffect.charactersPerSecond *= Melon<LifeCore>.Instance.DialogTextSpeed.Value;
        EggCore.EggCore.InfoMessage(LifeCore.MelonId, "Dialog Text Speed: " + dialog.typeWriterEffect.charactersPerSecond);
    }
}