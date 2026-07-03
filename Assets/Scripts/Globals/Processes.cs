using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Globals
{
    public static class Calculations
    {
        public static int CalculateDamage(CharacterObject attacker, CharacterObject defender)
        {
            return Math.Max((int)Math.Round(Random.Range(1, attacker.Attack) - defender.Defense), 1);
        }
    }
}