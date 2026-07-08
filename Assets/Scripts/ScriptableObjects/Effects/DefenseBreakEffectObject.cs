using Globals;
using NaughtyAttributes;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "DefenseBreak", menuName = "Scriptable Objects/Effects/DefenseBreakEffectObject")]
public class DefenseBreakEffectObject : Effect
{
    public override void Apply(CharacterObject _, CharacterObject[] targets)
    {
        foreach (CharacterObject target in targets)
        {
            bool hitChanceRoll = false;

            if (isChance)
            {
                if (Random.Range(0f, 1f) <= chancePercent) hitChanceRoll = true;
            }
            else hitChanceRoll = true;


            if (hitChanceRoll)
            {
                switch (numberType)
                {
                    case NumberType.Flat:
                        finalAmmount = flatAmmount;
                        break;
                    case NumberType.BasePercent:
                        finalAmmount = basePercentAmmount;
                        break;
                    case NumberType.ModifiedPercent:
                        finalAmmount = modifiedPercentAmmount;
                        break;
                }

                target.AddStatModifier(new(
                    true,
                    (string.IsNullOrEmpty(statModifier.Name.Trim())) ? name : statModifier.Name.Trim(),
                    statModifier.CounterType,
                    statModifier.Counter,
                    target.StatDef,
                    (numberType == NumberType.Flat) ? finalAmmount : StatModifier.defaultStatModifier.additive,
                    (numberType != NumberType.Flat) ? finalAmmount : StatModifier.defaultStatModifier.Multiplicative
                    ));

                BattleManager.Instance.AddLine2BattleLog(
                    $"<i>{target.CharacterName} got their defense broken by {Math.Round(finalAmmount, 2)}." +
                    $"Their new defense is {target.StatDef}</i>");
            }
        }
    }
}
