using EggCore;
using EggCore.Utils;

namespace OvaLifeQuality.Actions;

public class PauseTimeAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggTimeUtils.TogglePause();
    }
}