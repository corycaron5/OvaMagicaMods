using EggCore;
using Il2CppOvaMagica;

namespace OvaCheatTool.Actions;

public class OpenCheatConsole(string id) : EggAction(id)
{
    public override void Execute()
    {
        if (CheatLogic.current.disableCheats)
        {
            EggCore.EggCore.DebugMessage(CheatCore.MelonId, "Enabling cheats/dev commands");
            CheatLogic.current.disableCheats = false;
            CheatLogic.current.allowDevFunctions = true;
            CheatLogic.current.AllowDevFunctions = true;
        }
        if(CheatLogic.current.cheatConsoleOpen == null) CheatLogic.current.OpenConsole();
        else CheatLogic.current.CloseConsole();
    }
}