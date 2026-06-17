using System.Text.Json;
using EggCore.Utils;
using Il2CppOvaMagica;

namespace EggCore;

public abstract class OvaData
{
    //ItemDatabase -> ItemInfo
    //NpcDatabase
    //QuestDatabase -> QuestInfo
    //SkillDatabase -> SkillInfo
    //StoreObjectDatabase -> StoreObjectInfo
    //FestivalDatabase -> FestivalInfo, FallingStarInfo
    //BuildingDatabase -> BuildingInfo
    //BuildObjectDatabase -> BuildObjectInfo
    //BattleActionDataBase -> BattleActionInfo, BlobAbilityInfo
    //BlobManager

    public struct ExportData(List<SkillInfo> skills, List<BuildingInfo> buildings, List<ItemInfo> items, List<BuildObjectInfo> buildObjects, List<QuestInfo> quests, 
        List<StoreObjectInfo> storeObjects, List<FestivalInfo> festivals, FallingStarInfo fallingStars, List<BattleActionInfo> battleActions, List<BlobAbilityInfo> blobAbilities)
    {
        public List<SkillInfo> Skills = skills;
        public List<BuildingInfo> Buildings = buildings;
        public List<ItemInfo> Items = items;
        public List<BuildObjectInfo> BuildObjects = buildObjects;
        public List<QuestInfo> Quests = quests;
        public List<StoreObjectInfo> StoreObjects = storeObjects;
        public List<FestivalInfo> Festivals = festivals;
        public FallingStarInfo FallingStars = fallingStars;
        public List<BattleActionInfo> BattleActions = battleActions;
        public List<BlobAbilityInfo> BlobAbilities = blobAbilities;

        public ExportData() : this(new List<SkillInfo>(), new List<BuildingInfo>(), new List<ItemInfo>(), new List<BuildObjectInfo>(), 
            new List<QuestInfo>(), new List<StoreObjectInfo>(), new List<FestivalInfo>(), 
            new FallingStarInfo(new List<int>(), new List<int>(), new List<int>()), 
            new List<BattleActionInfo>(), new List<BlobAbilityInfo>()) {}
        
        public static string GetJson(ExportData data)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(data, options);
        }
    }
    
    public readonly struct SkillInfo(string key, string name, string description, string needsForUnlock, int pointCost, bool secret)
    {
        public readonly string Key = key;
        public readonly string Name = name;
        public readonly string Description = description;
        public readonly string NeedsForUnlock = needsForUnlock;
        public readonly int PointCost = pointCost;
        public readonly bool Secret = secret;

        public SkillInfo(SkillBonus skill) : this(skill.key, GameLogic.Current.skillDatabase.GetSkillName(skill.key), GameLogic.Current.skillDatabase.GetSkillDescription(skill.key), 
            skill.needsForUnlock, skill.pointCost, skill.secret) {}

        public static string GetJson(SkillInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }
    
    public readonly struct BuildingInfo(string key, string caption, string npcForUnlock, bool isTool, 
        List<RequiredItemInfo> requiredItems, List<string> unlocksWhenBuild, List<GameVariableInfo> variableToSetWhenBuild)
    {
        public readonly string Key = key;
        public readonly string Caption = caption;
        public readonly string NpcForUnlock = npcForUnlock;
        public readonly bool IsTool = isTool;
        public readonly List<RequiredItemInfo> RequiredItems = requiredItems;
        public readonly List<string> UnlocksWhenBuild = unlocksWhenBuild;
        public readonly List<GameVariableInfo> VariableToSetWhenBuild = variableToSetWhenBuild;

        public BuildingInfo(Building building) : this(building.key, GameLogic.Current.buildingDatabase.GetCaption(building.key), building.npcForUnlock, building.isTool,
            new List<RequiredItemInfo>(), new List<string>(), new List<GameVariableInfo>())
        {
            foreach (var item in building.requiredItems)RequiredItems.Add(new RequiredItemInfo(item));
            foreach (var unlock in building.unlocksWhenBuild)UnlocksWhenBuild.Add(unlock);
            foreach (var variable in building.variableToSetWhenBuild)VariableToSetWhenBuild.Add(new GameVariableInfo(variable));
        }
        
        public static string GetJson(BuildingInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }
    
    public readonly struct RequiredItemInfo(string key, int amount)
    {
        public readonly string Key = key;
        public readonly int Amount = amount;
        
        public RequiredItemInfo(RequiredItem item) : this(item.item.key, item.amount) {}
        
        public static string GetJson(RequiredItemInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }

    public readonly struct GameVariableInfo(string key, int value)
    {
        public readonly string Key = key;
        public readonly int Value = value;
        
        public GameVariableInfo(GameVariableData variable) : this(variable.key, variable.intValue) {}
        
        public static string GetJson(GameVariableInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }

    public readonly struct ItemInfo(
        string key, string caption, string description, bool isBlobGame, bool isBug, bool isConsumable, bool isFarmingGood, bool isFishingGood, bool isNoSeasonalFood,
        bool isRawMaterialGood, bool isRefillable, bool isSeasonalFoodCeres, bool isSeasonalFoodFrigus, bool isSeasonalFoodSolis, bool isSellable, int maxStack,
        string grindIntoItemKey, int grindIntoItemAmount, string meltIntoItemKey, int meltIntoItemAmount, string mixIntoItemKey, int mixIntoItemAmount,
        string pressIntoItemKey, int pressIntoItemAmount, string seedKey, int sellPrice, bool applyHealFromStats, int healthHeal, int energyHeal,
        float healHealthWeight, float healEnergyWeight, string statusEffectHeal, bool canBeDemanded, int fishFillets, int toolLevel, int toolStrength,
        List<string> usedInRecipes, List<string> usedInBlueprints)
    {
        public readonly string Key = key;
        public readonly string Caption = caption;
        public readonly string Description = description;
        public readonly bool IsBlobGame = isBlobGame;
        public readonly bool IsBug = isBug;
        public readonly bool IsConsumable = isConsumable;
        public readonly bool IsFarmingGood = isFarmingGood;
        public readonly bool IsFishingGood = isFishingGood;
        public readonly bool IsNoSeasonalFood = isNoSeasonalFood;
        public readonly bool IsRawMaterialGood = isRawMaterialGood;
        public readonly bool IsRefillable = isRefillable;
        public readonly bool IsSeasonalFoodCeres = isSeasonalFoodCeres;
        public readonly bool IsSeasonalFoodFrigus = isSeasonalFoodFrigus;
        public readonly bool IsSeasonalFoodSolis = isSeasonalFoodSolis;
        public readonly bool IsSellable = isSellable;
        public readonly int MaxStack = maxStack;
        public readonly string GrindIntoItemKey = grindIntoItemKey;
        public readonly int GrindIntoItemAmount = grindIntoItemAmount;
        public readonly string MeltIntoItemKey = meltIntoItemKey;
        public readonly int MeltIntoItemAmount = meltIntoItemAmount;
        public readonly string MixIntoItemKey = mixIntoItemKey;
        public readonly int MixIntoItemAmount = mixIntoItemAmount;
        public readonly string PressIntoItemKey = pressIntoItemKey;
        public readonly int PressIntoItemAmount = pressIntoItemAmount;
        public readonly string SeedKey = seedKey;
        public readonly int SellPrice = sellPrice;
        public readonly bool ApplyHealFromStats = applyHealFromStats;
        public readonly int HealthHeal = healthHeal;
        public readonly int EnergyHeal = energyHeal;
        public readonly float HealHealthWeight = healHealthWeight;
        public readonly float HealEnergyWeight = healEnergyWeight;
        public readonly string StatusEffectHeal = statusEffectHeal;
        public readonly bool CanBeDemanded = canBeDemanded;
        public readonly int FishFillets = fishFillets;
        public readonly int ToolLevel = toolLevel;
        public readonly int ToolStrength = toolStrength;
        public readonly List<string> UsedInRecipes = usedInRecipes;
        public readonly List<string> UsedInBlueprints = usedInBlueprints;

        public ItemInfo(Item item) : this(item.key, GameLogic.Current.itemDatabase.GetCaption(item), GameLogic.Current.itemDatabase.GetDescription(item, item.key), 
            item.isBlobGame, item.isBug, item.isConsumable, item.isFarmingGood, item.isFishingGood, item.IsNoSeasonalFood, item.isRawMaterialGood, 
            item.isRefillable, item.IsSeasonalFoodCeres, item.IsSeasonalFoodFrigus, item.IsSeasonalFoodSolis, item.isSellable, item.maxStack,
            "", item.amountForGrinding, "", item.amountForMelting, 
            "", item.amountForMixing, "", item.amountForPressing, 
            "", item.sellPrice, item.applyHealFromStats, item.healthHeal, item.energyHeal, item.healHealthWeight, item.healEnergyWeight, 
            item.statusEffectHeal.ToString(), item.canBeDemanded, item.fishFillets, item.toolLevel, item.toolStrength,new List<string>(), new List<string>())
        {
            SeedKey = item.srcSeed == null ? "" : item.srcSeed.key;
            GrindIntoItemKey = item.grindIntoItem == null ? "" : item.grindIntoItem.key;
            MeltIntoItemKey = item.meltIntoItem == null ? "" : item.meltIntoItem.key;
            MixIntoItemKey = item.mixIntoItem == null ? "" : item.mixIntoItem.key;
            PressIntoItemKey = item.pressIntoItem == null ? "" : item.pressIntoItem.key;
            foreach (var blueprint in item.usedInBlueprints)UsedInBlueprints.Add(blueprint.key);
            foreach (var recipe in item.usedInRecipes)UsedInRecipes.Add(recipe.key);
        }
        
        public static string GetJson(ItemInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }

    public readonly struct BuildObjectInfo(string key, string caption, string description, string alternativeDescription, string alternativeName, bool allowInBlobBarn,
        bool allowInside, bool allowOutside, bool allowInWater, bool forbidRemove, bool isBed, bool isCarpet, bool isCookingStation, bool isDecor,
        bool isDresser, bool isStorage, int temperatureValue)
    {
        public readonly string Key = key;
        public readonly string Caption = caption;
        public readonly string Description = description;
        public readonly string AlternativeDescription = alternativeDescription;
        public readonly string AlternativeName = alternativeName;
        public readonly bool AllowInBlobBarn = allowInBlobBarn;
        public readonly bool AllowInside = allowInside;
        public readonly bool AllowOutside = allowOutside;
        public readonly bool AllowInWater = allowInWater;
        public readonly bool ForbidRemove = forbidRemove;
        public readonly bool IsBed = isBed;
        public readonly bool IsCarpet = isCarpet;
        public readonly bool IsCookingStation = isCookingStation;
        public readonly bool IsDecor = isDecor;
        public readonly bool IsDresser = isDresser;
        public readonly bool IsStorage = isStorage;
        public readonly int TemperatureValue = temperatureValue;

        public BuildObjectInfo(BuildObject buildObject) : this(buildObject.key, GameLogic.Current.buildObjectDatabase.GetCaption(buildObject), 
            GameLogic.Current.buildObjectDatabase.GetDescription(buildObject), buildObject.alternativeDescription, buildObject.alternativeName, 
            buildObject.allowInBlobBarn, buildObject.allowInside, buildObject.allowOutside, buildObject.buildObjectPrefab.allowInWater, buildObject.buildObjectPrefab.forbidRemove,
            buildObject.buildObjectPrefab.isBed, buildObject.buildObjectPrefab.isCarpet, buildObject.buildObjectPrefab.isCookingStation, buildObject.buildObjectPrefab.isDecor,
            buildObject.buildObjectPrefab.isDresser, buildObject.buildObjectPrefab.isStorage, buildObject.buildObjectPrefab.temperatureValue){}
        
        public static string GetJson(BuildObjectInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }

    public readonly struct QuestInfo(string key, string caption, string description, string buildingForRequiredItems, string locationQuestMarkerWhenActive, bool notCompletableInDemo, 
        bool recheckConditionAppearOnActive, List<RequiredItemInfo> requiredItems, bool showQuestMarkerOnFestivalDay, GameVariableInfo variableToSetOnActive)
    {
        public readonly string Key = key;
        public readonly string Caption = caption;
        public readonly string Description = description;
        public readonly string BuildingForRequiredItems = buildingForRequiredItems;
        public readonly string LocationQuestMarkerWhenActive = locationQuestMarkerWhenActive;
        public readonly bool NotCompletableInDemo = notCompletableInDemo;
        public readonly bool RecheckConditionAppearOnActive = recheckConditionAppearOnActive;
        public readonly List<RequiredItemInfo> RequiredItems = requiredItems;
        public readonly bool ShowQuestMarkerOnFestivalDay = showQuestMarkerOnFestivalDay;
        public readonly GameVariableInfo VariableToSetOnActive = variableToSetOnActive;

        public QuestInfo(Quest quest) : this(quest.key, GameLogic.Current.questDatabase.GetCaption(quest.key), GameLogic.Current.questDatabase.GetDescription(quest.key), 
            quest.buildingForRequiredItems, quest.locationQuestMarkerWhenActive, quest.notCompletableInDemo, quest.recheckConditionAppearOnActive,
            new List<RequiredItemInfo>(), quest.showQuestMarkerOnFestivalDay, new GameVariableInfo(quest.variableToSetOnActive))
        {
            foreach (var item in quest.requiredItems)RequiredItems.Add(new RequiredItemInfo(item));
        }
        
        public static string GetJson(QuestInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }
    
    public readonly struct StoreObjectInfo(string alternativeNameKey, string accessoire, /*TODO BlobSettings*/ string buildObjectKey, string gameConditionVariableCheck, bool checkBlobWorldRune, 
        string checkDonatedToMuseumKey, bool doNorLearnRecipeByEating, string itemKey, string npcFriendship, int npcMinFriendshipLevel, string outfit, int overrideBattleRewardAmount, 
        int price, List<PriceCalculationInfo> priceCalc, bool prioForBattleReward, string recipeKey, bool sellInAutumn, bool sellInSpring, bool sellInWinter, GameVariableInfo variable)
    {
        public readonly string AlternativeNameKey = alternativeNameKey;
        public readonly string Accessoire = accessoire;
        //public readonly BlobSettingsInfo BlobInfo = blobInfo;
        public readonly string BuildObjectKey = buildObjectKey;
        public readonly string GameConditionVariableCheck = gameConditionVariableCheck;
        public readonly bool CheckBlobWorldRune = checkBlobWorldRune;
        public readonly string CheckDonatedToMuseumKey = checkDonatedToMuseumKey;
        public readonly bool DoNorLearnRecipeByEating = doNorLearnRecipeByEating;
        public readonly string ItemKey = itemKey;
        public readonly string NpcFriendship = npcFriendship;
        public readonly int NpcMinFriendshipLevel = npcMinFriendshipLevel;
        public readonly string Outfit = outfit;
        public readonly int OverrideBattleRewardAmount = overrideBattleRewardAmount;
        public readonly int Price = price;
        public readonly List<PriceCalculationInfo> PriceCalc = priceCalc;
        public readonly bool PrioForBattleReward = prioForBattleReward;
        public readonly string RecipeKey = recipeKey;
        public readonly bool SellInAutumn = sellInAutumn;
        public readonly bool SellInSpring = sellInSpring;
        public readonly bool SellInWinter = sellInWinter;
        public readonly GameVariableInfo Variable = variable;

        public StoreObjectInfo(StoreObject storeObject) : this(storeObject.alternativeNameKey, storeObject.accessoire, "", 
            storeObject.check.ToString(), storeObject.checkBlobWorldRune, "", storeObject.doNorLearnRecipebyEating, 
            "", storeObject.npcFriendship, storeObject.npcMinFriendshipLevel, storeObject.outfit, storeObject.overrideBattleRewardAmount, storeObject.price, 
            new List<PriceCalculationInfo>(), storeObject.prioForBattleReward, "", storeObject.sellInAutumn, storeObject.sellInSpring, storeObject.sellInWinter,
            new GameVariableInfo(storeObject.variable, storeObject.variableValue))
        {
            BuildObjectKey = storeObject.buildObject == null ? "" : storeObject.buildObject.key;
            ItemKey = storeObject.item == null ? "" : storeObject.item.key;
            RecipeKey = storeObject.recipe == null ? "" : storeObject.recipe.key;
            CheckDonatedToMuseumKey = storeObject.checkDonatedToMuseum == null ? "" : storeObject.checkDonatedToMuseum.key;
            foreach (var priceCalc in storeObject.priceCalc)PriceCalc.Add(new PriceCalculationInfo(priceCalc));
        }
        
        public static string GetJson(StoreObjectInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }

    public readonly struct PriceCalculationInfo(int basePrice, float baseMultiplicator, bool fixedPrice, bool isFish,
        bool isMakerFurnace, bool isMakerMill, bool isMakerMixer, bool isMakerPress, bool isRecipe, bool isRestaurant, bool isVendor, bool useBasePrice)
    {
        public readonly int BasePrice = basePrice;
        public readonly float BaseMultiplicator = baseMultiplicator;
        public readonly bool FixedPrice = fixedPrice;
        public readonly bool IsFish = isFish;
        public readonly bool IsMakerFurnace = isMakerFurnace;
        public readonly bool IsMakerMill = isMakerMill;
        public readonly bool IsMakerMixer = isMakerMixer;
        public readonly bool IsMakerPress = isMakerPress;
        public readonly bool IsRecipe = isRecipe;
        public readonly bool IsRestaurant = isRestaurant;
        public readonly bool IsVendor = isVendor;
        public readonly bool UseBasePrice = useBasePrice;

        public PriceCalculationInfo(PriceCalculation calc) : this(calc.basePrice, calc.baseMultiplicator, calc.fixedPrice, calc.isFish, calc.isMakerFurnace, calc.isMakerMill,
            calc.isMakerMixer, calc.isMakerPress, calc.isRecipe, calc.isRestaurant, calc.isVendor, calc.useBasePrice){ }
        
        public static string GetJson(PriceCalculationInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }

    public readonly struct FestivalInfo(string key, string caption, string description, int season, int day, int beginHour, int endHour, bool forceWeather, string weather)
    {
        public readonly string Key = key;
        public readonly string Caption = caption;
        public readonly string Description = description;
        public readonly int Season = season;
        public readonly int Day = day;
        public readonly int BeginHour = beginHour;
        public readonly int EndHour = endHour;
        public readonly bool ForceWeather = forceWeather;
        public readonly string Weather = weather;
        
        public FestivalInfo(Festival festival) : this(festival.key, GameLogic.Current.festivalDatabase.GetCaption(festival.key), 
            GameLogic.Current.festivalDatabase.GetDescription(festival.key), festival.season, festival.day, festival.beginHour, 
            festival.endHour, festival.forceWeather, festival.weather.ToString()){ }
        
        public static string GetJson(FestivalInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }

    public readonly struct FallingStarInfo(List<int> fallingStarDaysCeres, List<int> fallingStarDaysFrigus, List<int> fallingStarDaysSolis)
    {
        public readonly List<int> FallingStarDaysCeres = fallingStarDaysCeres;
        public readonly List<int> FallingStarDaysFrigus = fallingStarDaysFrigus;
        public readonly List<int> FallingStarDaysSolis = fallingStarDaysSolis;
        
        public static string GetJson(FallingStarInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }

    public readonly struct BattleActionInfo(string key, string caption, string description, string damageGrade, string element, int extraEnergyCost, string levelUpStat,
        string statusEffect, float statusEffectChance, int agiChange, int atkChange, int defChange, bool isHeal, bool isPositive, bool isRevive,
        bool isSingleTarget, bool noDamage, bool selectUser)
    {
        public readonly string Key = key;
        public readonly string Caption = caption;
        public readonly string Description = description;
        public readonly string DamageGrade = damageGrade;
        public readonly string Element = element;
        public readonly int ExtraEnergyCost = extraEnergyCost;
        public readonly string LevelUpStat = levelUpStat;
        public readonly string StatusEffect = statusEffect;
        public readonly float StatusEffectChance = statusEffectChance;
        public readonly int AgiChange = agiChange;
        public readonly int AtkChange = atkChange;
        public readonly int DefChange = defChange;
        public readonly bool IsHeal = isHeal;
        public readonly bool IsPositive = isPositive;
        public readonly bool IsRevive = isRevive;
        public readonly bool IsSingleTarget = isSingleTarget;
        public readonly bool NoDamage = noDamage;
        public readonly bool SelectUser = selectUser;
        
        public BattleActionInfo(BattleAction battleAction) : this(battleAction.key, GameLogic.Current.battleActionDataBase.GetBattleActionCaption(battleAction.key),
            GameLogic.Current.battleActionDataBase.GetBattleActionHelp(battleAction), battleAction.damageGrade.ToString(), battleAction.element.ToString(), 
            battleAction.extraEnergyCost, battleAction.levelUpStat.ToString(), battleAction.statusEffect.ToString(), battleAction.statusEffectChance, 
            battleAction.agiChange, battleAction.atkChange, battleAction.defChange, battleAction.isHeal, battleAction.isPositive, 
            battleAction.isRevive, battleAction.isSingleTarget, battleAction.noDamage, battleAction.selectUser){ }
        
        public static string GetJson(BattleActionInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }

    public readonly struct BlobAbilityInfo(string key, string caption, string description, bool isPassive, int energyCost,
        float range, bool isSingleTarget, bool useInFront, string product, List<string> productsRandom)
    {
        public readonly string Key = key;
        public readonly string Caption = caption;
        public readonly string Description = description;
        public readonly bool IsPassive = isPassive;
        public readonly int EnergyCost = energyCost;
        public readonly float Range = range;
        public readonly bool IsSingleTarget = isSingleTarget;
        public readonly bool UseInFront = useInFront;
        public readonly string Product = product;
        public readonly List<string> ProductsRandom = productsRandom;

        public BlobAbilityInfo(BlobAbility blobAbility) : this(blobAbility.key, GameLogic.Current.battleActionDataBase.GetNonBattleActionCaption(blobAbility.key),
            GameLogic.Current.battleActionDataBase.GetNonBattleActionDescription(blobAbility), blobAbility.isPassive, blobAbility.energyCost, blobAbility.range,
            blobAbility.isSingleTarget, blobAbility.useInFront, blobAbility.product, Il2CppUtils.ConvertToSystemList(blobAbility.productsRandom)) { }
        
        public static string GetJson(BlobAbilityInfo info)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            return JsonSerializer.Serialize(info, options);
        }
    }
    
    public readonly struct BlobStatInfo(string guid, string blobName, int attack, int defense, int speed, int health, int energy) : IEquatable<BlobStatInfo>
    {
        public readonly string Guid = guid;
        public readonly string BlobName = blobName;
        public readonly int Attack = attack;
        public readonly int Defense = defense;
        public readonly int Speed = speed;
        public readonly int Health = health;
        public readonly int Energy = energy;

        public BlobStatInfo(BlobData blobData) : this(blobData.guid, blobData.blobName, blobData.attack, blobData.defense, blobData.speed, blobData.health, blobData.energy) { }

        public int GetPowerLevel()
        {
            return Attack + Defense + Speed;
        }

        public override string ToString()
        {
            return "Guid: " + Guid + " Blob Name: " + BlobName + " Attack: " + Attack + " Defense: " + Defense + " Speed: " + Speed + " Health: " + Health + " Energy: " + Energy;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            BlobStatInfo other = (BlobStatInfo) obj;
            return Guid.Equals(other.Guid) && BlobName.Equals(other.BlobName) && 
                   Attack == other.Attack && Defense == other.Defense && 
                   Speed == other.Speed && Health == other.Health && Energy == other.Energy;
        }

        public static bool operator ==(BlobStatInfo left, BlobStatInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlobStatInfo left, BlobStatInfo right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            int hash = Guid.GetHashCode();
            hash += 7 * BlobName.GetHashCode();
            hash += 13 * Attack.GetHashCode();
            hash += 17 * Defense.GetHashCode();
            hash += 19 * Speed.GetHashCode();
            hash += 23 * Health.GetHashCode();
            hash += 29 * Energy.GetHashCode();
            return hash;
        }

        public bool Equals(BlobStatInfo other)
        {
            return Guid == other.Guid && BlobName == other.BlobName && Attack == other.Attack && Defense == other.Defense && Speed == other.Speed && Health == other.Health && Energy == other.Energy;
        }
    }
}