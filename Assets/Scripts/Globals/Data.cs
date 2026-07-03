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
        float BaseValue { get; set; }
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

    public struct StatModifier
    {
        public int Additive;

        float _mulitplicative;
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



    public abstract class Effect : ScriptableObject
    {
        public Tag[] Tags;
        public abstract void Apply(CharacterObject target);
    }
}
