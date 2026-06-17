using Il2CppOvaMagica;
using UnityEngine;

namespace EggCore.Utils;

public static class EggDebugUtils
{
    public static void TimeDebugInfo()
    {
        //Year = 57600 (3s/y)
        //Season = 19200 (5w/s)
        //Week = 3840 (4d/w)
        //Day = 960 (16h/d)
        //Hour = 60 (60m/h)
        //Minute = 1
        //Time starts at 57600
        int currentTime = GameData.current.time;
        
        EggCore.DebugMessage("Current Year: " + GameTimeUtil.GetYear(currentTime));
        EggCore.DebugMessage("Current Season: " + GameTimeUtil.GetSeason(currentTime));
        EggCore.DebugMessage("Current Week: " + GameTimeUtil.GetWeek(currentTime));
        EggCore.DebugMessage("Current Day: " + GameTimeUtil.GetDay(currentTime));
        EggCore.DebugMessage("Current Hour: " + GameTimeUtil.GetHour(currentTime));
        EggCore.DebugMessage("Current Minute: " + GameTimeUtil.GetMin(currentTime));
        EggCore.DebugMessage("Current Scenario: " + GameTimeUtil.GetScenario(currentTime));
        EggCore.DebugMessage("Minutes to Next Day: " + GameTimeUtil.GetMinutesToNextDay(currentTime));
        EggCore.DebugMessage("Minutes to Next Season: " + GameTimeUtil.GetMinutesToNextSeason(currentTime));
        
        EggCore.DebugMessage("Time: " + currentTime);
        EggCore.DebugMessage("Day Time: " + TimeLogic.GetDayTime());
        EggCore.DebugMessage("Current Hour: " + TimeLogic.GetHourInGame());
        EggCore.DebugMessage("Min duration in seconds: " + TimeLogic.current.GetMinDurationInSec());
        EggCore.DebugMessage("Speed Modifier: " + TimeLogic.current.speedModifier);
        EggCore.DebugMessage("Time Stopped: " + TimeLogic.current.IsTimeStopped());
        EggCore.DebugMessage("Hour Morning: " + TimeLogic.hourMorning);
        EggCore.DebugMessage("Hour Evening: " + TimeLogic.hourEvening);
        EggCore.DebugMessage("Hour Night: " + TimeLogic.hourNight);
            
        EggCore.DebugMessage("Time in Years: " + (EggTimeUtils.GetTimeInYears(currentTime)+1));
        EggCore.DebugMessage("Time in Seasons: " + (EggTimeUtils.GetTimeInSeasons(currentTime)+1));
        EggCore.DebugMessage("Time in Weeks: " + (EggTimeUtils.GetTimeInWeeks(currentTime)+1));
        EggCore.DebugMessage("Time in Days: " + (EggTimeUtils.GetTimeInDays(currentTime)+1));
        EggCore.DebugMessage("Time in Hours: " + (EggTimeUtils.GetTimeInHours(currentTime)+8));
        EggCore.DebugMessage("Formatted Time: " + EggTimeUtils.GetFormattedTime(currentTime));
    }

    public static void AssetFinder()
    {
        foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles_Native())
        {
            EggCore.DebugMessage("Bundle Name: " + bundle.name);
            foreach (var asset in bundle.AllAssetNames())
            {
                EggCore.DebugMessage("Asset Name: " + asset);
            }
        }
        //var events = Resources.LoadAll<EventGeneral>("Assets/_OvaMagica/Prefabs/PrefabEvents");
        //var events = Resources.FindObjectsOfTypeAll<EventGeneral>();
        /*foreach (EventGeneral general in events)
        {
            EggCore.DebugMessage("Event Name: " + general.GetEventName());
        }*/
    }

    public static void Nuke()
    {
        foreach (var resource in EggGameplayUtils.GetResources())
        {
            resource.Explode(100);
        }

        foreach (var tree in EggGameplayUtils.GetNonFruitTrees())
        {
            tree.Explode(100);
        }
    }

    public static void MaxFriendship()
    {
        List<NpcRelationData> relations = new List<NpcRelationData>();
        foreach (NpcRelationData relation in GameData.current.npcRelations)
        {
            relation.RaiseFriendship(99);
            relations.Add(relation);
            EggCore.DebugMessage("Friendship raised for: " + relation.npc);
        }
        GameData.current.npcRelations = Il2CppUtils.ConvertToIl2CppList(relations);
    }
}