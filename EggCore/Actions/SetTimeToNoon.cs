using EggCore.Utils;

namespace EggCore.Actions;

public class SetTimeToNoon(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggTimeUtils.SetTimeOfDay(4,0);
        EggCore.InfoMessage("Time set to 1200");
    }
}