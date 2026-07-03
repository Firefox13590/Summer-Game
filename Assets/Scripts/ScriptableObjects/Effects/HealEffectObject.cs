using Globals;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Scriptable Objects/Effects/HealEffectObject")]
public class HealEffectObject : Effect
{
    float HealAmmount;

    public override void Apply(CharacterObject target)
    {
        switch (NumberType)
        {
            case NumberType.Flat:
                HealAmmount = FlatAmmount;
                break;
            case NumberType.BasePercent:
                HealAmmount = target.MaxHp * BasePercentAmmount;
                break;
            case NumberType.ModifiedPercent:
                HealAmmount = target.CurrentHp * ModifiedPercentAmmount;
                break;
        }
        target.CurrentHp += HealAmmount;

        BattleManager.Instance.AddLine2BattleLog(
            $"<i>{target.CharacterName} healed {Math.Round(HealAmmount)} hp.</i>");
    }
}
