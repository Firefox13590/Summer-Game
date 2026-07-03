using Globals;
using NaughtyAttributes;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "DefenseBreak", menuName = "Scriptable Objects/Effects/DefenseBreakEffectObject")]
public class DefenseBreakEffectObject : Effect
{
    public bool IsChance = false;
    [ShowIf(nameof(IsChance)), Range(0f, 1f)]
    public float ChancePercent = .5f;

    float BreakAmmount;

    public override void Apply(CharacterObject target)
    {
        bool hitChanceRoll = false;

        if (IsChance)
        {
            if (Random.Range(0f, 1f) <= ChancePercent) hitChanceRoll = true;
        }
        else hitChanceRoll = true;


        if (hitChanceRoll)
        {
            switch (NumberType)
            {
                case NumberType.Flat:
                    BreakAmmount = FlatAmmount;
                    break;
                case NumberType.BasePercent:
                    BreakAmmount = target.StatDef.BaseValue * BasePercentAmmount;
                    break;
                case NumberType.ModifiedPercent:
                    BreakAmmount = target.StatDef.ModifiedValue * ModifiedPercentAmmount;
                    break;
            }
            target.CurrentHp -= BreakAmmount;

            BattleManager.Instance.AddLine2BattleLog(
                $"<i>{target.CharacterName} healed {Math.Round(BreakAmmount)} hp.</i>");
        }
    }
}
