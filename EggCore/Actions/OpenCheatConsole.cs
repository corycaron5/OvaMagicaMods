using Il2CppOvaMagica;

namespace EggCore.Actions;

public class OpenCheatConsole(string id) : EggAction(id)
{
    public override void Execute()
    {
        //CheatLogic.current.OpenConsole();
    }
}