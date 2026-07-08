using Globals;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Scriptable Objects/Effects/HealEffectObject")]
public class HealEffectObject : Effect
{
    public override void Apply(CharacterObject target)
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
        target.CurrentHp += finalAmmount;

        BattleManager.Instance.AddLine2BattleLog(
            $"<i>{target.CharacterName} healed {Math.Round(finalAmmount, 2)} hp.</i>");
    }
}
