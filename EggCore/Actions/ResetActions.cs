using MelonLoader;

namespace EggCore.Actions;

public class ResetActions(string id) : EggAction(id)
{
    public override void Execute()
    {
        Melon<EggCore>.Instance.ResetInputActions();
        EggCore.DebugMessage("Input Actions Reset");
    }
}