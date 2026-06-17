using EggCore;

namespace OvaFarmingAutomation.Actions;

public class ClearDeadPlantsAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        FarmingUtils.ClearDeadPlants();
    }
}