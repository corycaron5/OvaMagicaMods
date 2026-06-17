using EggCore;
using EggCore.Utils;

namespace OvaLifeQuality.Actions;

public class OpenFastTravelAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggGameplayUtils.OpenFastTravelMenu();
    }
}