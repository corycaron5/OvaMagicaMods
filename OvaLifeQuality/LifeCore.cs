using EggCore.Utils;
using MelonLoader;
using OvaLifeQuality.Actions;

[assembly: MelonInfo(typeof(OvaLifeQuality.LifeCore), "OvaLifeQuality", "1.0.1", "Cory", "https://www.nexusmods.com/ovamagica/mods/5")]
[assembly: MelonGame("Skinny Frog", "Ova Magica")]
[assembly: MelonAdditionalDependencies("EggCore")]

namespace OvaLifeQuality
{
    public class LifeCore : MelonMod
    {
        internal const string MelonId = "OvaLifeQuality";
        
        public MelonPreferences_Category LifeCategory;
        
        public MelonPreferences_Entry<float> SteamboardSpeedMod;
        public MelonPreferences_Entry<float> SteamboardSpeedEmptyMod;
        public MelonPreferences_Entry<float> SteamboardCost;
        
        public MelonPreferences_Entry<bool> AutoCollect;
        public MelonPreferences_Entry<bool> EnableBuildingFromMagicStorage;
        
        public MelonPreferences_Entry<int> MaxEnergyMod;
        
        public MelonPreferences_Entry<int> AxeActionCostReduction;
        public MelonPreferences_Entry<int> PickaxeActionCostReduction;
        public MelonPreferences_Entry<int> ShovelActionCostReduction;
        public MelonPreferences_Entry<int> HoeActionCostReduction;
        public MelonPreferences_Entry<int> BugNetActionCostReduction;
        public MelonPreferences_Entry<int> FishingActionCostReduction;
        public MelonPreferences_Entry<int> PlantActionCostReduction;
        public MelonPreferences_Entry<int> WaterActionCostReduction;
        
        public MelonPreferences_Entry<float> BlobAbilityRadius;
        public MelonPreferences_Entry<float> BlobAbilityCostPercent;
        
        public MelonPreferences_Entry<bool> BlobAlwaysHappyBreeding;

        public MelonPreferences_Entry<float> DialogTextSpeed;

        public MelonPreferences_Entry<string> FallingStarDays;
        
        public MelonPreferences_Entry<float> MinFishWaitTime;
        public MelonPreferences_Entry<float> MaxFishWaitTime;
        public MelonPreferences_Entry<float> FishBattleChance;
        public MelonPreferences_Entry<int> CleanerCloverTown;
        
        public MelonPreferences_Entry<int> ExtraBugs;
        
        public override void OnInitializeMelon()
        {
            LifeCategory = MelonPreferences.CreateCategory("LifeQuality");
            LifeCategory.SetFilePath(EggCore.EggCore.ConfigPath);
            
            //MoveToMagicKey = LifeCategory.CreateEntry("Move to Magic Storage Key", KeyCode.F8);
            //PauseKey = LifeCategory.CreateEntry("Pause Key", KeyCode.F9);
            EnableBuildingFromMagicStorage = LifeCategory.CreateEntry("Enable Building From Magic Storage", false);
            AutoCollect = LifeCategory.CreateEntry("Auto Collect", false);
            
            SteamboardSpeedMod = LifeCategory.CreateEntry("Steamboard Speed Modifier", 0f);//Default: 1.3f
            SteamboardSpeedEmptyMod = LifeCategory.CreateEntry("Steamboard Empty Speed Modifier", 0f);//Default: 0.2f
            SteamboardCost = LifeCategory.CreateEntry("Steam Cost Percent", -1f);//Default: 0.002f
            
            MaxEnergyMod = LifeCategory.CreateEntry("Max Energy Mod", 0);
            AxeActionCostReduction = LifeCategory.CreateEntry("Axe Action Cost Reduction", 0);
            PickaxeActionCostReduction = LifeCategory.CreateEntry("Pickaxe Action Cost Reduction", 0);
            ShovelActionCostReduction = LifeCategory.CreateEntry("Shovel Action Cost Reduction", 0);
            HoeActionCostReduction = LifeCategory.CreateEntry("Hoe Action Cost Reduction", 0);
            BugNetActionCostReduction = LifeCategory.CreateEntry("Bug Net Action Cost Reduction", 0);
            FishingActionCostReduction = LifeCategory.CreateEntry("Fishing Action Cost Reduction", 0);
            PlantActionCostReduction = LifeCategory.CreateEntry("Plant Action Cost Reduction", 0);
            WaterActionCostReduction = LifeCategory.CreateEntry("Water Action Cost Reduction", 0);
            
            BlobAbilityRadius = LifeCategory.CreateEntry("Blob Ability Radius", 0f);//Default: 1.5
            BlobAbilityCostPercent = LifeCategory.CreateEntry("Blob Ability Cost Percent", 1f);
            
            BlobAlwaysHappyBreeding = LifeCategory.CreateEntry("Blob Always Happy Breeding", false);

            DialogTextSpeed = LifeCategory.CreateEntry("Dialog Text Speed Percent", 1f);
            
            FallingStarDays = LifeCategory.CreateEntry("Falling Star Days", "0:1,9,Solis:17,1:1,Ceres:17,Frigus:1,2:17");
            
            FishBattleChance = LifeCategory.CreateEntry("Fish Battle Chance", 0.33f); //Default: 0.33
            MinFishWaitTime = LifeCategory.CreateEntry("Min Fish Wait Time", 1f); //Default: 1
            MaxFishWaitTime = LifeCategory.CreateEntry("Max Fish Wait Time", 4f); //Default: 4
            CleanerCloverTown = LifeCategory.CreateEntry("Cleaner Clover Town", 0);
            
            ExtraBugs = LifeCategory.CreateEntry("Extra Bugs", 0);
            
            LoggerInstance.Msg("Ova Life Quality Module Initialized.");
            
            LifeCategory.SaveToFile();

            EggCore.EggCore.RegisterLogger(MelonId);
            EggCore.EggCore.RegisterCategory(LifeCategory);
            EggCore.EggCore.RegisterOnReload(new TextSpeedAction("TextSpeed"));
            EggCore.EggCore.RegisterOnReload(new AddFallingStarAction("FallingStars"));
            
            RegisterInternalActions();
        }

        private static void RegisterInternalActions()
        {
            foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                EggCore.EggCore.RegisterAction(type);
            }
        }

        public void AddFallingStarDays()
        {
            if (FallingStarDays.Value == "") return;
            foreach (string days in FallingStarDays.Value.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = days.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1)
                {
                    if (int.TryParse(parts[0], out int day))
                    {
                        EggTimeUtils.AddFallingStarDay(EggTimeUtils.Season.Solis, day);
                        EggTimeUtils.AddFallingStarDay(EggTimeUtils.Season.Ceres, day);
                        EggTimeUtils.AddFallingStarDay(EggTimeUtils.Season.Frigus, day);
                    }
                }
                else if (parts.Length == 2)
                {
                    if (int.TryParse(parts[0], out int season) && int.TryParse(parts[1], out int day))
                    {
                        EggTimeUtils.AddFallingStarDay(season, day);
                    }
                    else if (Enum.TryParse(parts[0], true, out EggTimeUtils.Season season2) && int.TryParse(parts[1], out int day2))
                    {
                        EggTimeUtils.AddFallingStarDay(season2, day2);
                    }
                }
                else
                {
                    EggCore.EggCore.ErrorMessage(MelonId,"Invalid format for Falling Star Days.");
                }
            }
        }
    }
}