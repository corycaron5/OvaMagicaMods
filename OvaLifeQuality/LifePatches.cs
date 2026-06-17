// ReSharper disable InconsistentNaming
namespace OvaLifeQuality;

using EggCore.Utils;
using Il2CppOvaMagica;
using MelonLoader;
using UnityEngine;
using HarmonyLib;

[HarmonyPatch]
public class LifePatches
{
    /// <summary>
    /// Enables building from Charoi if the player has the items in their inventory + magic storage rather than just the inventory
    /// </summary>
    [HarmonyPatch(typeof(UIBuildingStoreButton), "SetBuilding")]
    private static class BuildingHasItemsPatch
    {
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(UIBuildingStoreButton __instance, ref Building building, ref BuildingData buildingData)
        {
            if(UIBuilingStore.current.isShown == null || !Melon<LifeCore>.Instance.EnableBuildingFromMagicStorage.Value) return;
            EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Building Key: " + building.key);
            if (!buildingData.isUnlocked || buildingData.isBuild)
            {
                EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Unlocked: " + buildingData.isUnlocked);
                EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Built: " + buildingData.isBuild);
                return;
            }
            int i = 1;
            foreach (RequiredItem item in building.requiredItems)
            {
                int inventoryCount = InventoryUtil.GetItemCountInInventory(item.item.key);
                int magicCount = InventoryUtil.GetItemCount(item.item.key, GameData.current.inventoryData.magicChestData);
                EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Item Required " + i + ": " + item.item.key);
                EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Amount Required " + i + ": " + item.amount);
                EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Amount Found " + i + ": " + (inventoryCount + magicCount));
                if (item.amount > inventoryCount + magicCount)
                {
                    return;
                }
                i++;
            }
            __instance.canBuild = true;
            _buildingKey = building.key;
        }
    }
    
    private static string _buildingKey = "";
    
    /// <summary>
    /// Reroutes the call to RemoveItems so that it uses the Magic Storage if the UIBuildingStore is open
    /// </summary>
    [HarmonyPatch(typeof(InventoryUtil), "RemoveItems")]
    private static class RemoveBuildingItems
    {
        // ReSharper disable once UnusedMember.Local
        private static bool Prefix()
        {
            if(UIBuilingStore.current.isShown == null || !Melon<LifeCore>.Instance.EnableBuildingFromMagicStorage.Value) return true;
            EggCore.EggCore.InfoMessage(LifeCore.MelonId,"Rerouting Remove Items to use Magic Storage.");
            EggCore.EggCore.InfoMessage(LifeCore.MelonId,"Building Title: " + UIBuilingStore.current.buildingTitle.text);
            Building building = EggGameplayUtils.GetBuilding(_buildingKey);
            if (building == null)
            {
                EggCore.EggCore.ErrorMessage(LifeCore.MelonId,"Failed to find building");
                return true;
            }
            InventoryUtil.RemoveAndAddItemsFromInventoryOrMagicChest(building.requiredItems);
            return false;
        }
    }
    
    /// <summary>
    /// Reduces the cost of actions based on the tool type and the corresponding cost reduction specified in the configs
    /// </summary>
    [HarmonyPatch(typeof(EnergyUtil), "ChangeEnergy")]
    private static class EnergyCostReduction
    {
        // ReSharper disable once UnusedMember.Local
        private static void Prefix(ref int value)
        {
            EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Energy Changed by " + value + " before reduction.");
            if (PlayerLogic.Current == null) return;
            if (PlayerLogic.Current.currentTool != null)
            {
                EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Tool Type: " + PlayerLogic.Current.currentTool.itemData.itemKey);
                EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Cost Reduction: " + LifeUtils.GetCostReduction(PlayerLogic.Current.currentTool));
            }
            if(value>=0) return;
            value = Math.Min(0, value + LifeUtils.GetCostReduction(PlayerLogic.Current.currentTool));
        }
    }
    
    /// <summary>
    /// Automatically collect items when the player walks into their collision trigger
    /// </summary>
    [HarmonyPatch(typeof(CollectTriggerBehaviour),"ShowActionOutline")]
    private static class CollectAuto
    {
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(CollectTriggerBehaviour __instance)
        {
            if(__instance == null) return;
            if(!Melon<LifeCore>.Instance.AutoCollect.Value) return;
            bool canCollect = InventoryUtil.GetFreeSlotCount(GameData.current.inventoryData.items) > 0 ||
                              InventoryUtil.GetItemCount(__instance.itemKey, GameData.current.inventoryData.items) > 0;
            if (!canCollect) return;
            if (__instance.CanExecuteAction() && !__instance.NeedsEnergy())
            {
                EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Picking up item: " + __instance.itemKey);
                __instance.ExecuteAction();
            }
        }
    }
    
    /// <summary>
    /// Modifies the stats of the Steam Board based on the values in the config
    /// </summary>
    [HarmonyPatch(typeof(SteamBoardTool),"Equip")]
    private static class SteamBoardModifiers
    {
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(SteamBoardTool __instance)
        {
            if (__instance == null) return;
            if (Melon<LifeCore>.Instance.SteamboardSpeedMod.Value > 0)
            {
                __instance.speedModifier += Melon<LifeCore>.Instance.SteamboardSpeedMod.Value;
            }
            if (Melon<LifeCore>.Instance.SteamboardSpeedEmptyMod.Value > 0)
            {
                __instance.speedModifierEmpty += Melon<LifeCore>.Instance.SteamboardSpeedEmptyMod.Value;
            }
            if (Melon<LifeCore>.Instance.SteamboardCost.Value >= 0)
            {
                __instance.steamBoardCost *= Melon<LifeCore>.Instance.SteamboardCost.Value;
            }
            EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Speed Modifier: " + __instance.speedModifier);
            EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Empty Modifier: " + __instance.speedModifierEmpty);
            EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Steam Cost: " + __instance.steamBoardCost);
            EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Jasper skill: " + __instance.jasperSpecialActive);
        }
    }
    
    /// <summary>
    /// Sets the range and energy cost of the blob abilities based on the values in the config
    /// </summary>
    // ReSharper disable once RedundantExplicitParamsArrayCreation
    // ReSharper disable once RedundantExplicitArrayCreation
    [HarmonyPatch(typeof(BlobControlBehaviour), "SetBlobAbilityRadiusVisibile", new Type[] { typeof(BlobAbility) })]
    private static class AbilityRadiusPatch
    {
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private static void Prefix(BlobControlBehaviour __instance, ref BlobAbility blobAbility)
        {
            if (__instance == null) return;
            if (blobAbility == null) return;
            EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Range: " + blobAbility.range);
            EggCore.EggCore.DebugMessage(LifeCore.MelonId,"Energy Cost: " + blobAbility.energyCost);
            if(Melon<LifeCore>.Instance.BlobAbilityRadius.Value > 0)
            {
                blobAbility.range = Melon<LifeCore>.Instance.BlobAbilityRadius.Value;
            }
            if (!Mathf.Approximately(Melon<LifeCore>.Instance.BlobAbilityCostPercent.Value, 1.0f))
            {
                blobAbility.energyCost = (int) Math.Round(Melon<LifeCore>.Instance.BlobAbilityCostPercent.Value * blobAbility.energyCost);
            }
        }
    }
    
    /// <summary>
    /// Sets the max energy based on the value in the config upon player initialization
    /// </summary>
    [HarmonyPatch(typeof(PlayerLogic), "Init")]
    private static class MaxEnergyPatch
    {
        // ReSharper disable once UnusedMember.Local
        private static void Prefix()
        {
            LifeUtils.EnergyMod();
        }
    }
    
    /// <summary>
    /// Sets the dialog text speed based on the value in the config
    /// </summary>
    [HarmonyPatch(typeof(DialogLogic), "Start")]
    private static class DialogTextSpeedPatch
    {
        // ReSharper disable once UnusedMember.Local
        private static void Postfix()
        {
            LifeUtils.SetTextSpeed();
        }
    }
    
    /// <summary>
    /// Adds the extra stars based on the values in the config
    /// </summary>
    [HarmonyPatch(typeof(GameLogic), "Start")]
    private static class ExtraStarsPatch
    {
        // ReSharper disable once UnusedMember.Local
        private static void Postfix()
        {
            Melon<LifeCore>.Instance.AddFallingStarDays();
        }
    }

    [HarmonyPatch(typeof(FishingWater), "DetermineTimeToWaitForNextBite")]
    private static class FishWaitTimePatch
    {
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedMember.Local
        private static void Prefix(FishingWater __instance)
        {
            if(__instance == null) return;
            float battleChance = Melon<LifeCore>.Instance.FishBattleChance.Value;
            float minTime = Melon<LifeCore>.Instance.MinFishWaitTime.Value;
            float maxTime = Melon<LifeCore>.Instance.MaxFishWaitTime.Value;
            if (Melon<LifeCore>.Instance.CleanerCloverTown.Value >= 1)
            {
                int index = 0;
                bool foundTrash = false;
                foreach (FishableObject fishObj in __instance.fishObjects)
                {
                    if(fishObj.name.Equals("Fishable_Trash"))
                    {
                        foundTrash = true;
                        break;
                    }
                    index++;
                }
                if (foundTrash)
                {
                    __instance.fishObjects.RemoveAt(index);
                }
            }
            if (battleChance is < 0f or > 1f)
            {
                EggCore.EggCore.ErrorMessage(LifeCore.MelonId,"Invalid Fish Battle Chance");
                return;
            }
            if(minTime < 0f)
            {
                EggCore.EggCore.ErrorMessage(LifeCore.MelonId,"Min fish wait time cannot be lower than 0");
                return;
            }
            if(maxTime < 0f || minTime > maxTime)
            {
                EggCore.EggCore.ErrorMessage(LifeCore.MelonId,"Max fish wait time cannot be lower than 0 or min time");
                return;
            }
            __instance.possibilityBattle = battleChance;
            __instance.minTimeToWaitForBite = minTime;
            __instance.maxTimeToWaitForBite = maxTime;
        }
    }
    
    [HarmonyPatch(typeof(FishingWater), "DetermineTimeToWaitForNextBite")]
    private static class FishWaitTimePatch2
    {
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedMember.Local
        private static void Prefix(FishingWater __instance)
        {
            if(__instance == null) return;
            EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Water Name: " + __instance.name);
            EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Current Season: " + EggTimeUtils.GetSeason(GameTimeUtil.GetSeason(GameData.current.time)));
            EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Current Time: " + TimeLogic.GetDayTime());
            foreach (FishableObject fishObj in __instance.fishObjects)
            {
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Fishable Name: " + fishObj.name);
            }
        }
    }

    [HarmonyPatch(typeof(FishableObject), "GetRandomItem")]
    private static class FishMoreTrashPatch
    {

        // ReSharper disable once UnusedMember.Local
        private static void Postfix(FishableObject __instance, ref ItemData __result)
        {
            if(__instance == null) return;
            if (!__instance.name.Equals("Fishable_Trash")) return;
            if (Melon<LifeCore>.Instance.CleanerCloverTown.Value <= -1)
            {
                __result.amount -= Melon<LifeCore>.Instance.CleanerCloverTown.Value;
            }
        }
    }
    
    [HarmonyPatch(typeof(BlobBreedingData), "GetBreedingHappinessPercentage")]
    private static class BlobHappyBreedingPatch
    {
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(BlobBreedingData __instance, ref float __result)
        {
            if(Melon<LifeCore>.Instance.BlobAlwaysHappyBreeding.Value)
            {
                
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Blob breeding happiness1");
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Blob happiness: " + __result);
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Blob 1 Guid: " + __instance.blob1Guid);
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Blob 2 Guid: " + __instance.blob2Guid);
                __result = 1f;
            }
        }
    }
    
    /*[HarmonyPatch(typeof(BlobManager), "DetermineMutationsForBreeding")]
    private static class BlobHappyBreedingPatch2
    {
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedMember.Local
        private static void Prefix(BlobManager __instance, ref float happinessPercent, ref BlobData parent1, ref BlobData parent2)
        {
            if(Melon<LifeCore>.Instance.BlobAlwaysHappyBreeding.Value)
            {
                
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Blob breeding happiness2");
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Blob happiness: " + happinessPercent);
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Parent 1: " + parent1.blobName);
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Parent 2: " + parent2.blobName);
                happinessPercent = 5f;
            }
        }
    }*/
    
    /*[HarmonyPatch(typeof(BugBehaviour), "ExecuteAction")]
    private static class ExtraBugsPatch
    {
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(BugBehaviour __instance)
        {
            if(Melon<LifeCore>.Instance.ExtraBugs.Value > 0)
            {
                InventoryUtil.AddItem(GameData.current.inventoryData.items, __instance.itemKey,
                    Melon<LifeCore>.Instance.ExtraBugs.Value);
            }
        }
    }*/
    
    /*[HarmonyPatch(typeof(BugNetTool), "SwingBugNet")]
    private static class ExtraBugsPatch
    {
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(BugNetTool __instance, ref BugBehaviour bug)
        {
            EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Net swung");
            if(Melon<LifeCore>.Instance.ExtraBugs.Value > 0)
            {
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Bug: " + bug.itemKey);
                InventoryUtil.AddItem(GameData.current.inventoryData.items, bug.itemKey,
                    Melon<LifeCore>.Instance.ExtraBugs.Value);
            }
        }
    }

    [HarmonyPatch(typeof(BugBehaviour), "UseTool")]
    private static class ExtraBugsPatch2
    {
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(BugBehaviour __instance)
        {
            EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Bug net used");
            if(Melon<LifeCore>.Instance.ExtraBugs.Value > 0)
            {
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Bug: " + __instance.itemKey);
                InventoryUtil.AddItem(GameData.current.inventoryData.items, __instance.itemKey,
                    Melon<LifeCore>.Instance.ExtraBugs.Value);
            }
        }
    }

    [HarmonyPatch(typeof(BugBehaviour), "OnDestroy")]
    private static class ExtraBugsPatch3
    {
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedMember.Local
        private static void Prefix(BugBehaviour __instance)
        {
            EggCore.EggCore.DebugMessage(LifeCore.MelonId, "About to destroy BugBehaviour");
            EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Escaping: " + __instance.isEscaping);
            if(Melon<LifeCore>.Instance.ExtraBugs.Value > 0)
            {
                EggCore.EggCore.DebugMessage(LifeCore.MelonId, "Bug: " + __instance.itemKey);
                InventoryUtil.AddItem(GameData.current.inventoryData.items, __instance.itemKey,
                    Melon<LifeCore>.Instance.ExtraBugs.Value);
            }
        }
    }*/
}

        