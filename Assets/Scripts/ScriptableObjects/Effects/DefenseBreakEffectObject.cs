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
            Debug.Log(target.CharacterName);
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
                target.StatDef.ModifiedValue -= breakAmmount;

                BattleManager.Instance.AddLine2BattleLog(
                    $"<i>{target.CharacterName} got their defense broken by {Math.Round(breakAmmount)}." +
                    $"Their new defense is {target.StatDef}</i>");
            }
        }
    }
}
