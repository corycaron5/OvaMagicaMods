using Il2CppOvaMagica;

namespace EggCore.Actions;

public class RevealDungeonMap(string id) :EggAction(id)
{
    public override void Execute()
    {
        DungeonSceneLogic.CurrentDungeonSceneLogic.RevealMap();
    }
}