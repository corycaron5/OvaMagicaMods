using EggCore;
using Il2CppOvaMagica;
using MelonLoader;

namespace OvaCheatTool.Actions;

public class ActivateCheatsAction(string id) : EggAction(id)
{
    public override void Execute()
    {
        if (CheatLogic.current == null)
        {
            EggCore.EggCore.WarningMessage(CheatCore.MelonId, "CheatLogic is null");
            return;
        }
        CheatLogic.current.disableCheats = false;
        CheatLogic.current.allowDevFunctions = true;
        CheatLogic.current.AllowDevFunctions = true;
        CheatLogic.current.allBlobpediaEntryUnlocked = Melon<CheatCore>.Instance.AllBlobpediaEntryUnlocked.Value;
        CheatLogic.current.allResidentsUnlocked = Melon<CheatCore>.Instance.AllResidentsUnlocked.Value;
        CheatLogic.current.hasAllBlueprints = Melon<CheatCore>.Instance.HasAllBlueprints.Value;
        CheatLogic.current.hasAllRecipes = Melon<CheatCore>.Instance.HasAllRecipes.Value;
        CheatLogic.current.blobAbilityNeedsNoEnergy = Melon<CheatCore>.Instance.BlobAbilityNeedsNoEnergy.Value;
        CheatLogic.current.canBuildWithoutResources = Melon<CheatCore>.Instance.CanBuildWithoutResources.Value;
        CheatLogic.current.canCookWithoutResources = Melon<CheatCore>.Instance.CanCookWithoutResources.Value;
        CheatLogic.current.plantsDoNotNeedWater = Melon<CheatCore>.Instance.PlantsDoNotNeedWater.Value;
        CheatLogic.current.plantsGrowInAllSeasons = Melon<CheatCore>.Instance.PlantsGrowInAllSeasons.Value;
        CheatLogic.current.treesGrowFast = Melon<CheatCore>.Instance.TreesGrowFast.Value;
        CheatLogic.current.destroyResourcesOneHit = Melon<CheatCore>.Instance.DestroyResourcesOneHit.Value;
        CheatLogic.current.doNotLowerEnergy = Melon<CheatCore>.Instance.DoNotLowerEnergy.Value;
        CheatLogic.current.enableSpeedRaise = Melon<CheatCore>.Instance.EnableSpeedRaise.Value;
        CheatLogic.current.skipBattle = Melon<CheatCore>.Instance.SkipBattle.Value;
        CheatLogic.current.skipBattleDungeon = Melon<CheatCore>.Instance.SkipBattleDungeon.Value;
        CheatLogic.current.skipBattleFishing = Melon<CheatCore>.Instance.SkipBattleFishing.Value;
        CheatLogic.current.escapeBattleOnSkip = Melon<CheatCore>.Instance.EscapeBattleOnSkip.Value;
        CheatLogic.current.winBattleOnSkip = Melon<CheatCore>.Instance.WinBattleOnSkip.Value;
    }
}