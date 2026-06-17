using EggCore;

namespace OvaFarmingAutomation.Actions;

public class PlantSeedAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        FarmingUtils.PlantSeedsInHand();
    }
}