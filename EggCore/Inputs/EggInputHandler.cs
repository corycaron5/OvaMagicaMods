using MelonLoader;
using UnityEngine;

namespace EggCore.Inputs;

[RegisterTypeInIl2Cpp]
public class EggInputHandler : MonoBehaviour
{
    private EggPlayerActionSet EggPlayerActions;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public EggInputHandler(IntPtr ptr) : base(ptr) { }

    public EggInputHandler(EggPlayerActionSet playerActions)
    {
        EggPlayerActions = playerActions;
    }
}