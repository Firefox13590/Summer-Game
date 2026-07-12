using Globals.Data.Classes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Scriptable Objects/Effects/HealEffectObject")]
public class HealEffectObject : Effect
{
    public override void Apply(CharacterObject target)
    {
        CalculateFinalAmmount();
        target.CurrentHp += finalAmmount;

        BattleManager.Instance.AddLine2BattleLog(
            $"<i>{target.CharacterName} healed {Math.Round(finalAmmount, 2)} hp.</i>");
    }
}
