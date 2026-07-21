using UnityEngine;

namespace Model.Modules
{
    /// <summary>
    /// Représentation d'une case dans le jeu.
    /// </summary>
    /// <remarks>
    /// Le code présent sert de placeholder simple pour représenter une case dans le jeu.
    /// </remarks>
    public class Tile : MonoBehaviour
    {
        [Header("Affectation inspecteur"), Space(30)]
        [Header("Projet")]
        public MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}