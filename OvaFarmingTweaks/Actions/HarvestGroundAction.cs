using EggCore;

namespace OvaFarmingAutomation.Actions;

public class HarvestGroundAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        FarmingUtils.HarvestPlantingGrounds();
    }
}