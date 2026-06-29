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

    #region Character
    [Serializable]
    public struct CharacterStat
    {
        [field: SerializeField]
        private float BaseValue { get; set; }
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
    #endregion
}
