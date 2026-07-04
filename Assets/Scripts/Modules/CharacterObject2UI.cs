using TMPro;
using UnityEngine;

namespace Placeholder
{
    public class CharacterObject2UI : MonoBehaviour
    {
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
            BattleManager.HurtEvent += (hurtCharacter, _) => { if (hurtCharacter == character) UpdateCharacterSheetUI(); };
        }
        private void OnDisable()
        {
            character.DeadCharacterEvent -= KillCharacter;
            BattleManager.HurtEvent -= (hurtCharacter, _) => { if (hurtCharacter == character) UpdateCharacterSheetUI(); };
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
        void KillCharacter(CharacterObject _)
        {
            Destroy(gameObject);
        }
    }
}