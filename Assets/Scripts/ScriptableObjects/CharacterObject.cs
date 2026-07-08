using Globals;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "CharacterObject", menuName = "Scriptable Objects/Character")]
public class CharacterObject : ScriptableObject
{
    [field: Header("Identification"), Space(30)]
    [field: SerializeField] public string CharacterName { get; private set; }


    [field: Header("Stats"), Space(30)]
    [field: SerializeField] public float MaxHp { get; private set; }
    float currentHp;
    public float CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = Math.Clamp(value, 0, MaxHp);
            if (currentHp == 0) Die();
        }
    }
    [field: SerializeField] public float Attack { get; private set; }
    [field: SerializeField] public float Defense { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    public CharacterStat StatDef = new(100);


    public bool IsAlive => CurrentHp > 0;


    const int MODIFIER_ARRAY_SIZE = 10;
    [SerializeReference]
    public StatModifier[] modifiers = new StatModifier[MODIFIER_ARRAY_SIZE];


    [Header("Effets dépendant d'un évènement"), Space(30)]
    [SerializeReference]
    public Effect[] battleStartEffects;
    [SerializeReference]
    public Effect[] battleEndEffects,
        roundStartEffects, roundEndEffects,
        turnStartEffects, turnEndEffects,
        hitEffects, hurtEffects;


    public event Action<CharacterObject> DeadCharacterEvent;


    private void OnEnable()
    {
        CurrentHp = MaxHp;
        StatDef.Init();
        Array.Fill(modifiers, null);

        BattleManager.BattleStartEvent += () => ApplyEffectList(battleStartEffects);
        BattleManager.RoundStartEvent += () => ApplyEffectList(roundStartEffects);
        BattleManager.TurnStartEvent += (_) => ApplyEffectList(turnStartEffects);
        BattleManager.BattleEndEvent += () => ApplyEffectList(battleEndEffects);
        BattleManager.RoundEndEvent += () => ApplyEffectList(roundEndEffects);
        BattleManager.TurnEndEvent += (_) => ApplyEffectList(turnEndEffects);

        BattleManager.HitEvent += (attacker, targets) => { if (attacker == this) ApplyEffectList(hitEffects, attacker, targets); };
        BattleManager.HurtEvent += (defender, attacker) => { if (defender == this) ApplyEffectList(hurtEffects, defender, attacker); };

        BattleManager.BattleStartEvent += () => { DecrementCounters(CounterType.Battle); };
        BattleManager.TurnStartEvent += (character) => { DecrementCounters(CounterType.Turn, character); };
        BattleManager.RoundEndEvent += () => { DecrementCounters(CounterType.Round); };
        BattleManager.HitEvent += (_, _) => { DecrementCounters(CounterType.Hit); };
        BattleManager.HurtEvent += (_, _) => { DecrementCounters(CounterType.Hurt); };

    }
    private void OnDisable()
    {
        BattleManager.BattleStartEvent -= () => ApplyEffectList(battleStartEffects);
        BattleManager.RoundStartEvent -= () => ApplyEffectList(roundStartEffects);
        BattleManager.TurnStartEvent -= (_) => ApplyEffectList(turnStartEffects);
        BattleManager.BattleEndEvent -= () => ApplyEffectList(battleEndEffects);
        BattleManager.RoundEndEvent -= () => ApplyEffectList(roundEndEffects);
        BattleManager.TurnEndEvent -= (_) => ApplyEffectList(turnEndEffects);

        BattleManager.HitEvent -= (attacker, targets) => { if (attacker == this) ApplyEffectList(hitEffects, attacker, targets); };
        BattleManager.HurtEvent -= (defender, attacker) => { if (defender == this) ApplyEffectList(hurtEffects, defender, attacker); };

        BattleManager.BattleStartEvent -= () => { DecrementCounters(CounterType.Battle); };
        BattleManager.TurnStartEvent -= (character) => { DecrementCounters(CounterType.Turn, character); };
        BattleManager.RoundEndEvent -= () => { DecrementCounters(CounterType.Round); };
        BattleManager.HitEvent -= (_, _) => { DecrementCounters(CounterType.Hit); };
        BattleManager.HurtEvent -= (_, _) => { DecrementCounters(CounterType.Hurt); };
    }
    private void OnValidate()
    {
        if (modifiers.Length != MODIFIER_ARRAY_SIZE)
        {
            Debug.LogWarning("Le tableau de modificateurs ne peut pas être redimensionné");
            Array.Resize(ref modifiers, MODIFIER_ARRAY_SIZE);
        }
    }



    public void Die()
    {
        DeadCharacterEvent?.Invoke(this);
    }
    void ApplyEffectList(Effect[] effects)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i] == null) throw new ArgumentNullException("Un effet ne peut pas être nul");
            effects[i].Apply(this);
        }
    }
    void ApplyEffectList(Effect[] effects, CharacterObject attacker, CharacterObject[] targets)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i] == null) throw new ArgumentNullException("Un effet ne peut pas être nul");
            effects[i].Apply(attacker, targets);
        }
    }
    void ApplyEffectList(Effect[] effects, CharacterObject defender, CharacterObject attacker)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i] == null) throw new ArgumentNullException("Un effet ne peut pas être nul");
            effects[i].Apply(defender, attacker);
        }
    }
    public void UpdateStats()
    {
        foreach (var modifier in modifiers)
        {
            if (modifier == null) continue;
            else if (modifier.characterStatRef == null) throw new ArgumentNullException("La référence de CharacterStat d'un StatModifier ne peut pas être nul.");
            else
            {
                //Debug.Log(modifier.Name);
                modifier.characterStatRef.ModifyStat(modifier);
            }
        }
        //Debug.Log("Stats updated");
    }
    private void ResetStats()
    {
        foreach (var modifier in modifiers)
        {
            if (modifier == null) continue;
            else if (modifier.characterStatRef == null) throw new ArgumentNullException("La référence de CharacterStat d'un StatModifier ne peut pas être nul.");
            else
            {
                modifier.characterStatRef.Init();
            }
        }
        //Debug.Log("Stats reset");
        UpdateStats();
    }
    public void AddStatModifier(StatModifier modifier)
    {
        int indexLowestCounter = -1, lowestCounter = int.MaxValue;
        bool modifierAdded = false;

        if (modifiers.FirstOrDefault(mod => mod != null && mod.Name == modifier.Name)?.Name == modifier.Name) modifierAdded = true;

        if (!modifierAdded)
        {
            for (int i = 0; i < modifiers.Length; i++)
            {
                if (modifiers[i] == null)
                {
                    modifiers[i] = modifier;
                    modifierAdded = true;
                    break;
                }

                if (modifiers[i].Counter < lowestCounter)
                {
                    lowestCounter = modifiers[i].Counter;
                    indexLowestCounter = i;
                }
            }

            if (indexLowestCounter != -1 && !modifierAdded)
            {
                modifiers[indexLowestCounter] = modifier;
                modifierAdded = true;
            }
        }

        //UpdateStats();
        if (modifierAdded) ResetStats();
    }
    void DecrementCounters(CounterType counterType)
    {
        for (int i = 0; i < modifiers.Length; i++)
        {
            if (modifiers[i] == null) continue;
            if (modifiers[i].CounterType == counterType)
            {
                modifiers[i].DecrementCounter();
                if (modifiers[i].Counter == 0)
                {
                    modifiers[i] = null;
                    ResetStats();
                }
            }
        }
    }
    void DecrementCounters(CounterType counterType, CharacterObject character)
    {
        if (character == this) DecrementCounters(counterType);
    }
}
