using EggCore.Utils;
using Il2CppOvaMagica;

namespace EggCore.Actions;

public class SetFestivalTime(string id) : EggAction(id)
{
    public override void Execute()
    {
        Festival fest = EggGameplayUtils.GetTodaysFestival();
        if(fest ==null)
        {
            EggCore.InfoMessage("No festival today");
            return;
        }
        EggTimeUtils.SetTimeOfDay(fest.beginHour - 8, 0);
        EggCore.InfoMessage("Time set to " + fest.beginHour + "00");
    }
}