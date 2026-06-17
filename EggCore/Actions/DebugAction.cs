using EggCore.Utils;
using Il2CppOvaMagica;

namespace EggCore.Actions;

public class DebugAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        /*List<BlobData> blobs = new List<BlobData>();
        foreach (BlobData data in GameData.current.blobDatas)
        {
            data.attack = 25;
            data.defense = 25;
            data.speed = 25;
            data.health = 250;
            data.energy = 100;
            blobs.Add(data);
        }
        GameData.current.blobDatas = Il2CppUtils.ConvertToIl2CppList(blobs);*/
        foreach (var look in GameLogic.Current.blobManager.blobPrefab.AllLookElements)
        { 
            EggCore.DebugMessage("Look: " + look.name);
        }
    }
}