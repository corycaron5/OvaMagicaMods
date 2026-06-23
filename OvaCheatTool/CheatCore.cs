using MelonLoader;

[assembly: MelonInfo(typeof(OvaCheatTool.CheatCore), "OvaCheatTool", "1.0.0", "Cory", null)]
[assembly: MelonGame("Skinny Frog", "Ova Magica")]
[assembly: MelonAdditionalDependencies("EggCore")]

namespace OvaCheatTool
{
    public class CheatCore : MelonMod
    {
        internal const string MelonId = "OvaCheatTool";
        
        public MelonPreferences_Category CheatCategory;
        
        public MelonPreferences_Entry<bool> AllBlobpediaEntryUnlocked;
        public MelonPreferences_Entry<bool> AllResidentsUnlocked;
        public MelonPreferences_Entry<bool> BlobAbilityNeedsNoEnergy;
        public MelonPreferences_Entry<bool> CanBuildWithoutResources;
        public MelonPreferences_Entry<bool> CanCookWithoutResources;
        public MelonPreferences_Entry<bool> DestroyResourcesOneHit;
        public MelonPreferences_Entry<bool> DoNotLowerEnergy;
        public MelonPreferences_Entry<bool> EnableSpeedRaise;
        public MelonPreferences_Entry<bool> EscapeBattleOnSkip;
        public MelonPreferences_Entry<bool> HasAllBlueprints;
        public MelonPreferences_Entry<bool> HasAllRecipes;
        public MelonPreferences_Entry<bool> PlantsDoNotNeedWater;
        public MelonPreferences_Entry<bool> PlantsGrowInAllSeasons;
        public MelonPreferences_Entry<bool> SkipBattle;
        public MelonPreferences_Entry<bool> SkipBattleDungeon;
        public MelonPreferences_Entry<bool> SkipBattleFishing;
        public MelonPreferences_Entry<bool> TreesGrowFast;
        public MelonPreferences_Entry<bool> WinBattleOnSkip;
        
        public override void OnInitializeMelon()
        {
            CheatCategory = MelonPreferences.CreateCategory("CheatTool");
            CheatCategory.SetFilePath(EggCore.EggCore.ConfigPath);

            AllBlobpediaEntryUnlocked = CheatCategory.CreateEntry("All Blobpedia Entries Unlocked", false);
            AllResidentsUnlocked = CheatCategory.CreateEntry("All Residents Unlocked", false);
            HasAllBlueprints = CheatCategory.CreateEntry("Has All Blueprints", false);
            HasAllRecipes = CheatCategory.CreateEntry("Has All Recipes", false);
            BlobAbilityNeedsNoEnergy = CheatCategory.CreateEntry("Blob Ability Doesnt Need Energy", false);
            CanBuildWithoutResources = CheatCategory.CreateEntry("Can Build Without Resources", false);
            CanCookWithoutResources = CheatCategory.CreateEntry("Can Cook Without Resources", false);
            PlantsDoNotNeedWater = CheatCategory.CreateEntry("Plants Do Not Need Water", false);
            PlantsGrowInAllSeasons = CheatCategory.CreateEntry("Plants Grow In All Seasons", false);
            TreesGrowFast = CheatCategory.CreateEntry("Trees Grow Fast", false);
            DestroyResourcesOneHit = CheatCategory.CreateEntry("Destroy Resources With One Hit", false);
            DoNotLowerEnergy = CheatCategory.CreateEntry("Do Not Lower Energy", false);
            EnableSpeedRaise = CheatCategory.CreateEntry("Enable Speed Raise", false);
            SkipBattle = CheatCategory.CreateEntry("Skip Battle", false);
            SkipBattleDungeon = CheatCategory.CreateEntry("Skip Battle Dungeon", false);
            SkipBattleFishing = CheatCategory.CreateEntry("Skip Battle Fishing", false);
            EscapeBattleOnSkip = CheatCategory.CreateEntry("Escape Battle On Skip", false);
            WinBattleOnSkip = CheatCategory.CreateEntry("Win Battle On Skip", false);
            
            LoggerInstance.Msg("Ova Cheat Tool Initialized.");
            
            CheatCategory.SaveToFile();
                
            EggCore.EggCore.RegisterLogger(MelonId);
            EggCore.EggCore.RegisterCategory(CheatCategory);
            
            RegisterInternalActions();
        }

        private static void RegisterInternalActions()
        {
            foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                EggCore.EggCore.RegisterAction(type);
            }
        }
    }
}