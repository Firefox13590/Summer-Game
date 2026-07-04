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
        Weak = -1,
        Normal,
        Strong
    }
    public enum AffinityType
    {
        Blunt = 1,
        Slash,
        Pierce,
        Fire,
        Water,
        Thunder,
        Earth,
        Light,
        Dark,

        Absolute = 11,
        Typeless,

        Physical = Blunt | Slash | Pierce,
        Magic = Fire | Water | Thunder | Earth | Light | Dark,
        Special = Absolute | Typeless
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
    public class CharacterStat
    {
        [field: SerializeField]
        public float BaseValue { get; private set; }
        [field: SerializeField, ReadOnly, AllowNesting]
        public float ModifiedValue { get; set; }

        public CharacterStat() { }
        public CharacterStat(float baseValue)
        {
            BaseValue = ModifiedValue = baseValue;
        }
        public override string ToString()
        {
            return (DebugMode.Instance.AlsoOutputBaseValue) ? $"{ModifiedValue} ({BaseValue})" : ModifiedValue.ToString();
        }




        public void ModifyStat()
        {

        }
    }

    [Serializable]
    public class StatModifier
    {
        CharacterStat characterStatRef;
        public int additive = 0;

        [SerializeField]
        float mulitplicative = 1;
        public float Mulitplicative
        {
            get => mulitplicative;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Multiplicative modifier can't be negative");
                mulitplicative = value;
            }
        }
        public static StatModifier DefaultModifier;

        public StatModifier() { }
        public StatModifier(int additive = 0, float multiplicative = 1)
        {
            this.additive = additive;
            Mulitplicative = multiplicative;
        }
    }


    [Serializable]
    public abstract class Effect : ScriptableObject
    {
        public Tag[] tags;

        public NumberType numberType = NumberType.Flat;
        [ShowIf(nameof(numberType), NumberType.Flat), Min(0)]
        public float flatAmmount = 10;
        [ShowIf(nameof(numberType), NumberType.BasePercent), Min(0)]
        public float basePercentAmmount = .1f;
        [ShowIf(nameof(numberType), NumberType.ModifiedPercent), Min(0)]
        public float modifiedPercentAmmount = .2f;


        public virtual void Apply() { }
        public virtual void Apply(CharacterObject target) { }
        public virtual void Apply(CharacterObject attacker, CharacterObject[] targets) { }
        public virtual void Apply(CharacterObject defender, CharacterObject attacker) { }
    }
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance { get { return instance; } }

        protected virtual void Awake()
        {
            //Debug.Log("Interface singleton");
            if (Instance == null)
            {
                instance = this as T;
            }
            else Destroy(gameObject);
        }
    }
}
