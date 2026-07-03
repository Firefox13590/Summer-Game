using Globals;
using NaughtyAttributes;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "HealEffectObject", menuName = "Scriptable Objects/Effects/Heal")]
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

    float HealAmmount;

    public override void Apply(CharacterObject target)
    {
        switch (NumberType)
        {
            case NumberType.Flat:
                HealAmmount = FlatHeal;
                break;
            case NumberType.BasePercent:
                HealAmmount = target.MaxHp * BasePercentHeal;
                break;
            case NumberType.ModifiedPercent:
                HealAmmount = target.CurrentHp * ModifiedPercentHeal;
                break;
        }
        target.CurrentHp += HealAmmount;

        BattleManager.Instance.AddLine2BattleLog(
            $"<i>{target.CharacterName} healed {Math.Round(HealAmmount)} hp.</i>");
    }
}
