using Model.SO;
using TMPro;
using UnityEngine;

namespace View.Placeholder
{
    /// <summary>
    /// Sert à afficher les informations d'un CharacterObject dans l'UI.
    /// </summary>
    /// <remarks>Placeholder</remarks>
    public class CharacterObject2UI : MonoBehaviour
    {
        [Header("Affectation inspecteur"), Space(30)]
        [Header("Projet")]
        public CharacterObject character;

        GameObject textContainer;

        private void Awake()
        {
            textContainer = transform.Find("TextContainer").gameObject;

            UpdateCharacterSheetUI();
        }
        private void OnEnable()
        {
            character.DeadCharacterEvent += KillCharacter;
            //BattleManager.HurtEvent += (hurtCharacter, _) => { if (hurtCharacter == character) UpdateCharacterSheetUI(); };
        }
        private void OnDisable()
        {
            character.DeadCharacterEvent -= KillCharacter;
            //BattleManager.HurtEvent -= (hurtCharacter, _) => { if (hurtCharacter == character) UpdateCharacterSheetUI(); };
        }



        /// <summary>
        /// Met à jour l'affichage des informations du personnage dans l'UI.
        /// </summary>
        void UpdateCharacterSheetUI()
        {
            textContainer.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = character.CharacterName;
            UpdateCharacterHpUI();
            textContainer.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"Attack: {character.Attack}";
            textContainer.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"Speed: {character.Speed}";
            textContainer.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"Defense: {character.Defense}";
        }
        /// <summary>
        /// Met à jour l'affichage des PV du personnage dans l'UI.
        /// </summary>
        void UpdateCharacterHpUI()
        {
            textContainer.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"HP: {character.CurrentHp}/{character.MaxHp}";
        }
        /// <summary>
        /// Détruit le GameObject représantant le personnage
        /// </summary>
        /// <param name="_">Le personnage qui est mort</param>
        void KillCharacter(CharacterObject _)
        {
            Destroy(gameObject);
        }
    }
}