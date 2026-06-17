using System.Text.Json;
using Il2CppOvaMagica;
using MelonLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EggCore.Utils;

public static class DataExporter
{
    //ItemDatabase -> Done
    //NpcDatabase
    //QuestDatabase -> Done
    //SkillDatabase -> Done
    //StoreObjectDatabase -> Done -> Database is never instantiated? Find all StoreOwners?
    //FestivalDatabase -> Done
    //BuildingDatabase -> Done
    //BuildObjectDatabase -> Done
    //BattleActionDataBase -> Done
    //BlobManager 
    
    private static OvaData.ExportData _toExport = new OvaData.ExportData();

    private static string ConvertToJson(OvaData.ExportData data)
    {
        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;
        options.IncludeFields = true;
        return JsonSerializer.Serialize(data, options);
    }

    public static void ExportDatabases()
    {
        ExportSkills();
        ExportBuildings();
        ExportItems();
        ExportBuildObjects();
        ExportQuests();
        ExportStoreObjects();
        ExportFestivals();
        ExportFallingStars();
        ExportBattleActions();
        ExportBlobAbilities();

        MelonPreferences_Category config = MelonPreferences.CreateCategory("Export");
        config.SetFilePath("UserData/Export.json");
        string json = ConvertToJson(_toExport);
        config.DeleteEntry("Databases");
        config.CreateEntry("Databases", json);
        config.SaveToFile();
    }

    private static void ExportSkills()
    {
        List<OvaData.SkillInfo> skills = new List<OvaData.SkillInfo>();
        foreach(SkillBonus skill in GameLogic.Current.skillDatabase.skills)
        {
            EggCore.DebugMessage("Exporting Skill: " + skill.key);
            skills.Add(new OvaData.SkillInfo(skill));
        }
        _toExport.Skills = skills;
        //ExportSkillImages();
    }

    /*private static void ExportSkillImages()
    {
        foreach(SkillBonus skill in GameLogic.Current.skillDatabase.skills)
        {
            EggCore.DebugMessage("Exporting Skill Image: " + skill.key);
            byte[] image = skill.image.texture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + Path.PathSeparator + "Export" + Path.PathSeparator + "Skills" + Path.PathSeparator + skill.key + ".png", image);
        }
    }*/
    
    private static void ExportBuildings()
    {
        List<OvaData.BuildingInfo> buildings = new List<OvaData.BuildingInfo>();
        foreach(Building building in GameLogic.Current.buildingDatabase.buildings)
        {
            EggCore.DebugMessage("Exporting Building: " + building.key);
            buildings.Add(new OvaData.BuildingInfo(building));
        }
        _toExport.Buildings = buildings;
    }
    
    private static void ExportItems()
    {
        List<OvaData.ItemInfo> items = new List<OvaData.ItemInfo>();
        foreach(Item item in GameLogic.Current.itemDatabase.items)
        {
            EggCore.DebugMessage("Exporting Item: " + item.key);
            items.Add(new OvaData.ItemInfo(item));
        }
        _toExport.Items = items;
    }
    
    private static void ExportBuildObjects()
    {
        List<OvaData.BuildObjectInfo> buildObjects = new List<OvaData.BuildObjectInfo>();
        foreach(BuildObject buildObject in GameLogic.Current.buildObjectDatabase.buildObjects)
        {
            EggCore.DebugMessage("Exporting Build Object: " + buildObject.key);
            buildObjects.Add(new OvaData.BuildObjectInfo(buildObject));
        }
        _toExport.BuildObjects = buildObjects;
    }
    
    private static void ExportQuests()
    {
        List<OvaData.QuestInfo> quests = new List<OvaData.QuestInfo>();
        foreach(Quest quest in GameLogic.Current.questDatabase.quests)
        {
            EggCore.DebugMessage("Exporting Quest: " + quest.key);
            quests.Add(new OvaData.QuestInfo(quest));
        }
        _toExport.Quests = quests;
    }

    private static void ExportStoreObjects()
    {
        var storeObjectDatabase = Object.FindObjectsByType<StoreObjectDatabase>(FindObjectsSortMode.None);
        if (storeObjectDatabase.Count <= 0) return;
        List<OvaData.StoreObjectInfo> storeObjects = new List<OvaData.StoreObjectInfo>();
        foreach(StoreObject storeObject in storeObjectDatabase[0].storeObjects)
        {
            EggCore.DebugMessage("Exporting Store Object: " + storeObject.alternativeNameKey);
            storeObjects.Add(new OvaData.StoreObjectInfo(storeObject));
        }
        _toExport.StoreObjects = storeObjects;
    }
    
    private static void ExportFestivals()
    {
        List<OvaData.FestivalInfo> festivals = new List<OvaData.FestivalInfo>();
        foreach(Festival festival in GameLogic.Current.festivalDatabase.festivals)
        {
            EggCore.DebugMessage("Exporting Festival: " + festival.key);
            festivals.Add(new OvaData.FestivalInfo(festival));
        }
        _toExport.Festivals = festivals;
    }

    private static void ExportFallingStars()
    {
        EggCore.DebugMessage("Exporting Falling Star Days");
        _toExport.FallingStars = new OvaData.FallingStarInfo(
            Il2CppUtils.ConvertToSystemList(GameLogic.Current.festivalDatabase.fallingStarDaysCeres),
            Il2CppUtils.ConvertToSystemList(GameLogic.Current.festivalDatabase.fallingStarDaysFrigus),
            Il2CppUtils.ConvertToSystemList(GameLogic.Current.festivalDatabase.fallingStarDaysSolis));
    }
    
    private static void ExportBattleActions()
    {
        List<OvaData.BattleActionInfo> battleActions = new List<OvaData.BattleActionInfo>();
        foreach(BattleAction battleAction in GameLogic.Current.battleActionDataBase.battleActions)
        {
            EggCore.DebugMessage("Exporting Battle Action: " + battleAction.key);
            battleActions.Add(new OvaData.BattleActionInfo(battleAction));
        }
        _toExport.BattleActions = battleActions;
    }

    private static void ExportBlobAbilities()
    {
        List<OvaData.BlobAbilityInfo> blobAbilities = new List<OvaData.BlobAbilityInfo>();
        foreach (BlobAbility blobAbility in GameLogic.Current.battleActionDataBase.blobAbilities)
        {
            EggCore.DebugMessage("Exporting Blob Ability: " + blobAbility.key);
            blobAbilities.Add(new OvaData.BlobAbilityInfo(blobAbility));
        }

        _toExport.BlobAbilities = blobAbilities;
    }
}