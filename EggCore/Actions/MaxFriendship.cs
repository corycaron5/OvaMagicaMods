using EggCore.Utils;

namespace EggCore.Actions;

public class MaxFriendship(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggDebugUtils.MaxFriendship();
        EggCore.InfoMessage("Raising friendship for all NPCs");
    }
}