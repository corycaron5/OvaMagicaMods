using EggCore;

namespace OvaFarmingAutomation.Actions;

public class WaterGroundAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        FarmingUtils.WaterPlantingGrounds();
    }
}