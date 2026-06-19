using EggCore;
using HarmonyLib;
using Il2CppOvaMagica;
using MelonLoader;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable InconsistentNaming

[assembly: MelonInfo(typeof(OvaCombatTweaks.CombatCore), "OvaCombatTweaks", "1.0.0", "Cory")]
[assembly: MelonGame("Skinny Frog", "Ova Magica")]
[assembly: MelonAdditionalDependencies("EggCore")]

namespace OvaCombatTweaks
{
    public class CombatCore : MelonMod
    {
        internal const string MelonId = "OvaCombatTweaks";
        
        public MelonPreferences_Category CombatCategory;
        
        public MelonPreferences_Entry<float> BlobTrainingPatchPercent;
        public MelonPreferences_Entry<bool> BlobStartingAtbPatch;
        public MelonPreferences_Entry<bool> BlobSoloBossPatch;
        public MelonPreferences_Entry<bool> BlobSoloBossTrainerPatch;
        
        public override void OnInitializeMelon()
        {
            CombatCategory = MelonPreferences.CreateCategory("Combat");
            CombatCategory.SetFilePath(EggCore.EggCore.ConfigPath);
            
            BlobTrainingPatchPercent = CombatCategory.CreateEntry("Blob Training Patch Percent", -1f);
            BlobStartingAtbPatch = CombatCategory.CreateEntry("Blob Combat Start Patch", false);
            BlobSoloBossPatch = CombatCategory.CreateEntry("Blob Solo Boss Patch", false);
            BlobSoloBossTrainerPatch = CombatCategory.CreateEntry("Blob Solo Boss Trainer Patch", false);
            
            LoggerInstance.Msg("Ova Combat Module Initialized.");
            
            CombatCategory.SaveToFile();
                
            EggCore.EggCore.RegisterLogger(MelonId);
            EggCore.EggCore.RegisterCategory(CombatCategory);
        }
        
        /// <summary>
        /// Patch to enable placing blobs in the training device based on the Blob Training Patch Percent
        /// </summary>
        [HarmonyPatch(typeof(BlobTrainingDevice), "SupportsBlobPlacement")]
        private static class BlobTrainingPatch
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(BlobTrainingDevice __instance, ref bool __result, ref BlobData blobData)
            {
                if(Melon<CombatCore>.Instance.BlobTrainingPatchPercent.Value < 0) return;
                if(__instance == null || blobData == null) return;
                int maxPower = BlobTrainingDevice.GetMaxAllowedPowerLevel();
                int target = (int) Math.Round(maxPower * Melon<CombatCore>.Instance.BlobTrainingPatchPercent.Value);
                int current = 0;
                BlobStat statType = __instance.statToTrain;
                EggCore.EggCore.DebugMessage(MelonId,"Max Power: " + maxPower);
                EggCore.EggCore.DebugMessage(MelonId,"Stat to Train: " + statType);
                EggCore.EggCore.DebugMessage(MelonId,"Target Stat Value: " + target);
                EggCore.EggCore.DebugMessage(MelonId,"Blob Name: " + blobData.blobName);
                EggCore.EggCore.DebugMessage(MelonId,"Blob Attack: " + blobData.attack);
                EggCore.EggCore.DebugMessage(MelonId,"Blob Defense: " + blobData.defense);
                EggCore.EggCore.DebugMessage(MelonId,"Blob Speed: " + blobData.speed);
                EggCore.EggCore.DebugMessage(MelonId,"Blob Health: " + blobData.health);
                EggCore.EggCore.DebugMessage(MelonId,"Blob Energy: " + blobData.energy);
                current = statType switch
                {
                    BlobStat.Atk => blobData.attack,
                    BlobStat.Def => blobData.defense,
                    BlobStat.Agi => blobData.speed,
                    _ => current
                };
                if (current < target)
                {
                    __result = true;
                }
            }
        }
        
        internal static readonly Dictionary<string, OvaData.BlobStatInfo> trainingBlobs = new Dictionary<string, OvaData.BlobStatInfo>();
        
        /// <summary>
        /// Patch to force blob training.
        /// Known Issue: Animation in the training device is broken
        /// </summary>
        [HarmonyPatch(typeof(TimeLogic), "UpdateBlobTrainingDevicesAfterDayPassed")]
        private static class BlobTrainingPatch2
        {
            /// <summary>
            /// Adds blobs to the training list if they can be trained based on the Blob Training Patch Percent
            /// If Blob Training Patch Percent is negative, the patch is disabled
            /// </summary>
            // ReSharper disable once UnusedMember.Local
            private static void Prefix()
            {
                if (Melon<CombatCore>.Instance.BlobTrainingPatchPercent.Value < 0) return;
                foreach (SceneData scene in GameData.current.sceneDatas)
                {
                    foreach (BlobTrainingDeviceData train in scene.blobTrainingDeviceDatas)
                    {
                        BlobData blobData = train.BlobData;
                        BlobFeederData feederData = train.FeederData;
                        if(blobData == null || feederData == null) continue;
                        if (feederData.items.Count <= 0) continue;
                        if (!CombatUtils.CanTrainPatched(train, blobData)) continue;
                        OvaData.BlobStatInfo blobInfo = new OvaData.BlobStatInfo(blobData.guid, blobData.blobName, blobData.attack,
                            blobData.defense, blobData.speed, blobData.health, blobData.energy);
                        trainingBlobs.Add(blobData.guid, blobInfo);
                        EggCore.EggCore.DebugMessage(MelonId,"Blob Added to Training List: " + blobData.blobName);
                    }
                }
            }
            
            /// <summary>
            /// Checks the training list to see if blobs have already been trained, and if not, force trains them 
            /// </summary>
            // ReSharper disable once UnusedMember.Local
            private static void Postfix()
            {
                if (Melon<CombatCore>.Instance.BlobTrainingPatchPercent.Value < 0) return;
                //List<SceneData> scenes = Il2CppUtils.ConvertToSystemList(GameData.current.sceneDatas);
                foreach (SceneData scene in GameData.current.sceneDatas)
                {
                    //List<BlobTrainingDeviceData> devices = Il2CppUtils.ConvertToSystemList(scene.blobTrainingDeviceDatas);
                    //int feederItems = 0;
                    foreach (BlobTrainingDeviceData train in scene.blobTrainingDeviceDatas)
                    {
                        BlobData blobData = train.BlobData;
                        BlobFeederData feederData = train.FeederData;
                        if(blobData == null || feederData == null) continue;
                        OvaData.BlobStatInfo newBlobInfo = new OvaData.BlobStatInfo(blobData);
                        if (!trainingBlobs.TryGetValue(blobData.guid, out var oldBlobInfo)) continue;
                        if (oldBlobInfo.Equals(newBlobInfo))
                        {
                            EggCore.EggCore.InfoMessage(MelonId,"Force Training: " + blobData.blobName);
                            BlobStat statType = train.statToTrain;
                            /*bool removedTrain = devices.Remove(train);
                            EggCore.EggCore.DebugMessage(MelonId,"Removed Training Device: " + removedTrain);
                            //feederData.items.RemoveFirst();//TODO Fix this
                            devices.Add(train);*/
                            switch(statType)
                            {
                                case BlobStat.Atk:
                                    blobData.attack++;
                                    break;
                                case BlobStat.Def:
                                    blobData.defense++;
                                    break;
                                case BlobStat.Agi:
                                    blobData.speed++;
                                    break;
                            }
                        }
                        //else trainingBlobs.Remove(blobData.guid);
                    }
                    //CombatUtils.RemoveFeederItems(scene, feederItems);
                    /*bool removedScene = scenes.Remove(scene);
                    EggCore.EggCore.DebugMessage(MelonId,"Removed Scene: " + removedScene);
                    scene.blobTrainingDeviceDatas = Il2CppUtils.ConvertToIl2CppList(devices);
                    scenes.Add(scene);*/
                }
                //GameData.current.sceneDatas = Il2CppUtils.ConvertToIl2CppList(scenes);
                trainingBlobs.Clear();
            }
        }
        
        /// <summary>
        /// Patch to modify the starting Atb value to more highly favour agility
        /// </summary>
        [HarmonyPatch(typeof(BattleAtbData), "StartWaiting")]
        private static class BattleBlobStartingAtbPatch
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(BattleAtbData __instance, ref bool initial)
            {
                if (!Melon<CombatCore>.Instance.BlobStartingAtbPatch.Value) return;
                if (!initial) return;
                float halfWait = __instance.waitDuration / 2f;
                float agiMod = __instance.battleBlob.CurrentAgi / 50f;
                float maxMod = Mathf.Min(halfWait / 2f  + halfWait * agiMod, __instance.waitDuration);
                float mod = Random.RandomRange(agiMod * 2f, maxMod);
                __instance.atbTimePassed = Mathf.Min(__instance.waitDuration, mod);
                EggCore.EggCore.InfoMessage(MelonId, "Blob Name: " + __instance.battleBlob.BlobSettings.blobData.blobName);
                EggCore.EggCore.InfoMessage(MelonId, "Tweaked Atb: " + __instance.atbTimePassed);
            }
        }

        [HarmonyPatch(typeof(BattleLogic), "CreateBattleChars")]
        private static class BattleBlobTestPatch
        {
            // ReSharper disable once UnusedParameter.Local
            // ReSharper disable once UnusedMember.Local
            private static void Prefix(BattleLogic __instance, ref Il2CppSystem.Collections.Generic.List<BlobData> ownTeamBlobs,
                // ReSharper disable once UnusedParameter.Local
                ref Il2CppSystem.Collections.Generic.List<BlobSettings> enemiesToBattle, 
                // ReSharper disable once UnusedParameter.Local
                ref Il2CppSystem.Collections.Generic.List<BattleTrainer> enemyTrainerPrefabs, 
                // ReSharper disable once UnusedParameter.Local
                ref string companyNpc)
            {
                if(!Melon<CombatCore>.Instance.BlobSoloBossPatch.Value) return;
                if(enemyTrainerPrefabs.Count > 0 && !Melon<CombatCore>.Instance.BlobSoloBossTrainerPatch.Value) return;
                if (ownTeamBlobs.Count == 1)
                {
                    BlobData data = null;
                    foreach (var blob in ownTeamBlobs)
                    {
                        data = blob;
                    }
                    ownTeamBlobs.Clear();
                    if (data != null) data.isBoss = true;
                    else data = new BlobData();
                    ownTeamBlobs.Add(data);
                }
                if (enemiesToBattle.Count == 1)
                {
                    BlobSettings data = null;
                    foreach (var blob in enemiesToBattle)
                    {
                        data = blob;
                    }
                    enemiesToBattle.Clear();
                    if (data != null) data.blobData.isBoss = true;
                    enemiesToBattle.Add(data);
                }
            }
        }
        
        [HarmonyPatch(typeof(BattleArea), "ClearBattleLogic")]
        private static class BattleBlobTestPatch2
        {
            // ReSharper disable once UnusedMember.Local
            private static void Prefix(BattleArea __instance)
            {
                if(__instance == null) return;
                if(__instance.battleLogic == null) return;
                if(!Melon<CombatCore>.Instance.BlobSoloBossPatch.Value) return;
                EggCore.EggCore.DebugMessage(MelonId,"Battle Logic Clearing");
                BattleLogic logic = __instance.battleLogic;
                Il2CppSystem.Collections.Generic.List<BattleBlob> battleBlobsAlly = new Il2CppSystem.Collections.Generic.List<BattleBlob>();
                foreach (var blob in logic.BattleBlobsAlly)
                {
                    blob.BlobSettings.blobData.isBoss = false;
                    battleBlobsAlly.Add(blob);
                }
                logic.BattleBlobsAlly.Clear();
                foreach (var blob in battleBlobsAlly)
                {
                    logic.BattleBlobsAlly.Add(blob);
                }
            }
        }
    }
}