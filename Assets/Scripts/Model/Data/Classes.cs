using Model.SO;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace Model.Data
{
    /// <summary>
    /// Repréentation d'une statistique de personnage
    /// </summary>
    [Serializable]
    public class CharacterStat
    {
        [field: SerializeField]
        public float BaseValue { get; private set; }
        [field: SerializeField, ReadOnly, AllowNesting]
        public float ModifiedValue { get; set; }
        /// <summary>
        /// Le cumul de tous les modificateurs actifs sur cette stat
        /// </summary>
        StatModifier cumulativeModifiers = new();

        public static CharacterStat defaultCharacterStat = new();


        public CharacterStat()
        {
            BaseValue = 0;
            Init();
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



        /// <summary>
        /// Initialise l'objet en affectant la valeur de base à la vlaeur modifiée et en nettoyant le cumul de modificateurs
        /// </summary>
        /// <remarks>
        /// Cette méthode sert essentiellement de réinitialisation
        /// </remarks>
        public void Init()
        {
            ModifiedValue = BaseValue;
            cumulativeModifiers = new();
        }
        /// <summary>
        /// Permet de modifier la stat
        /// </summary>
        /// <param name="modifier">Le modificateur à appliquer</param>
        public void ModifyStat(StatModifier modifier)
        {
            cumulativeModifiers = StatModifier.Combine(cumulativeModifiers, modifier);
            //Debug.Log(cumulativeModifiers);

            //Debug.Log($"Updated CharacterStat from: {this}");
            ModifiedValue = (BaseValue + cumulativeModifiers.additive) * cumulativeModifiers.Multiplicative;
            //Debug.Log($"To: {this}");
        }
    }
    /// <summary>
    /// Représente un modificateur de stat
    /// </summary>
    [Serializable]
    public class StatModifier
    {
        readonly bool isInspectorReadOnly = false;

        [field: SerializeField, EnableIf(nameof(isInspectorReadOnly)), AllowNesting]
        public string Name { get; private set; }
        /// <summary>
        /// Le type de compteur qui affectera le modificateur
        /// </summary>
        [field: SerializeField, EnableIf(nameof(isInspectorReadOnly)), AllowNesting]
        public CounterType CounterType { get; private set; }
        [field: SerializeField, EnableIf(nameof(isInspectorReadOnly)), AllowNesting]
        public int Counter { get; private set; }
        /// <summary>
        /// Une référence vers la <see cref="CharacterStat"/> à modifier
        /// </summary>
        [SerializeReference, ReadOnly, AllowNesting]
        public CharacterStat characterStatRef;
        [EnableIf(nameof(isInspectorReadOnly)), AllowNesting]
        public float additive;
        [SerializeField, EnableIf(nameof(isInspectorReadOnly)), AllowNesting]
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
            isInspectorReadOnly = false;
            Name = "";
            SetNewCounter(0, -1);
            characterStatRef = null;
            additive = 0;
            Multiplicative = 1;
        }
        public StatModifier(StatModifier modifier)
        {
            isInspectorReadOnly = modifier.isInspectorReadOnly;
            Name = modifier.Name;
            SetNewCounter(modifier.CounterType, modifier.Counter);
            characterStatRef = modifier.characterStatRef;
            additive = modifier.additive;
            Multiplicative = modifier.Multiplicative;
        }
        public StatModifier(bool isReadOnly = false, string name = "", CounterType counterType = 0, int counter = -1, CharacterStat characterStat = null, float additive = 0, float multiplicative = 1)
        {
            this.isInspectorReadOnly = isReadOnly;
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



        /// <summary>
        /// Permet de changer le type de compteur ainsi que la valeur de départ du compteur
        /// </summary>
        /// <param name="counterType">Le nouveau type de compteur</param>
        /// <param name="counter">
        /// La nouvelle valeur de départ du compteur. 
        /// Si <paramref name="counter"/> = <c>-1</c>, une valeur par défaut lui sera automatiquement attribué selon le type de compteur.</param>
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
        /// <summary>
        /// D♪0crémente le compteur
        /// </summary>
        public void DecrementCounter() => Counter--;
        /// <summary>
        /// Calcule le cumulatif de deux modificateurs
        /// </summary>
        /// <param name="firstModifier">Le premier modificateur</param>
        /// <param name="secondModifier">Le deuxième modificateur</param>
        /// <returns>Le cumul des deux</returns>
        public static StatModifier Combine(StatModifier firstModifier, StatModifier secondModifier)
        {
            return new(
                additive: firstModifier.additive + secondModifier.additive,
                multiplicative: firstModifier.multiplicative * secondModifier.multiplicative);
        }
    }
    /// <summary>
    /// Représente un effet. Peut être un bonus (buff) ou un malus (debuff).
    /// <para>
    /// Vient automatiquement avec un <see cref="StatModifier"/>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Classe abstraite.
    /// Hérite de <see cref="ScriptableObject"/> au lieu de <see cref="MonoBehaviour"/> pour permettre d'utiliser les instances comme ressources dans l'éditeur.
    /// </remarks>
    [Serializable]
    public abstract class Effect : ScriptableObject
    {
        [Header("Identification et description"), Space(30)]
        public Tag[] tags;

        [Header("Comportement"), Space(30)]
        public bool isChance = false;
        [ShowIf(nameof(isChance)), Range(0f, 1f)]
        public float chancePercent = 1f;

        public bool isStackable = false;
        [ShowIf(nameof(isStackable)), Min(1)]
        public int stackAmmount = 1;

        public bool doesOverride = false;

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


        /// <summary>
        /// Calcule la valeur finale
        /// </summary>
        protected void CalculateFinalAmmount()
        {
            switch (numberType)
            {
                case NumberType.Flat:
                    finalAmmount = flatAmmount;
                    break;
                case NumberType.BasePercent:
                    finalAmmount = basePercentAmmount;
                    break;
                case NumberType.ModifiedPercent:
                    finalAmmount = modifiedPercentAmmount;
                    break;
            }
        }
        /// <summary>
        /// Applique un effet à une cible
        /// </summary>
        /// <param name="target">La cible</param>
        public virtual void Apply(CharacterObject target) { }
        /// <summary>
        /// Applique un effet à plusieurs cibles
        /// </summary>
        /// <remarks>
        /// Variante adaptée pour <see cref="BattleManager.HitEvent"/>
        /// </remarks>
        /// <param name="attacker">L'attaquant, le <see langword="this"/> du <see cref="CharacterObject"/> qui détient l'effet présent dans l'une de ces listes d'effets</param>
        /// <param name="targets">Les cibles</param>
        public virtual void Apply(CharacterObject attacker, CharacterObject[] targets) { }
        /// <summary>
        /// Applique un effet à une cible
        /// </summary>
        /// <remarks>
        /// Variante adaptée pour <see cref="BattleManager.HurtEvent"/>
        /// </remarks>
        /// <param name="defender">Le défenseur, le <see langword="this"/> du <see cref="CharacterObject"/> qui détient l'effet présent dans l'une de ces listes d'effets</param>
        /// <param name="attacker">L'attaquant</param>
        public virtual void Apply(CharacterObject defender, CharacterObject attacker) { }
    }
}