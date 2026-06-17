using EggCore;

namespace OvaFarmingAutomation.Actions;

public class FertilizeGroundAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        FarmingUtils.FertilizePlantingGrounds();
    }
}