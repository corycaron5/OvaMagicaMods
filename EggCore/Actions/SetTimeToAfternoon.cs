using EggCore.Utils;

namespace EggCore.Actions;

public class SetTimeToAfternoon(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggTimeUtils.SetTimeOfDay(7,0);
        EggCore.InfoMessage("Time set to 1500");
    }
}