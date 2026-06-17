using EggCore.Utils;
using Il2CppOvaMagica;

namespace EggCore.Actions;

public class IncreaseAllBlobStats(string id) : EggAction(id)
{
    public override void Execute()
    {
        List<BlobData> blobs = new List<BlobData>();
        foreach (BlobData data in GameData.current.blobDatas)
        {
            data.attack += 5;
            data.defense += 5;
            data.speed += 5;
            data.health += 15;
            data.energy += 10;
            blobs.Add(data);
        }
        GameData.current.blobDatas = Il2CppUtils.ConvertToIl2CppList(blobs);
    }
}