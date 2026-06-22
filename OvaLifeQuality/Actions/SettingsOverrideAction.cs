using EggCore;
using EggCore.Utils;
using MelonLoader;

namespace OvaLifeQuality.Actions;

public class SettingsOverrideAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        if(Melon<LifeCore>.Instance.DayLength.Value != 0) EggTimeUtils.SetDayLength(Math.Max(Melon<LifeCore>.Instance.DayLength.Value, -15));
    }
}