using Il2CppOvaMagica;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EggCore.Utils;

public static class EggGameplayUtils
{
    public static void MoveItemsToMagicStorage()
    {
        InventoryData data = GameData.current.inventoryData;
        foreach (var item in data.items)
        {
            int itemAmount = item.amount;
            if(itemAmount == 0) continue;
            string itemKey = item.itemKey;
            if (GameLogic.Current.itemDatabase.GetItem(itemKey).maxStack <= 1) continue;
            int magicAmount = InventoryUtil.GetItemCount(itemKey, data.magicChestData);
            if (item.EggData != null) continue;
            EggCore.DebugMessage("Item Key: " + itemKey);
            EggCore.DebugMessage("Item Amount: " + itemAmount);
            EggCore.DebugMessage("Amount in Magic Storage: " + magicAmount);
            if (magicAmount > 0)
            {
                InventoryUtil.AddItem(data.magicChestData, itemKey, itemAmount);
                InventoryUtil.RemoveItem(itemKey, itemAmount, data.items);
            }
        }
    }
    
    public static List<PlantingGroundBehaviour> GetPlayerPlantingGroundBehaviours()
    {
        List<PlantingGroundBehaviour> playerGrounds = new List<PlantingGroundBehaviour>();
        var grounds = Object.FindObjectsByType<PlantingGroundBehaviour>(FindObjectsSortMode.None);
        foreach (PlantingGroundBehaviour ground in grounds)
        {
            if (ground.isNonPlayer) continue;
            playerGrounds.Add(ground);
        }
        return playerGrounds;
    }

    public static List<TreeBehaviour> GetNonFruitTrees()
    {
        List<TreeBehaviour> nonFruitTrees = new List<TreeBehaviour>();
        var trees = Object.FindObjectsByType<TreeBehaviour>(FindObjectsSortMode.None);
        foreach (TreeBehaviour tree in trees)
        {
            if(tree.fruitItem != null) continue;
            nonFruitTrees.Add(tree);
        }

        return nonFruitTrees;
    }
    
    public static List<TreeBehaviour> GetFruitTrees()
    {
        List<TreeBehaviour> nonFruitTrees = new List<TreeBehaviour>();
        var trees = Object.FindObjectsByType<TreeBehaviour>(FindObjectsSortMode.None);
        foreach (TreeBehaviour tree in trees)
        {
            if(tree.fruitItem == null) continue;
            nonFruitTrees.Add(tree);
        }

        return nonFruitTrees;
    }

    public static List<ResourceBehaviour> GetResources()
    {
        return Il2CppUtils.ConvertToSystemList(Object.FindObjectsByType<ResourceBehaviour>(FindObjectsSortMode.None));
    }
    
    public static Building GetBuilding(string key)
    {
        foreach (Building build in GameLogic.Current.buildingDatabase.buildings)
        {
            if (build.key == key || build.key == key.Replace(" ", "")) return build;
        }
        return null;
    }

    public static Festival GetTodaysFestival()
    {
        return GameLogic.Current.festivalDatabase.GetTodaysFestival();
    }

    public static int GetGameVariableValue(string key)
    {
        return HasGameVariable(key) == false ? 0 : GameVariableData.GetIntValue(GameData.current, key);
    }
    
    public static bool HasGameVariable(string key)
    {
        foreach (var variable in GameData.current.variables)
        {
            if (variable.key == key) return true;
        }
        return false;
    }
    
    public static void SetGameVariable(string key, int value)
    {
        if(HasGameVariable(key)) GameVariableData.SetIntValue(GameData.current, key, value);
        else GameVariableData.CreateVariable(GameData.current, key, value);
    }

    public static void OpenFastTravelMenu()
    {
        if(UIMap.current == null) return;
        if (UIMap.current.close) return;
        UIMap.current.InitFastTravel();
    }
}