using Globals;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Scriptable Objects/Effects/HealEffectObject")]
public class HealEffectObject : Effect
{
    float healAmmount;

    public override void Apply(CharacterObject target)
    {
        switch (numberType)
        {
            case NumberType.Flat:
                healAmmount = flatAmmount;
                break;
            case NumberType.BasePercent:
                healAmmount = target.MaxHp * basePercentAmmount;
                break;
            case NumberType.ModifiedPercent:
                healAmmount = target.CurrentHp * modifiedPercentAmmount;
                break;
        }
        target.CurrentHp += healAmmount;

        BattleManager.Instance.AddLine2BattleLog(
            $"<i>{target.CharacterName} healed {Math.Round(healAmmount)} hp.</i>");
    }
}
