using EggCore.Utils;

namespace EggCore.Actions;

public class SetTimeToEvening(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggTimeUtils.SetTimeOfDay(10,0);
        EggCore.InfoMessage("Time set to 1800");
    }
}