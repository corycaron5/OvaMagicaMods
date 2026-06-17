using EggCore;

namespace OvaFarmingAutomation.Actions;

public class TillGroundAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        FarmingUtils.TillPlantingGrounds();
    }
}