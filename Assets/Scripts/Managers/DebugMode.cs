using Globals;
using NaughtyAttributes;
using UnityEngine;

public class DebugMode : Singleton<DebugMode>
{
    public bool activateDebugMode = false;

    [Header("Character"), Space(30)]
    [ShowIf(nameof(activateDebugMode)), Tooltip("Montre ModifiedValue et BaseValue au lieu de juste ModifiedValue")]
    [SerializeField]
    bool alsoOutputBaseValue;
    public bool AlsoOutputBaseValue
    {
        get => activateDebugMode && alsoOutputBaseValue;
        set => alsoOutputBaseValue = value;
    }
}
