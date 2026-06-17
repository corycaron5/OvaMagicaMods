using Il2CppInControl;
using MelonLoader;
using UnityEngine;

namespace EggCore.Inputs;

[RegisterTypeInIl2Cpp]
public class EggPlayerActionSet : PlayerActionSet
{
    public Dictionary<KeyCode, PlayerAction> CustomActions = new ();
    
    public EggPlayerActionSet(IntPtr ptr) : base(ptr) { }

    public EggPlayerActionSet(Dictionary<KeyCode, string> actionNames)
    {
        foreach (var actionName in actionNames)
        {
            CustomActions.Add(actionName.Key, CreatePlayerAction(actionName.Value));
        }
    }
}