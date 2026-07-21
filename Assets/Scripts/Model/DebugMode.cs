using Model.Globals;
using NaughtyAttributes;
using UnityEngine;

namespace Model
{
#if UNITY_EDITOR
    /// <summary>
    /// Classe qui sert d'outil de déboguage/développement pour le projet.
    /// </summary>
    public class DebugMode : Singleton<DebugMode>
    {
        [Tooltip("Active le mode de déboguage/développeur ou non")]
        public bool activateDebugMode = false;

        [Header("Character"), Space(30)]
        [ShowIf(nameof(activateDebugMode)), Tooltip("Montre ModifiedValue et BaseValue au lieu de juste ModifiedValue")]
        [SerializeField]
        bool alsoOutputBaseValue;
        public bool AlsoOutputBaseValue
        {
            get => activateDebugMode && alsoOutputBaseValue;
            set => alsoOutputBaseValue = value;
        }
    }
#endif
}