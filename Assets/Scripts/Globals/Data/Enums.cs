using UnityEngine;

namespace Globals.Data.Enums
{
    /// <summary>
    /// Définit le niveau de scaling d'un nombre
    /// </summary>
    public enum NumberType
    {
        /// <summary>
        /// Valeur fixe qui ne dépend pas d'une stat. Ex: +10 de dégâts.
        /// </summary>
        /// <remarks>
        /// Valeur additive.
        /// </remarks>
        Flat,
        /// <summary>
        /// Valeur qui dépend d'une stat de base. Ex: 10% de la stat de base.
        /// </summary>
        /// <remarks>
        /// Valeur multiplicative. Se base sur <see cref="CharacterStat.BaseValue"/>.
        /// </remarks>
        BasePercent,
        /// <summary>
        /// Valeur qui dépend d'une stat de base. Ex: 10% de la stat modifiée.
        /// </summary>
        /// <remarks>
        /// Valeur multiplicative. Se base sur <see cref="CharacterStat.ModifiedValue"/>.
        /// </remarks>
        ModifiedPercent
    }
    /// <summary>
    /// Le niveau de résistance face à une affinité
    /// </summary>
    public enum AffinityResistance
    {
        /// <summary>
        /// Résistance faible. +50% de dégâts reçu.
        /// </summary>
        Weak = -1,
        /// <summary>
        /// Résistance normale. Aucun changement.
        /// </summary>
        Normal,
        /// <summary>
        /// R♪0sistance forte. -50% de dégâts reçu.
        /// </summary>
        Strong
    }
    public enum AffinityType
    {
        /// <summary>
        /// Peut étourdir
        /// </summary>
        Blunt = 1,
        /// <summary>
        /// Peut faire saigner
        /// </summary>
        Slash,
        /// <summary>
        /// Chances augmentées de crit
        /// </summary>
        Pierce,
        /// <summary>
        /// Peut brûler
        /// </summary>
        Fire,
        /// <summary>
        /// Peut geler
        /// </summary>
        Ice,
        /// <summary>
        /// Peut paralyser
        /// </summary>
        Thunder,
        /// <summary>
        /// Peut ralentir
        /// </summary>
        Earth,
        /// <summary>
        /// Peut éblouir
        /// </summary>
        Light,
        /// <summary>
        /// Peut apeurer
        /// </summary>
        Dark,

        /// <summary>
        /// Ignore la défense
        /// </summary>
        Absolute = 11,
        /// <summary>
        /// Ne peut pas être bloqué
        /// </summary>
        Typeless,

        /// <summary>
        /// Catégorie d'attaques physiques
        /// </summary>
        Physical = Blunt | Slash | Pierce,
        /// <summary>
        /// Catégorie d'attaques magiques
        /// </summary>
        Magic = Fire | Ice | Thunder | Earth | Light | Dark,
        /// <summary>
        /// Catégorie d'attaques qui brisent les conventions
        /// </summary>
        Special = Absolute | Typeless
    }
    /// <summary>
    /// Définit comment une attaque est inflgée
    /// </summary>
    public enum AttackMethod
    {
        /// <summary>
        /// Attaque de contact
        /// </summary>
        Melee,
        /// <summary>
        /// Attaque à distance
        /// </summary>
        Range,
        /// <summary>
        /// Attaque instantanée et difficilement évitable
        /// </summary>
        Apparition,
        /// <summary>
        /// Grosse attaque qui frappe une fois
        /// </summary>
        SingleHit,
        /// <summary>
        /// Attaque répétée
        /// </summary>
        MultiHit,
        /// <summary>
        /// Attaque dans une zone d'effet
        /// </summary>
        /// <remarks>
        /// AoE - Area of Effect
        /// </remarks>
        Aoe
    }
    /// <summary>
    /// Définit différentes étiquettes afin de décrire quelque chose
    /// </summary>
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
        DamageOverTime,
        Block,
        Dodge
    }
    /// <summary>
    /// Définit le type de compteur
    /// </summary>
    public enum CounterType
    {
        /// <summary>
        /// Une condition doit d♪0terminer la durée d'un effet ou modificateur plutôt que le compteur
        /// </summary>
        Infinite = -1,
        /// <summary>
        /// Décrémente le compteur à chaque tour
        /// </summary>
        Turn,
        /// <summary>
        /// Décrémente le compteur à chaque round
        /// </summary>
        Round,
        /// <summary>
        /// Décrémente le compteur à chaque batail
        /// </summary>
        Battle,
        /// <summary>
        /// Décrémente le compteur à chaque coup infligé
        /// </summary>
        Hit,
        /// <summary>
        /// Décrémente le compteur à chaque coup reçu
        /// </summary>
        Hurt
    }
}