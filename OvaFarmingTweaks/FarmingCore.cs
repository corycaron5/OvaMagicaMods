using MelonLoader;

[assembly: MelonInfo(typeof(OvaFarmingAutomation.FarmingCore), "OvaFarmingTweaks", "0.1.0", "Cory")]
[assembly: MelonGame("Skinny Frog", "Ova Magica")]

namespace OvaFarmingAutomation
{
    public class FarmingCore : MelonMod
    {
        internal const string MelonId = "OvaFarmingTweaks";
        
        public MelonPreferences_Category FarmingCategory;
        
        /*public MelonPreferences_Entry<KeyCode> TillSoilKey;
        public MelonPreferences_Entry<KeyCode> WaterCropsKey;
        public MelonPreferences_Entry<KeyCode> PlantSeedsKey;
        public MelonPreferences_Entry<KeyCode> FertCropsKey;
        public MelonPreferences_Entry<KeyCode> HarvestCropsKey;
        public MelonPreferences_Entry<KeyCode> VacuumKey;*/
        
        public MelonPreferences_Entry<float> VacuumDistance;
        public MelonPreferences_Entry<float> VacuumSpeed;
        public MelonPreferences_Entry<int> VacuumFrequency;

        public MelonPreferences_Entry<bool> TillCostsEnergy;
        public MelonPreferences_Entry<bool> WaterCostsEnergy;
        public MelonPreferences_Entry<bool> PlantCostsEnergy;
        public MelonPreferences_Entry<bool> FertCostsItems;
        
        public override void OnInitializeMelon()
        {
            FarmingCategory = MelonPreferences.CreateCategory("Farming");
            FarmingCategory.SetFilePath(EggCore.EggCore.ConfigPath);
            
            /*TillSoilKey = FarmingCategory.CreateEntry("Till Soil Key", KeyCode.F1);
            WaterCropsKey = FarmingCategory.CreateEntry("Water Crops Key", KeyCode.F2);
            PlantSeedsKey = FarmingCategory.CreateEntry("Plant Seeds Key", KeyCode.F3);
            FertCropsKey = FarmingCategory.CreateEntry("Fertilize Crops Key", KeyCode.F4);
            HarvestCropsKey = FarmingCategory.CreateEntry("Harvest Crops Key", KeyCode.F5);
            VacuumKey = FarmingCategory.CreateEntry("Vacuum Items Key", KeyCode.F6);*/
            
            VacuumDistance = FarmingCategory.CreateEntry("Vacuum Distance", 20f);
            VacuumSpeed = FarmingCategory.CreateEntry("Vacuum Speed", 12f);
            VacuumFrequency = FarmingCategory.CreateEntry("Vacuum Frequency", 2);
            
            TillCostsEnergy = FarmingCategory.CreateEntry("Bulk Till Soil Costs Energy", true);
            WaterCostsEnergy = FarmingCategory.CreateEntry("Bulk Water Crops Costs Energy", true);
            PlantCostsEnergy = FarmingCategory.CreateEntry("Bulk Plant Seeds Costs Energy", true);
            FertCostsItems = FarmingCategory.CreateEntry("Bulk Fertilize Crops Costs Items", true);
            
            LoggerInstance.Msg("Ova Farming Module Initialized.");
            
            FarmingCategory.SaveToFile();

            EggCore.EggCore.RegisterLogger(MelonId);
            EggCore.EggCore.RegisterCategory(FarmingCategory);
            
            RegisterInternalActions();
        }

        private static void RegisterInternalActions()
        {
            foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                EggCore.EggCore.RegisterAction(type);
            }
        }

        /*public override void OnUpdate()
        {
            if (Input.GetKeyDown(TillSoilKey.Value))
            {
                FarmingUtils.TillPlantingGrounds();
            }
            else if (Input.GetKeyDown(WaterCropsKey.Value))
            {
                FarmingUtils.WaterPlantingGrounds();
            }
            else if (Input.GetKeyDown(PlantSeedsKey.Value))
            {
                FarmingUtils.PlantSeedsInHand();
            }
            else if (Input.GetKeyDown(FertCropsKey.Value))
            {
                FarmingUtils.FertilizePlantingGrounds();
            }
            else if (Input.GetKeyDown(HarvestCropsKey.Value))
            {
                FarmingUtils.HarvestPlantingGrounds();
            }
        }

        private static int _frequencyCounter = 1;
        
        public override void OnFixedUpdate()
        {
            if(_frequencyCounter % Melon<FarmingCore>.Instance.VacuumFrequency.Value != 0)
            {
                _frequencyCounter++;
                return;
            }
            _frequencyCounter++;
            if (Input.GetKey(VacuumKey.Value))
            {
                FarmingUtils.VacuumItems();
            }
        }*/
        
        
    }
}