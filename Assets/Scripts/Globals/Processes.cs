using System;
using System.Collections;
using UnityEngine;

namespace Globals
{
    public struct StatModifier
    {
        public int Additive;

        private float _mulitplicative;
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
        public abstract void Apply(CharacterObject target);
    }
}