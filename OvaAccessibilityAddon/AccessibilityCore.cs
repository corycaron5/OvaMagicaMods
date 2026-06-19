using HarmonyLib;
using Il2CppOvaMagica;
using MelonLoader;

[assembly: MelonInfo(typeof(OvaAccessibilityAddon.AccessibilityCore), "OvaAccessibilityAddon", "1.0.0", "Cory")]
[assembly: MelonGame("Skinny Frog", "Ova Magica")]
[assembly: MelonAdditionalDependencies("EggCore")]

namespace OvaAccessibilityAddon
{
    
    public class AccessibilityCore : MelonMod
    {
        internal const string MelonId = "OvaAccessibilityAddon";
        
        public MelonPreferences_Category AccessibilityCategory;
        
        public MelonPreferences_Entry<bool> SkipMinigames;
        public MelonPreferences_Entry<bool> MuteOnFocusLoss;
        public MelonPreferences_Entry<bool> PauseOnFocusLoss;
        
        private AudioMuterPatch _muter;
        private GamePauserPatch _pauser;
        
        public override void OnInitializeMelon()
        {
            AccessibilityCategory = MelonPreferences.CreateCategory("Accessibility");
            AccessibilityCategory.SetFilePath(EggCore.EggCore.ConfigPath);
            
            SkipMinigames = AccessibilityCategory.CreateEntry("Skip Minigames", false);
            MuteOnFocusLoss = AccessibilityCategory.CreateEntry("Mute on Focus Loss", false);
            PauseOnFocusLoss = AccessibilityCategory.CreateEntry("Pause on Focus Loss", false);
            
            LoggerInstance.Msg("Ova Accessibility Module Initialized.");

            AccessibilityCategory.SaveToFile();
            
            EggCore.EggCore.RegisterLogger(MelonId);
            EggCore.EggCore.RegisterCategory(AccessibilityCategory);
        }
        
        /// <summary>
        /// Attaches the muter component to the audio logic game object. If the muter is already attached, logs an error message and returns false.
        /// </summary>
        /// <returns>True if the muter was attached successfully, false otherwise.</returns>
        public bool AttachMuter()
        {
            if (_muter != null)
            {
                EggCore.EggCore.ErrorMessage(MelonId,"Muter already exists");
                return false;
            }
            if (GameLogic.Current == null)
            {
                EggCore.EggCore.ErrorMessage(MelonId,"Game Logic is null");
                return false;
            }
            if (GameLogic.Current.audioLogic == null)
            {
                EggCore.EggCore.ErrorMessage(MelonId,"Audio Logic is null");
                return false;
            }
            if (GameLogic.Current.audioLogic.gameObject == null)
            {
                EggCore.EggCore.ErrorMessage(MelonId,"Audio Logic Game Object is null");
                return false;
            }
            _muter = GameLogic.Current.audioLogic.gameObject.AddComponent<AudioMuterPatch>();
            EggCore.EggCore.InfoMessage(MelonId, "Muter attached");
            return true;
        }

        /// <summary>
        /// Attaches the pauser component to the time logic game object. If the pauser is already attached, logs an error message and returns false.
        /// </summary>
        /// <returns>True if the pauser was attached successfully, false otherwise.</returns>
        public bool AttachPauser()
        {
            if (_pauser != null)
            {
                EggCore.EggCore.ErrorMessage(MelonId,"Pauser already exists");
                return false;
            }
            if (TimeLogic.current == null)
            {
                EggCore.EggCore.ErrorMessage(MelonId,"Time Logic is null");
                return false;
            }
            if (TimeLogic.current.gameObject == null)
            {
                EggCore.EggCore.ErrorMessage(MelonId,"Time Logic Game Object is null");
                return false;
            }
            _pauser = TimeLogic.current.gameObject.AddComponent<GamePauserPatch>();
            EggCore.EggCore.InfoMessage(MelonId, "Pauser attached");
            return true;
        }
        
        /// <summary>
        /// Harmony patch to attach the muter at game start.
        /// </summary>
        [HarmonyPatch(typeof(GameLogic), "Start")]
        private static class AttachPatches
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix()
            {
                Melon<AccessibilityCore>.Instance.AttachMuter();
                Melon<AccessibilityCore>.Instance.AttachPauser();
            }
        }
        
        /// <summary>
        /// Harmony patch to skip minigames.
        /// </summary>
        [HarmonyPatch(typeof(BlobGameBase),"Init", new Type[] { typeof(BlobGameSceneLogic) })]
        private static class BlobGameSkip
        {
            // ReSharper disable once InconsistentNaming
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(BlobGameBase __instance)
            {
                if (!Melon<AccessibilityCore>.Instance.SkipMinigames.Value) return;
                __instance.success = true;
                __instance.isFinished = true;
            }
        }
    }
}