using EggCore.Utils;

namespace EggCore.Actions;

public class NukeScript(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggDebugUtils.Nuke();
        EggCore.InfoMessage("Nuking all resources and trees");
    }
}