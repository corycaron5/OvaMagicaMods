using EggCore;
using Il2CppOvaMagica;

namespace OvaLifeQuality.Actions;

public class QualifyCloverCupAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        GameData.current.cloverCupData.qualifyingPoints = 5;
    }
}