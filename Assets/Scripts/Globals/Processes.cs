using System;
using Random = UnityEngine.Random;

namespace Globals
{
    /// <summary>
    /// Classe responsable des gros calculs récurrents.
    /// </summary>
    public static class Calculations
    {
        /// <summary>
        /// Calcule les dégâts infligés par un attaquant à un défenseur.
        /// </summary>
        /// <param name="attacker">Le personnage qui inflige les dégâts</param>
        /// <param name="defender">Le personnage qui reçoit les dégats</param>
        /// <returns>La quantité de dégâts résultant du calcul, converti en <see langword="int"/></returns>
        public static int Damage(CharacterObject attacker, CharacterObject defender)
        {
            return Math.Max((int)Math.Round(Random.Range(1, attacker.Attack) - defender.Defense), 1);
        }
    }
}