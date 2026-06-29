using Globals;
using NaughtyAttributes;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "HealEffectObject", menuName = "Scriptable Objects/Effects/HealEffectObject")]
public class HealEffectObject : Effect
{
    public NumberType NumberType = NumberType.Flat;
    [ShowIf(nameof(NumberType), NumberType.Flat)]
    public float FlatHeal = 10;
    [ShowIf(nameof(NumberType), NumberType.BasePercent)]
    [Range(0f, 1f)]
    public float BasePercentHeal = .1f;
    [ShowIf(nameof(NumberType), NumberType.ModifiedPercent)]
    [Range(0f, 1f)]
    public float ModifiedPercentHeal = .2f;

    public override void Apply(CharacterObject target)
    {
        switch (NumberType)
        {
            case NumberType.Flat:
                target.CurrentHp += FlatHeal;
                break;
            case NumberType.BasePercent:
                target.CurrentHp += BasePercentHeal;
                break;
            case NumberType.ModifiedPercent:
                target.CurrentHp += ModifiedPercentHeal;
                break;
        }
    }
}
