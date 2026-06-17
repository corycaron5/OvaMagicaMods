using EggCore;
using EggCore.Utils;

namespace OvaLifeQuality.Actions;

public class MoveMagicStorageAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        EggGameplayUtils.MoveItemsToMagicStorage();
    }
}