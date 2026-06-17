using EggCore;
using EggCore.Utils;
using MelonLoader;

namespace OvaLifeQuality.Actions;

public class AddFallingStarAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggTimeUtils.ResetDefaultFallingStarDays();
        Melon<LifeCore>.Instance.AddFallingStarDays();
    }
}