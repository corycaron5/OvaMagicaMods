using EggCore.Utils;
using Il2CppOvaMagica;

namespace EggCore.Actions;

public class DebugAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        /*foreach (var look in GameLogic.Current.blobManager.blobPrefab.AllLookElements)
        { 
            EggCore.DebugMessage("Look: " + look.name);
        }*/
        EggCore.DebugMessage("Day length: " + SettingsDataNew.instance.dayLength);
    }
}