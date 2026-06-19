using EggCore.Actions;
using EggCore.Utils;
using EggCore.Inputs;
using Il2CppInControl;
using Il2CppOvaMagica;
using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;
using Tomlet;

[assembly: MelonInfo(typeof(EggCore.EggCore), "EggCore", "1.0.0", "Cory")]
[assembly: MelonGame("Skinny Frog", "Ova Magica")]
[assembly: MelonPriority(-100)]//Ensure the core mod is loaded first

namespace EggCore
{
    public class EggCore : MelonMod
    {
        public const string ConfigPath = "UserData/EggCore.cfg";
        public const string ConfigName = "EggCore.cfg";

        private static readonly Dictionary<string, Type> ActionRegistry = new Dictionary<string, Type>();
        
        private static readonly Dictionary<string, EggLogger> Loggers = new Dictionary<string, EggLogger>();
        private static readonly Dictionary<string, MelonPreferences_Category> Categories = new Dictionary<string, MelonPreferences_Category>();
        private static readonly Dictionary<string, EggAction> OnReload = new Dictionary<string, EggAction>();
        private static Dictionary<KeyCode, List<EggAction>> _inputActions = new Dictionary<KeyCode, List<EggAction>>();
        private static Dictionary<KeyCode, List<EggAction>> _repeatingInputActions = new Dictionary<KeyCode, List<EggAction>>();
        
        public MelonPreferences_Category GeneralCategory;
        public MelonPreferences_Category InputActionsCategory;
        public MelonPreferences_Category RepeatingInputActionsCategory;
        
        public MelonPreferences_Entry<int> LoggerLevel;
        /*public MelonPreferences_Entry<KeyCode> ConsoleKey;//TODO
        public MelonPreferences_Entry<KeyCode> DebugInfoKey;
        public MelonPreferences_Entry<KeyCode> ReloadConfigKey;
        public MelonPreferences_Entry<KeyCode> SaveConfigKey;*/
        
        public override void OnInitializeMelon()
        {
            GeneralCategory = MelonPreferences.CreateCategory("General");
            GeneralCategory.SetFilePath(ConfigPath);
            
            InputActionsCategory = MelonPreferences.CreateCategory("InputActions");
            InputActionsCategory.SetFilePath(ConfigPath);
            
            RepeatingInputActionsCategory = MelonPreferences.CreateCategory("RepeatingInputActions");
            RepeatingInputActionsCategory.SetFilePath(ConfigPath);
            
            LoggerLevel = GeneralCategory.CreateEntry("Logger Level", 2);
            EggLogger.Level = EggLogger.GetLogLevel(LoggerLevel.Value);

            InputActionsCategory.CreateEntry("ExampleEntry", "ReloadConfigs");
            RepeatingInputActionsCategory.CreateEntry("ExampleEntry", "");
            
            /*ConsoleKey = GeneralCategory.CreateEntry("Console Key", KeyCode.Tilde); //TODO
            DebugInfoKey = GeneralCategory.CreateEntry("Debug Info Key", KeyCode.F10);
            ReloadConfigKey = GeneralCategory.CreateEntry("Reload Config Key", KeyCode.F11);
            SaveConfigKey = GeneralCategory.CreateEntry("Save Config Key", KeyCode.F12);*/
            
            Loggers.Add("EggCore", new EggLogger(""));
            Categories.Add(GeneralCategory.Identifier, GeneralCategory);
            Categories.Add(InputActionsCategory.Identifier, InputActionsCategory);
            Categories.Add(RepeatingInputActionsCategory.Identifier, RepeatingInputActionsCategory);
            RegisterOnReload(new ResetActions("ResetActions"));

            RegisterInternalActions();

            SaveConfig();
        }

        public override void OnLateInitializeMelon()
        {
            RegisterInputActions(InputActionsCategory, ref _inputActions);
            RegisterInputActions(RepeatingInputActionsCategory, ref _repeatingInputActions);
        }

        public void RegisterInputActions(MelonPreferences_Category category, ref Dictionary<KeyCode, List<EggAction>> actions)
        {
            var toml = TomlParser.ParseFile(MelonEnvironment.UserDataDirectory + Path.DirectorySeparatorChar + ConfigName);
            var section = toml.GetSubTable(category.Identifier);
            foreach (var entry in section.Entries)
            {
                Enum.TryParse(entry.Key, out KeyCode key);
                if (key != KeyCode.None)
                {
                    List<EggAction> acts = new List<EggAction>();
                    foreach (string script in entry.Value.StringValue.Split(';'))
                    {
                        EggAction action = CreationAction(script);
                        if (action != null)
                        {
                            acts.Add(action);
                            InfoMessage("Registered input action: " + script);
                        }
                        else DebugMessage("Failed to create action: " + script);
                    }
                    actions.Add(key, acts);
                }
                else
                {
                    DebugMessage("Failed to parse KeyCode: " + entry.Key);
                }
            }
        }

        public static void ClearInputActions()
        {
            _inputActions.Clear();
            _repeatingInputActions.Clear();
        }

        public void ResetInputActions()
        {
            ClearInputActions();
            RegisterInputActions(InputActionsCategory, ref _inputActions);
            RegisterInputActions(RepeatingInputActionsCategory, ref _repeatingInputActions);
        }

        public static EggAction CreationAction(string name)
        {
            bool reg = ActionRegistry.TryGetValue(name, out Type actionType);
            if(reg) return (EggAction)Activator.CreateInstance(actionType, name);
            actionType = Type.GetType(name);
            if (actionType == null) return null;
            return (EggAction)Activator.CreateInstance(actionType, name);
        }

        public static bool RegisterAction(Type eggActionType)
        {
            if (!eggActionType.IsSubclassOf(typeof(EggAction)))
            {
                DebugMessage("Failed to register action: " + eggActionType.Name + " is not an EggAction");
                return false;
            }
            bool added = ActionRegistry.TryAdd(eggActionType.Name, eggActionType);
            if (!added) DebugMessage("Failed to register action: " + eggActionType.Name);
            else InfoMessage("Registered action: " + eggActionType.Name);
            return added;
        }

        private static void RegisterInternalActions()
        {
            foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                RegisterAction(type);
            }
        }

        /// <summary>
        /// Reloads the configuration for all registered categories from the specified file paths.
        /// </summary>
        /// <remarks>
        /// This method is a shortcut for calling <see cref="MelonPreferences_Category.LoadFromFile"/> on each category in the registered categories list.
        /// </remarks>
        public void ReloadConfig()
        {
            foreach (MelonPreferences_Category category in Categories.Values) category.LoadFromFile();
            foreach (EggAction runnable in OnReload.Values) runnable.Execute();
        }
        
        /// <summary>
        /// Saves the configuration for all registered categories to the specified file paths.
        /// </summary>
        /// <remarks>
        /// This method is a shortcut for calling <see cref="MelonPreferences_Category.SaveToFile"/> on each category in the registered categories list.
        /// </remarks>
        public void SaveConfig()
        {
            foreach (MelonPreferences_Category category in Categories.Values) category.SaveToFile();
        }

        /// <summary>
        /// Registers a logger with the specified ID.
        /// </summary>
        /// <param name="loggerID">The ID to register the logger under.</param>
        /// <returns>True if the logger was registered successfully, false if a logger with the same ID already exists.</returns>
        /// <remarks>
        /// If a logger with the same ID is already registered,
        /// <see cref="ErrorMessage"/> is called with the logger ID "EggCore" and the message "Failed to register logger with ID: " + loggerID.
        /// </remarks>
        public static bool RegisterLogger(string loggerID)
        {
            bool registered = Loggers.TryAdd(loggerID, new EggLogger(loggerID));
            if (!registered) ErrorMessage("Failed to register logger with ID: " + loggerID);
            return registered;
        }

        /// <summary>
        /// Registers a category with the EggCore mod.
        /// </summary>
        /// <param name="category">The category to register.</param>
        /// <returns>True if the category was registered successfully, false if the category is already registered.</returns>
        /// <remarks>
        /// If the category is already registered,
        /// <see cref="ErrorMessage"/> is called with the logger ID "EggCore" and the message "Category already registered.".
        /// </remarks>
        public static bool RegisterCategory(MelonPreferences_Category category)
        {
            // ReSharper disable once CanSimplifyDictionaryLookupWithTryAdd
            if (Categories.ContainsKey(category.Identifier))
            {
                ErrorMessage("Category with ID " + category.Identifier + " already registered.");
                return false;
            }
            Categories.Add(category.Identifier, category);
            return true;
        }
        
        public static bool RegisterOnReload(EggAction action)
        {
            // ReSharper disable once CanSimplifyDictionaryLookupWithTryAdd
            if (OnReload.ContainsKey(action.Id))
            {
                ErrorMessage("Runnable with ID " + action.Id + " already registered.");
                return false;
            }
            OnReload.Add(action.Id, action);
            return true;
        }

        /// <summary>
        /// Logs a critical message using the specified logger.
        /// </summary>
        /// <param name="loggerID">The ID of the logger to use for logging the message.</param>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// If the specified logger is not found, it defaults to calling <see cref="CriticalMessage(string)"/>
        /// with the logger ID "EggCore".
        /// </remarks>
        public static void CriticalMessage(string loggerID, string message)
        {
            Loggers.TryGetValue(loggerID, out EggLogger logger);
            if(logger != null)logger.CriticalMessage(message);
            else CriticalMessage(message);
        }
        
        /// <summary>
        /// Logs an error message using the specified logger.
        /// </summary>
        /// <param name="loggerID">The ID of the logger to use for logging the message.</param>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// If the specified logger is not found, it defaults to calling <see cref="ErrorMessage(string)"/>
        /// with the logger ID "EggCore".
        /// </remarks>
        public static void ErrorMessage(string loggerID, string message)
        {
            Loggers.TryGetValue(loggerID, out EggLogger logger);
            if(logger != null)logger.ErrorMessage(message);
            else ErrorMessage(message);
        }
        
        /// <summary>
        /// Logs a warning message using the specified logger.
        /// </summary>
        /// <param name="loggerID">The ID of the logger to use for logging the message.</param>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// If the specified logger is not found, it defaults to calling <see cref="WarningMessage(string)"/>
        /// with the logger ID "EggCore".
        /// </remarks>
        public static void WarningMessage(string loggerID, string message)
        {
            Loggers.TryGetValue(loggerID, out EggLogger logger);
            if(logger != null)logger.WarningMessage(message);
            else WarningMessage(message);
        }
        
        /// <summary>
        /// Logs an informational message using the specified logger.
        /// </summary>
        /// <param name="loggerID">The ID of the logger to use for logging the message.</param>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// If the specified logger is not found, it defaults to calling <see cref="InfoMessage(string)"/>
        /// with the logger ID "EggCore".
        /// </remarks>
        public static void InfoMessage(string loggerID, string message)
        {
            Loggers.TryGetValue(loggerID, out EggLogger logger);
            if(logger != null)logger.InfoMessage(message);
            else InfoMessage(message);
        }
        
        /// <summary>
        /// Logs a debug message using the specified logger.
        /// </summary>
        /// <param name="loggerID">The ID of the logger to use for logging the message.</param>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// If the specified logger is not found, it defaults to calling <see cref="DebugMessage(string)"/>
        /// with the logger ID "EggCore".
        /// </remarks>
        public static void DebugMessage(string loggerID, string message)
        {
            Loggers.TryGetValue(loggerID, out EggLogger logger);
            if(logger != null)logger.DebugMessage(message);
            else DebugMessage(message);
        }
        
        /// <summary>
        /// Logs a critical message using the default logger.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// This method is a shortcut for calling <see cref="CriticalMessage(string, string)"/> with the logger ID "EggCore".
        /// </remarks>
        internal static void CriticalMessage(string message) => CriticalMessage("EggCore", message);
        
        /// <summary>
        /// Logs an error message using the default logger.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// This method is a shortcut for calling <see cref="ErrorMessage(string, string)"/> with the logger ID "EggCore".
        /// </remarks>
        internal static void ErrorMessage(string message) => ErrorMessage("EggCore", message);
        
        /// <summary>
        /// Logs a warning message using the default logger.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// This method is a shortcut for calling <see cref="WarningMessage(string, string)"/> with the logger ID "EggCore".
        /// </remarks>
        internal static void WarningMessage(string message) => WarningMessage("EggCore", message);
        
        /// <summary>
        /// Logs an informational message using the default logger.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// This method is a shortcut for calling <see cref="InfoMessage(string, string)"/> with the logger ID "EggCore".
        /// </remarks>
        internal static void InfoMessage(string message) => InfoMessage("EggCore", message);
        
        /// <summary>
        /// Logs a debug message using the default logger.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// This method is a shortcut for calling <see cref="DebugMessage(string, string)"/> with the logger ID "EggCore".
        /// </remarks>
        internal static void DebugMessage(string message) => DebugMessage("EggCore", message);
        
        
        /// <summary>
        /// Sets the logging level for the EggLogger and updates the configuration.
        /// </summary>
        /// <param name="level">The desired logging level to be set, given as an integer.</param>
        /// <remarks>
        /// The logging level is set by calling <see cref="EggLogger.GetLogLevel"/> with the given value.
        /// </remarks>
        public static void SetLogLevel(int level)
        { 
            SetLogLevel(EggLogger.GetLogLevel(level));
        }
        
        /// <summary>
        /// Sets the logging level for the EggLogger and updates the configuration.
        /// </summary>
        /// <param name="level">The desired logging level to be set.</param>
        public static void SetLogLevel(EggLogger.LogLevel level)
        { 
            EggLogger.Level = level;
            Melon<EggCore>.Instance.LoggerLevel.Value = (int) level;
            Melon<EggCore>.Instance.SaveConfig();
        }

        public override void OnUpdate()
        {
            if (Input.anyKeyDown)
            {
                SpitOutInput();
                CheckGameLogicInput();
                foreach (KeyCode key in _inputActions.Keys)
                {
                    if (Input.GetKeyDown(key))
                    {
                        DebugMessage("Key down " + key + " detected");
                        foreach (EggAction runnable in _inputActions[key]) runnable.Execute();
                        break;
                    }
                }
            }
        }
        
        public override void OnFixedUpdate()
        {
            if (Input.anyKey)
            {
                CheckGameLogicInputFixed();
                foreach (KeyCode key in _repeatingInputActions.Keys)
                {
                    if (Input.GetKey(key))
                    {
                        DebugMessage("Repeating key " + key + " detected");
                        foreach (EggAction runnable in _repeatingInputActions[key]) runnable.Execute();
                        break;
                    }
                }
                
            }
        }

        private void SpitOutInput()
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKeyDown(code))DebugMessage("Key down " + code + " detected");
            }
        }

        private static void CheckGameLogicInputFixed()
        {
            DebugMessage("Checking Fixed Player Actions");
            if (GameLogic.PlayerInput is not null)
            {
                foreach (var input in GameLogic.PlayerInput.actions)
                {
                    if (input.IsPressed) DebugMessage("Key is pressed: " + input.Name);
                }
            }
            else
            {
                DebugMessage("Player Input is null");
            }
            DebugMessage("Checking Fixed UI Actions");
            if (GameLogic.UiInput is not null)
            {
                foreach (var input in GameLogic.UiInput.actions)
                {
                    if (input.IsPressed) DebugMessage("Key is pressed: " + input.Name);
                }
            }
            else
            {
                DebugMessage("UI Input is null");
            }
        }

        private static void CheckGameLogicInput()
        {
            DebugMessage("Checking Player Actions");
            if (GameLogic.PlayerInput is not null)
            {
                foreach (var input in GameLogic.PlayerInput.actions)
                {
                    if(input.WasPressed) DebugMessage("Key was pressed: " + input.Name);
                }
            }
            else
            {
                DebugMessage("Player Input is null");
            }
            DebugMessage("Checking UI Actions");
            if (GameLogic.UiInput is not null)
            {
                foreach (var input in GameLogic.UiInput.actions)
                {
                    if(input.WasPressed) DebugMessage("Key was pressed: " + input.Name);
                }
            }
            else
            {
                DebugMessage("UI Input is null");
            }
        }

        public static void DebugInfo()
        {
            DataExporter.ExportDatabases();
            EggDebugUtils.TimeDebugInfo();
            //EggDebugUtils.AssetFinder();
        }
    }
}