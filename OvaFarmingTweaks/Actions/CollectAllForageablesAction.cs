using EggCore;
using EggCore.Utils;
using Il2CppChivalrousGames.Common;
using Il2CppOvaMagica;

namespace OvaFarmingAutomation.Actions;

public class CollectAllForageablesAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        List<CollectTriggerBehaviour> forageables = EggGameplayUtils.GetForageables();
        foreach (var forageable in forageables)
        {
            bool canCollect = InventoryUtil.GetFreeSlotCount(GameData.current.inventoryData.items) > 0 ||
                              InventoryUtil.GetItemCount(forageable.itemKey, GameData.current.inventoryData.items) > 0;
            if (!canCollect) continue;
            if (forageable.CanExecuteAction() && !forageable.NeedsEnergy())
            {
                EggCore.EggCore.DebugMessage(FarmingCore.MelonId,"Collecting item: " + forageable.itemKey);
                forageable.ExecuteAction();
                forageable.gameObject.SafeDestroy();
            }
            else
            {
                EggCore.EggCore.DebugMessage(FarmingCore.MelonId,"Unable to collect item: " + forageable.itemKey);
            }
        }
    }
}