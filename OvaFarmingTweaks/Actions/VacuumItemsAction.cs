using EggCore;
using MelonLoader;

namespace OvaFarmingAutomation.Actions;

public class VacuumItemsAction(string id) : EggAction(id)
{
    private static int _frequencyCounter = 1;
    
    public override void Execute()
    {
        _frequencyCounter++;
        if (_frequencyCounter % Melon<FarmingCore>.Instance.VacuumFrequency.Value != 0) return;
        FarmingUtils.VacuumItems();
    }
}