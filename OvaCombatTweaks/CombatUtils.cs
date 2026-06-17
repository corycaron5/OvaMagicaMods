using Il2CppOvaMagica;
using MelonLoader;

namespace OvaCombatTweaks;

public static class CombatUtils
{
    /// <summary>
    /// Checks whether the given blob can still be trained on the given device
    /// based on the current blob training patch percentage.
    /// </summary>
    /// <param name="device">The device to check.</param>
    /// <param name="blobData">The blob data to check.</param>
    /// <returns>true if the blob can still be trained, false otherwise.</returns>
    public static bool CanTrainPatched(BlobTrainingDeviceData device, BlobData blobData)
    {
        if(device == null || blobData == null) return false;
        int maxPower = BlobTrainingDevice.GetMaxAllowedPowerLevel();
        int target = (int) Math.Round(maxPower * Melon<CombatCore>.Instance.BlobTrainingPatchPercent.Value);
        int current = 0;
        BlobStat statType = device.statToTrain;
        current = statType switch
        {
            BlobStat.Atk => blobData.attack,
            BlobStat.Def => blobData.defense,
            BlobStat.Agi => blobData.speed,
            _ => current
        };
        return current < target;
    }

    /*public static void RemoveFeederItems(SceneData scene, int amount)
    {
        int toRemove = amount;
        List<BlobFeederData> updatedFeeders = new List<BlobFeederData>();
        var feeders = scene.blobFeederDatas;
        foreach (BlobFeederData feeder in feeders)
        {
            while (toRemove > 0 && feeder.items.Count > 0)
            {
                feeder.items.RemoveFirst();
                toRemove--;
            }
            updatedFeeders.Add(feeder);
        }
        scene.blobFeederDatas = Il2CppUtils.ConvertToIl2CppList(updatedFeeders);
    }*/
}