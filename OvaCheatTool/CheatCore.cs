using MelonLoader;

[assembly: MelonInfo(typeof(OvaCheatTool.CheatCore), "OvaCheatTool", "1.0.0", "Cory", null)]
[assembly: MelonGame("Skinny Frog", "Ova Magica")]

namespace OvaCheatTool
{
    public class CheatCore : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }
}