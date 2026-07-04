using Globals;
using System;
using UnityEngine;

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
    public StatModifier[] modifiers = new StatModifier[MODIFIER_ARRAY_SIZE];


    [Header("Effets dépendant d'un évènement"), Space(30)]
    [SerializeReference]
    public Effect[] battleStartEffects;
    [SerializeReference]
    public Effect[] battleEndEffects,
        roundStartEffects, roundEndEffects,
        turnStartEffects, turnEndEffects,
        titEffects, hurtEffects;


    public event Action<CharacterObject> DeadCharacterEvent;

    private void OnEnable()
    {
        CurrentHp = MaxHp;
        Array.Fill(modifiers, null);

        BattleManager.BattleStartEvent += () => ApplyEffectList(battleStartEffects);
        BattleManager.RoundStartEvent += () => ApplyEffectList(roundStartEffects);
        BattleManager.TurnStartEvent += () => ApplyEffectList(turnStartEffects);
        BattleManager.BattleEndEvent += () => ApplyEffectList(battleEndEffects);
        BattleManager.RoundEndEvent += () => ApplyEffectList(roundEndEffects);
        BattleManager.TurnEndEvent += () => ApplyEffectList(turnEndEffects);

        BattleManager.HitEvent += (attacker, targets) => { if (attacker == this) ApplyEffectList(titEffects, attacker, targets); };
        BattleManager.HurtEvent += (defender, attacker) => { if (defender == this) ApplyEffectList(hurtEffects, defender, attacker); };
    }
    private void OnDisable()
    {
        BattleManager.BattleStartEvent -= () => ApplyEffectList(battleStartEffects);
        BattleManager.RoundStartEvent -= () => ApplyEffectList(roundStartEffects);
        BattleManager.TurnStartEvent -= () => ApplyEffectList(turnStartEffects);
        BattleManager.BattleEndEvent -= () => ApplyEffectList(battleEndEffects);
        BattleManager.RoundEndEvent -= () => ApplyEffectList(roundEndEffects);
        BattleManager.TurnEndEvent -= () => ApplyEffectList(turnEndEffects);

        BattleManager.HitEvent -= (attacker, targets) => { if (attacker == this) ApplyEffectList(titEffects, attacker, targets); };
        BattleManager.HurtEvent -= (defender, attacker) => { if (defender == this) ApplyEffectList(hurtEffects, defender, attacker); };
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
        if (effects.Length == 0) return;
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i] == null) throw new ArgumentNullException("Un effet ne peut pas être nul");
            effects[i].Apply(this);
        }
    }
    void ApplyEffectList(Effect[] effects, CharacterObject attacker, CharacterObject[] targets)
    {
        if (effects.Length == 0) return;
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i] == null) throw new ArgumentNullException("Un effet ne peut pas être nul");
            effects[i].Apply(attacker, targets);
        }
    }
    void ApplyEffectList(Effect[] effects, CharacterObject defender, CharacterObject attacker)
    {
        if (effects.Length == 0) return;
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i] == null) throw new ArgumentNullException("Un effet ne peut pas être nul");
            effects[i].Apply(defender, attacker);
        }
    }
}
