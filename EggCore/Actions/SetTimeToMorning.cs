using EggCore.Utils;

namespace EggCore.Actions;

public class SetTimeToMorning(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggTimeUtils.SetTimeOfDay(0, 0);
        EggCore.InfoMessage("Time set to 0800");
    }
}