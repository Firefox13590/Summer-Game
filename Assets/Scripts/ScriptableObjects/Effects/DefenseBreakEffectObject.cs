using Globals;
using NaughtyAttributes;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "DefenseBreak", menuName = "Scriptable Objects/Effects/DefenseBreakEffectObject")]
public class DefenseBreakEffectObject : Effect
{
    public bool isChance = false;
    [ShowIf(nameof(isChance)), Range(0f, 1f)]
    public float chancePercent = .5f;

    float breakAmmount;

    public override void Apply(CharacterObject _, CharacterObject[] targets)
    {
        foreach (CharacterObject target in targets)
        {
            //Debug.Log(target.CharacterName);
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
                        breakAmmount = flatAmmount;
                        break;
                    case NumberType.BasePercent:
                        breakAmmount = target.StatDef.BaseValue * basePercentAmmount;
                        break;
                    case NumberType.ModifiedPercent:
                        breakAmmount = target.StatDef.ModifiedValue * modifiedPercentAmmount;
                        break;
                }

                // https://github.com/Firefox13590/Summer-Game/issues/2
                //statModifier = new(
                //    true,
                //    name,
                //    CounterType.Turn,
                //    characterStat: target.StatDef,
                //    additive: -breakAmmount);
                //statModifier = new(
                //    true,
                //    name: (string.IsNullOrEmpty(statModifier.Name.Trim())) ? name : statModifier.Name.Trim(),
                //    counterType: statModifier.CounterType,
                //    counter: statModifier.Counter,
                //    additive: (numberType == NumberType.Flat) ? -breakAmmount : StatModifier.defaultStatModifier.additive,
                //    multiplicative: (numberType != NumberType.Flat) ? breakAmmount : StatModifier.defaultStatModifier.Multiplicative
                //    );
                //statModifier = new(statModifier);
                //statModifier.characterStatRef = target.StatDef;
                //target.AddStatModifier(statModifier);
                Debug.Log(breakAmmount);
                target.AddStatModifier(new(
                    true,
                    (string.IsNullOrEmpty(statModifier.Name.Trim())) ? name : statModifier.Name.Trim(),
                    statModifier.CounterType,
                    statModifier.Counter,
                    target.StatDef,
                    (numberType == NumberType.Flat) ? -breakAmmount : StatModifier.defaultStatModifier.additive,
                    (numberType != NumberType.Flat) ? breakAmmount : StatModifier.defaultStatModifier.Multiplicative
                    ));

                BattleManager.Instance.AddLine2BattleLog(
                    $"<i>{target.CharacterName} got their defense broken by {Math.Round(breakAmmount)}." +
                    $"Their new defense is {target.StatDef}</i>");
            }
        }
    }
}
