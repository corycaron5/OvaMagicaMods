using EggCore.Utils;
using Il2CppMultiplayerBasicExample;
using Il2CppOvaMagica;
using MelonLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OvaFarmingAutomation;

public static class FarmingUtils
{
    /// <summary>
    /// Plants seeds in hand. Iterates over the player's planting grounds and
    /// calls the PlantSeed method on each ground where the ground is hoed and
    /// the plant can be planted.
    /// </summary>
    public static void PlantSeedsInHand()
    {
        PlayerTool tool = PlayerLogic.Current.currentTool;
        if (tool == null) return;
        if (!tool.itemData.itemKey.Contains("Seed") || tool.itemData.itemKey.Contains("Tree")) return;
        int count = tool.itemData.amount;
        string plant = tool.itemData.itemKey.Replace("Seed", "Plant");
        EggCore.EggCore.DebugMessage(FarmingCore.MelonId, "Seeds Found: " + count);
        EggCore.EggCore.DebugMessage(FarmingCore.MelonId, "Seed Key: " + tool.itemData.itemKey);
        EggCore.EggCore.DebugMessage(FarmingCore.MelonId, "Plant Prefab: " + plant);
        foreach (PlantingGroundBehaviour ground in EggGameplayUtils.GetPlayerPlantingGroundBehaviours())
        {
            if (count <= 0) break;
            if (ground.plant == null && ground.isHoed && !ground.isPlantDead)
            {
                if (Melon<FarmingCore>.Instance.PlantCostsEnergy.Value)
                {
                    if (GameData.current.playerData.currentEnergy > 0)
                    {
                        EnergyUtil.ChangeEnergy(0 - EnergyUtil.GetPlantEnergyCost());
                    }
                    else break;
                }
                ground.PlantSeed(plant,ground.GetPlantPos());
                count--;
            }
        }
        if (count <= 0)
        {
            InventoryUtil.RemoveItemFromCurrentHotbarSlot(tool.itemData.amount);
        }
        else
        {
            InventoryUtil.RemoveItemFromCurrentHotbarSlot(tool.itemData.amount - count);
        }
    }

    /// <summary>
    /// Waters all planting grounds that are hoed but not yet watered.
    /// Iterates over the player's planting grounds and calls the SetWatered method
    /// on each ground where the ground is hoed but not yet watered.
    /// </summary>
    public static void WaterPlantingGrounds()
    {
        foreach (PlantingGroundBehaviour ground in EggGameplayUtils.GetPlayerPlantingGroundBehaviours())
        {
            if (ground.isHoed && !ground.isWatered)
            {
                if (Melon<FarmingCore>.Instance.WaterCostsEnergy.Value)
                {
                    if (GameData.current.playerData.currentEnergy > 0)
                    {
                        EnergyUtil.ChangeEnergy(0 - EnergyUtil.GetWaterCanEnergyCost());
                    }
                    else break;
                }
                ground.SetWatered(true);
            }
        }
    }

    /// <summary>
    /// Tills all planting grounds that are not already tilled.
    /// Iterates over the player's planting grounds and calls the SetHoed method
    /// on each ground where the ground is not already tilled.
    /// </summary>
    public static void TillPlantingGrounds()
    {
        foreach (PlantingGroundBehaviour ground in EggGameplayUtils.GetPlayerPlantingGroundBehaviours())
        {
            if (!ground.isHoed)
            {
                if (Melon<FarmingCore>.Instance.TillCostsEnergy.Value)
                {
                    if (GameData.current.playerData.currentEnergy > 0)
                    {
                        EnergyUtil.ChangeEnergy(0 - EnergyUtil.GetHoeEnergyCost());
                    }
                    else break;
                }
                ground.SetHoed(true);
            }
        }
    }

    /// <summary>
    /// Fertilizes all planting grounds that are ready for fertilization.
    /// Iterates over the player's planting grounds and calls the Fertilize method
    /// on each ground where the plant can be fertilized.
    /// </summary>
    public static void FertilizePlantingGrounds()
    {
        int count = 0;
        PlayerTool tool = PlayerLogic.Current.currentTool;
        if(Melon<FarmingCore>.Instance.FertCostsItems.Value){
            if (tool == null) return;
            if (!tool.itemData.itemKey.Contains("Fertilizer")) return;
            count = PlayerLogic.Current.currentTool.itemData.amount;
            EggCore.EggCore.InfoMessage(FarmingCore.MelonId, "Fertilizer Found: " + count);
        }
        foreach (PlantingGroundBehaviour ground in EggGameplayUtils.GetPlayerPlantingGroundBehaviours())
        {
            if (ground.PlantCanBeFertilized())
            {
                if(Melon<FarmingCore>.Instance.FertCostsItems.Value)
                {
                    if (count == 0)
                    {
                        break;
                    }
                    InventoryUtil.RemoveItemFromCurrentHotbarSlot(1);
                    count--;
                }
                ground.Fertilize();
            }
        }
    }

    /// <summary>
    /// Harvests all planting grounds that are ready for harvest.
    /// Iterates over the player's planting grounds and calls the Harvest method
    /// on each ground where the plant can be harvested.
    /// </summary>
    public static void HarvestPlantingGrounds()
    {
        foreach (PlantingGroundBehaviour ground in EggGameplayUtils.GetPlayerPlantingGroundBehaviours())
        {
            if (ground.PlantCanBeHarvested())
            {
                /*if (Melon<FarmingCore>.Instance.HarvestCostsEnergy.Value)
                {
                    if (GameData.current.playerData.currentEnergy > 0)
                    {
                        EnergyUtil.ChangeEnergy(0 - EnergyUtil);
                    }
                    else break;
                }*/
                ground.Harvest();
            }
        }
    }

    /// <summary>
    /// Clears all planting grounds that have a dead plant.
    /// Iterates over the player's planting grounds and calls the ClearPlant method
    /// on each ground where the plant is dead.
    /// </summary>
    public static void ClearDeadPlants()
    {
        foreach (PlantingGroundBehaviour ground in EggGameplayUtils.GetPlayerPlantingGroundBehaviours())
        {
            if (ground.isPlantDead)
            {
                ground.SetPlantDead(false);
                ground.ClearPlant();
            }
        }
    }

    /// <summary>
    /// Moves items within VacuumDistance units of the player.
    /// Items are moved at VacuumSpeed divided by Time.fixedDeltaTime units per second.
    /// </summary>
    public static void VacuumItems()
    {
        if (!PlayerLogic.Current) return;
        Vector3 playerPos = PlayerLogic.Current.transform.position;
        var items = Object.FindObjectsByType<ItemBehaviour>(FindObjectsSortMode.None);
        float vacuumDistance = Melon<FarmingCore>.Instance.VacuumDistance.Value;
        float vacuumSpeed = Melon<FarmingCore>.Instance.VacuumSpeed.Value;
        foreach (ItemBehaviour item in items)
        {
            if(!item.collide.enabled)continue;
            if (Vector3.SqrMagnitude(playerPos - item.transform.position) >= vacuumDistance * vacuumDistance) continue; 
            item.transform.position = Vector3.MoveTowards(item.transform.position, playerPos, vacuumSpeed * Time.fixedDeltaTime);
            if (Vector3.SqrMagnitude(playerPos - item.transform.position) <= 0.25f) item.Collect();
        }
    }
}