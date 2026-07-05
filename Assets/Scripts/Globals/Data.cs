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
    public enum CounterType
    {
        Infinite = -1,
        Turn,
        Round,
        Battle,
        Hit,
        Hurt
    }



    [Serializable]
    public class CharacterStat
    {
        [field: SerializeField]
        public float BaseValue { get; private set; }
        [field: SerializeField, ReadOnly, AllowNesting]
        public float ModifiedValue { get; set; }
        public static CharacterStat defaultCharacterStat = new();

        public CharacterStat()
        {
            BaseValue = 0;
            ModifiedValue = 0;
        }
        public CharacterStat(float baseValue)
        {
            BaseValue = ModifiedValue = baseValue;
        }
        public override string ToString()
        {
            return (DebugMode.Instance.AlsoOutputBaseValue) ? $"{ModifiedValue} ({BaseValue})" : ModifiedValue.ToString();
        }




        public void ModifyStat(StatModifier modifier)
        {
            Debug.Log($"Updated CharacterStat from: {this}");
            ModifiedValue = (BaseValue + modifier.additive) * modifier.Mulitplicative;
            Debug.Log($"To: {this}");
        }
    }

    [Serializable]
    public class StatModifier
    {
        readonly bool isReadOnly = false;
        [field: SerializeField, EnableIf(nameof(isReadOnly)), AllowNesting]
        public CounterType CounterType { get; private set; }
        [field: SerializeField, EnableIf(nameof(isReadOnly)), AllowNesting]
        public int Counter { get; private set; }
        [EnableIf(nameof(isReadOnly)), AllowNesting]
        public CharacterStat characterStatRef;
        [EnableIf(nameof(isReadOnly)), AllowNesting]
        public float additive;

        [SerializeField, EnableIf(nameof(isReadOnly)), AllowNesting]
        float mulitplicative;
        public float Mulitplicative
        {
            get => mulitplicative;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Multiplicative modifier can't be negative");
                mulitplicative = value;
            }
        }
        public static StatModifier defaultStatModifier = new();

        public StatModifier()
        {
            isReadOnly = false;
            CounterType = 0;
            Counter = -1;
            characterStatRef = null;
            additive = 0;
            Mulitplicative = 1;
        }
        public StatModifier(bool isReadOnly, CounterType counterType = 0, int counter = 0, CharacterStat characterStat = null, float additive = 0, float multiplicative = 1)
        {
            this.isReadOnly = isReadOnly;
            CounterType = counterType;
            Counter = counter;
            characterStatRef = characterStat;
            this.additive = additive;
            Mulitplicative = multiplicative;
        }
        public override string ToString()
        {
            return $"+{additive}, x{mulitplicative}";
        }
        public void SetNewCounter(CounterType counterType, int counter = -1)
        {
            CounterType = counterType;
            if (counter == -1)
            {
                switch (CounterType)
                {
                    case CounterType.Turn:
                    case CounterType.Hit:
                    case CounterType.Hurt:
                        Counter = 3;
                        break;
                    case CounterType.Round:
                        Counter = 2;
                        break;
                    case CounterType.Battle:
                        Counter = 1;
                        break;
                    default:
                        Counter = counter;
                        break;
                }
            }
            else Counter = counter;
        }
    }


    [Serializable]
    public abstract class Effect : ScriptableObject
    {
        [Header("Identification et description"), Space(30)]
        public Tag[] tags;

        [Header("Valeur de l'effet"), Space(30)]
        public NumberType numberType = NumberType.Flat;
        [ShowIf(nameof(numberType), NumberType.Flat), Min(0)]
        public float flatAmmount = 10;
        [ShowIf(nameof(numberType), NumberType.BasePercent), Min(0)]
        public float basePercentAmmount = .1f;
        [ShowIf(nameof(numberType), NumberType.ModifiedPercent), Min(0)]
        public float modifiedPercentAmmount = .2f;

        [Header("Modificateur de stat, si nécessaire"), Space(30)]
        public StatModifier statModifier = new(true);


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
