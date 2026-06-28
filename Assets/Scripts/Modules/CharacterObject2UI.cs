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
            BattleManager.HurtEvent += (hurtCharacter) => { if (hurtCharacter == character) UpdateCharacterSheetUI(); };
        }
        private void OnDisable()
        {
            character.DeadCharacterEvent -= KillCharacter;
            BattleManager.HurtEvent -= (hurtCharacter) => { if (hurtCharacter == character) UpdateCharacterSheetUI(); };
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