using MelonLoader;

namespace EggCore.Actions;

public class SaveConfigs(string id) : EggAction(id)
{
    public override void Execute()
    {
        Melon<EggCore>.Instance.SaveConfig();
    }
}