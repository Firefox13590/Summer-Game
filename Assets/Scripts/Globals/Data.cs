using NaughtyAttributes;
using System;
using System.Xml.Linq;
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
        StatModifier cumulativeModifiers = new();

        public static CharacterStat defaultCharacterStat = new();


        public CharacterStat()
        {
            BaseValue = 0;
            ModifiedValue = 0;
        }
        public CharacterStat(float baseValue)
        {
            BaseValue = baseValue;
            Init();
        }
        public override string ToString()
        {
            return (DebugMode.Instance.AlsoOutputBaseValue) ? $"{ModifiedValue} ({BaseValue})" : ModifiedValue.ToString();
        }



        public void Init()
        {
            ModifiedValue = BaseValue;
            cumulativeModifiers = new();
        }
        public void ModifyStat(StatModifier modifier)
        {
            cumulativeModifiers = StatModifier.Combine(cumulativeModifiers, modifier);
            //Debug.Log(cumulativeModifiers);

            //Debug.Log($"Updated CharacterStat from: {this}");
            ModifiedValue = (BaseValue + cumulativeModifiers.additive) * cumulativeModifiers.Multiplicative;
            //Debug.Log($"To: {this}");
        }
    }

    [Serializable]
    public class StatModifier
    {
        readonly bool isReadOnly = false;

        [field: SerializeField, EnableIf(nameof(isReadOnly)), AllowNesting]
        public string Name { get; private set; }
        [field: SerializeField, EnableIf(nameof(isReadOnly)), AllowNesting]
        public CounterType CounterType { get; private set; }
        [field: SerializeField, EnableIf(nameof(isReadOnly)), AllowNesting]
        public int Counter { get; private set; }
        [SerializeReference, ReadOnly, AllowNesting]
        public CharacterStat characterStatRef;
        [EnableIf(nameof(isReadOnly)), AllowNesting]
        public float additive;
        [SerializeField, EnableIf(nameof(isReadOnly)), AllowNesting]
        float multiplicative;
        public float Multiplicative
        {
            get => multiplicative;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Multiplicative modifier can't be negative");
                multiplicative = value;
            }
        }
        public static StatModifier defaultStatModifier = new();


        public StatModifier()
        {
            isReadOnly = false;
            Name = "";
            SetNewCounter(0, -1);
            characterStatRef = null;
            additive = 0;
            Multiplicative = 1;
        }
        public StatModifier(StatModifier modifier)
        {
            isReadOnly = modifier.isReadOnly;
            Name = modifier.Name;
            SetNewCounter(modifier.CounterType, modifier.Counter);
            characterStatRef = modifier.characterStatRef;
            additive = modifier.additive;
            Multiplicative = modifier.Multiplicative;
        }
        public StatModifier(bool isReadOnly = false, string name = "", CounterType counterType = 0, int counter = -1, CharacterStat characterStat = null, float additive = 0, float multiplicative = 1)
        {
            this.isReadOnly = isReadOnly;
            Name = name;
            SetNewCounter(counterType, counter);
            characterStatRef = characterStat;
            this.additive = additive;
            Multiplicative = multiplicative;
        }
        public override string ToString()
        {
            return $"{Name}: {additive}, x{multiplicative}";
        }



        public void SetNewCounter(CounterType counterType, int counter = -1)
        {
            CounterType = counterType;
            if (counter == -1)
            {
                // switch expression au lieu d'un switch statement parce que c'est uniquement des affectations
                Counter = CounterType switch
                {
                    CounterType.Turn or CounterType.Hit or CounterType.Hurt => 3,
                    CounterType.Round => 2,
                    CounterType.Battle => 1,
                    _ => counter,
                };
            }
            else Counter = counter;
        }
        public void DecrementCounter() => Counter--;
        public static StatModifier Combine(StatModifier firstModifier, StatModifier secondModifier)
        {
            return new(
                additive: firstModifier.additive + secondModifier.additive,
                multiplicative: firstModifier.multiplicative * secondModifier.multiplicative);
        }
    }


    [Serializable]
    public abstract class Effect : ScriptableObject
    {
        [Header("Identification et description"), Space(30)]
        public Tag[] tags;

        public bool isChance = false;
        [ShowIf(nameof(isChance)), Range(0f, 1f)]
        public float chancePercent = .5f;

        [Header("Valeur de l'effet"), Space(30)]
        public NumberType numberType = NumberType.Flat;
        [ShowIf(nameof(numberType), NumberType.Flat)]
        [Tooltip("Valeur additive")]
        public float flatAmmount = 10;
        [ShowIf(nameof(numberType), NumberType.BasePercent), Min(0)]
        [Tooltip("Valeur multiplicative. Échelle de 0 à 1. Ex: 0.5 -> 50%. Se base sur BaseValue")]
        public float basePercentAmmount = .1f;
        [ShowIf(nameof(numberType), NumberType.ModifiedPercent), Min(0)]
        [Tooltip("Valeur multiplicative. Échelle de 0 à 1. Ex: 0.5 -> 50%. Se base sur ModifiedValue")]
        public float modifiedPercentAmmount = .2f;
        protected float finalAmmount;

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
