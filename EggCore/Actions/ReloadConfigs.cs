using MelonLoader;

namespace EggCore.Actions;

public class ReloadConfigs(string id) : EggAction(id)
{
    public override void Execute()
    {
        Melon<EggCore>.Instance.ReloadConfig();
    }
}