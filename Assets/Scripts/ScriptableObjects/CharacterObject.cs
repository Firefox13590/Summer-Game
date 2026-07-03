using Globals;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterObject", menuName = "Scriptable Objects/Character")]
public class CharacterObject : ScriptableObject
{
    [field: Header("Identification")]
    [field: SerializeField] public string CharacterName { get; private set; }


    [field: Header("Stats")]
    [field: SerializeField] public float MaxHp { get; private set; }
    float _currentHp;
    public float CurrentHp
    {
        get => _currentHp;
        set
        {
            _currentHp = Math.Clamp(value, 0, MaxHp);
            if (_currentHp == 0) Die();
        }
    }
    [field: SerializeField] public float Attack { get; private set; }
    [field: SerializeField] public float Defense { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    public CharacterStat StatDef = new(100);


    public StatModifier[] Modifiers = new StatModifier[10];


    [Header("Effects depending on event")]
    [SerializeReference]
    public Effect[] BattleStartEffects;
    [SerializeReference]
    public Effect[] BattleEndEffects,
        RoundStartEffects, RoundEndEffects,
        TurnStartEffects, TurnEndEffects,
        HitEffects, HurtEffects;


    public event Action<CharacterObject> DeadCharacterEvent;

    private void OnEnable()
    {
        CurrentHp = MaxHp;

        BattleManager.BattleStartEvent += () => ApplyEffectList(BattleStartEffects);
        BattleManager.RoundStartEvent += () => ApplyEffectList(RoundStartEffects);
        BattleManager.TurnStartEvent += () => ApplyEffectList(TurnStartEffects);
        BattleManager.BattleEndEvent += () => ApplyEffectList(BattleEndEffects);
        BattleManager.RoundEndEvent += () => ApplyEffectList(RoundEndEffects);
        BattleManager.TurnEndEvent += () => ApplyEffectList(TurnEndEffects);

        BattleManager.HitEvent += (character) => ApplyEffectList(HitEffects, character);
        BattleManager.HurtEvent += (character) => ApplyEffectList(HurtEffects, character);
    }
    private void OnDisable()
    {
        BattleManager.BattleStartEvent -= () => ApplyEffectList(BattleStartEffects);
        BattleManager.RoundStartEvent -= () => ApplyEffectList(RoundStartEffects);
        BattleManager.TurnStartEvent -= () => ApplyEffectList(TurnStartEffects);
        BattleManager.BattleEndEvent -= () => ApplyEffectList(BattleEndEffects);
        BattleManager.RoundEndEvent -= () => ApplyEffectList(RoundEndEffects);
        BattleManager.TurnEndEvent -= () => ApplyEffectList(TurnEndEffects);

        BattleManager.HitEvent -= (character) => ApplyEffectList(HitEffects, character);
        BattleManager.HurtEvent -= (character) => ApplyEffectList(HurtEffects, character);
    }



    public void Die()
    {
        DeadCharacterEvent?.Invoke(this);
    }
    void ApplyEffectList(Effect[] effects)
    {
        if (effects.Length == 0) return;
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].Apply(this);
        }
    }
    void ApplyEffectList(Effect[] effects, CharacterObject character)
    {
        if (character == this)
        {
            ApplyEffectList(effects);
        }
        else
        {
            if (effects.Length == 0) return;
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].Apply(character);
            }
        }
    }
}
