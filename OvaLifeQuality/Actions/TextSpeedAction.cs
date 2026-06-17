using EggCore;

namespace OvaLifeQuality.Actions;

public class TextSpeedAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        LifeUtils.SetTextSpeed();
    }
}