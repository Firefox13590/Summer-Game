using NaughtyAttributes;
using System;
using UnityEngine;

namespace Globals
{
    public enum NumberType
    {
        Flat,
        BasePercent,
        ModifiedPercent
    }
    public enum AffinityResistance
    {
        Normal,
        Weak,
        Strong
    }
    public enum Tag
    {
        Health,
        Attack,
        Defense,
        Speed,
        Heal,
        Support,
        Offensive,
        Defensive,
        Chance,
        Survivability,
        Damage,
        DamageOverTime
    }



    [Serializable]
    public struct CharacterStat
    {
        [field: SerializeField]
        public float BaseValue { get; private set; }
        [field: SerializeField, ReadOnly, AllowNesting]
        public float ModifiedValue { get; private set; }

        public CharacterStat(float baseValue = 100)
        {
            BaseValue = ModifiedValue = baseValue;
        }



        public override readonly string ToString()
        {
            return (BaseValue == ModifiedValue) ? BaseValue.ToString() : $"{ModifiedValue} ({BaseValue})";
        }
    }

    [Serializable]
    public struct StatModifier
    {
        public int Additive;

        [SerializeField] float _mulitplicative;
        public float Mulitplicative
        {
            get => _mulitplicative;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Multiplicative modifier can't be negative");
                _mulitplicative = value;
            }
        }

        public StatModifier(int additive = 0, float multiplicative = 1) : this()
        {
            Additive = additive;
            Mulitplicative = multiplicative;
        }
    }


    [Serializable]
    public abstract class Effect : ScriptableObject
    {
        public Tag[] Tags;

        public NumberType NumberType = NumberType.Flat;
        [ShowIf(nameof(NumberType), NumberType.Flat), Min(0)]
        public float FlatAmmount = 10;
        [ShowIf(nameof(NumberType), NumberType.BasePercent), Min(0)]
        public float BasePercentAmmount = .1f;
        [ShowIf(nameof(NumberType), NumberType.ModifiedPercent), Min(0)]
        public float ModifiedPercentAmmount = .2f;

        public abstract void Apply(CharacterObject target);
    }
}
