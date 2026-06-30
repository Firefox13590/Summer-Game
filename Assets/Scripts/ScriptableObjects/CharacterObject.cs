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
    private float _currentHp;
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
    [field: SerializeField] public float Speed { get; private set; }


    [Header("Effects depending on event")]
    [SerializeReference]
    public Effect[] BattleStartEffects;
    [SerializeReference]
    public Effect[] BattleEndEffects,
        RoundStartEffects, RoundEndEffects,
        TurnStartEffects, TurnEndEffects;


    public event Action<CharacterObject> DeadCharacterEvent;

    private void OnEnable()
    {
        CurrentHp = MaxHp;
        BattleManager.TurnStartEvent += () => ApplyEffectList(TurnStartEffects);
    }
    private void OnDisable()
    {
        BattleManager.TurnStartEvent -= () => ApplyEffectList(TurnStartEffects);
    }



    public void Die()
    {
        DeadCharacterEvent?.Invoke(this);
    }
    private void ApplyEffectList(Effect[] effects)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].Apply(this);
        }
    }
}
