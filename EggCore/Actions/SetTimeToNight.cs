using EggCore.Utils;

namespace EggCore.Actions;

public class SetTimeToNight(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggTimeUtils.SetTimeOfDay(13,0);
        EggCore.InfoMessage("Time set to 2100");
    }
}