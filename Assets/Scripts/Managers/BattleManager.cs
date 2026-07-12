using Globals;
using Globals.Data.Classes;
using Placeholder;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Gestionnaire pour les combats.
/// <para>
/// Gère la progression des combats, le tour des personnages et le journal de combat.
/// </para>
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    [Header("Affectation inspecteur"), Space(30)]
    [Header("Hiérarchie")]
    public TextMeshProUGUI battleLog;

    public static event Action
        BattleStartEvent, BattleEndEvent,
        RoundStartEvent, RoundEndEvent;
    public static event Action<CharacterObject>
        TurnStartEvent, TurnEndEvent;
    public static event Action<CharacterObject, CharacterObject[]> HitEvent;
    public static event Action<CharacterObject, CharacterObject> HurtEvent;


    List<CharacterObject> characters = new();
    int roundCounter, turnCounter;
    string battleLogText = "";
    bool battleStarted, battleEnded,
        roundStarted, roundEnded,
        turnStarted, turnEnded;
    int currentTurn = 0, nextTurn;

#pragma warning disable CS0114 // Un membre masque un membre hérité ; le mot clé override est manquant
    private void Awake()
#pragma warning restore CS0114 // Un membre masque un membre hérité ; le mot clé override est manquant
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else Destroy(Instance);
        base.Awake();

        roundCounter = turnCounter = 1;
        battleStarted = roundStarted = turnStarted = true;
        battleEnded = roundEnded = turnEnded = false;

        foreach (var item in GameObject.FindGameObjectsWithTag("Character"))
        {
            characters.Add(item.GetComponent<CharacterObject2UI>().character);
            item.GetComponent<CharacterObject2UI>().character.DeadCharacterEvent += MournCharacterList;
        }
        characters = characters.OrderByDescending(c => c.Speed).ToList();
    }
    private void Start()
    {
        ClearBattleLogText();
        //NextBattleAction();
    }
    private void OnDisable()
    {
        foreach (var item in characters)
        {
            item.DeadCharacterEvent -= MournCharacterList;
        }
    }



    /// <summary>
    /// Ajoute une ligne au journal de combat et met à jour l'affichage du texte du journal.
    /// </summary>
    /// <param name="line">La ligne à ajouter</param>
    public void AddLine2BattleLog(string line = "")
    {
        if (string.IsNullOrEmpty(line.Trim())) return;

        battleLogText += line + "\n";
        UpdateBettleLogText();
    }
    /// <summary>
    /// Met à jour le texte du journal de combat dans l'UI avec le contenu actuel de battleLogText.
    /// </summary>
    void UpdateBettleLogText()
    {
        battleLog.text = battleLogText;
    }
    /// <summary>
    /// Efface le texte du journal de combat et met à jour l'affichage du texte du journal.
    /// </summary>
    public void ClearBattleLogText()
    {
        battleLogText = "";
        UpdateBettleLogText();
    }
    /// <summary>
    /// Passe à la prochaine étape de la progression du combat.
    /// <para>
    /// Gère et met à jour les états de début et de fin pour le combat, les tours et les rounds tout en déclenchant les événements correspondants.
    /// </para>
    /// </summary>
    /// <exception cref="Exception">Soulève une exception quand une étape de progression de bataille est en même temps commnecée et terminée, car impossible</exception>
    public void NextBattleAction()
    {
        string color, intensity, content, fullText;
        color = intensity = content = "";

        //Debug.Log($"[battleStarted, roundStarted, turnStarted]: [{battleStarted},{roundStarted},{turnStarted}]");
        //Debug.Log($"[battleEnded, roundEnded, turnEnded]: [{battleEnded},{roundEnded},{turnEnded}]");

        if (battleStarted & battleEnded |
            roundStarted & roundEnded |
            turnStarted & turnEnded
            )
        {
            throw new Exception("Étape de progression de bataille est en même temps commnecée et terminée");
        }


        // énorme check pour savoir quelle étape de progression de bataille va commencer ou terminer, ou si on est en progression normale d'un tour (tour d'un personnage)
        while (true)
        {
            if (characters.Count == 1)
            {
                //Debug.Log("Battle will end");
                battleEnded = true;
            }

            // checks pour étapes qui vont commencer
            if (battleStarted || roundStarted || turnStarted)
            {
                //Debug.Log("Un battle state commence");
                //Debug.Log($"[battleStarted, roundStarted, turnStarted]: [{battleStarted},{roundStarted},{turnStarted}]");
                color = "<color=\"green\">";

                if (battleStarted)
                {
                    intensity = "<i><b>";
                    content = "Battle started</b></i>";

                    BattleStartEvent?.Invoke();

                    battleStarted = false;
                }
                else if (roundStarted)
                {
                    intensity = "<b>";
                    content = $"Round #{roundCounter} started</b>";

                    RoundStartEvent?.Invoke();

                    roundStarted = false;
                }
                else if (turnStarted)
                {
                    content = $"Turn #{turnCounter}. Turn of <i>" +
                        $"{characters[currentTurn].CharacterName}</i> started";

                    TurnStartEvent?.Invoke(characters[currentTurn]);

                    turnStarted = false;
                }

                break;
            }

            // progression tour (une fois tour commencé si pas terminé)
            if (!(turnStarted | turnEnded | roundEnded) && !battleEnded)
            {
                //Debug.Log("action personnage (progression tour)");

                TurnHandler();
                break;
            }

            // checks pour étapes qui vont terminer
            if (battleEnded || roundEnded || turnEnded)
            {
                //Debug.Log("Un battle state termine");
                //Debug.Log($"[battleEnded, roundEnded, turnEnded]: [{battleEnded},{roundEnded},{turnEnded}]");
                color = "<color=\"red\">";

                if (turnEnded)
                {
                    content = $"Turn #{turnCounter}. Turn of <i>" +
                        $"{characters[currentTurn].CharacterName}</i> ended";

                    BattleEndEvent?.Invoke();

                    turnCounter++;
                    currentTurn = nextTurn;
                    turnEnded = false;
                    //turnStarted = true;
                    if (!(roundEnded | battleEnded)) turnStarted = true;

                    break;
                }
                else if (roundEnded)
                {
                    intensity = "<b>";
                    content = $"Round #{roundCounter} ended</b>";

                    RoundEndEvent?.Invoke();

                    roundCounter++;
                    roundEnded = false;
                    if (!battleEnded) roundStarted = turnStarted = true;
                }
                else if (battleEnded)
                {
                    intensity = "<i><b>";
                    content = "Battle ended</b></i>";

                    TurnEndEvent?.Invoke(characters[currentTurn]);

                    Destroy(gameObject);
                }

                break;
            }

            break;
        }

        //if (characters.Count > 1) RoundHandler();
        //else battleEnded = true;


        //Debug.Log("content: " + content);
        //Debug.Log(content.Length);
        fullText = (content.Length == 0) ? "" : (color + intensity + content + "</color>");
        //Debug.Log("fullText: " + fullText);

        AddLine2BattleLog(fullText);
    }
    /// <summary>
    /// Mini gestionnaire de tour.
    /// <para>
    /// Permet à un personnage de faire une action (attaquer un autre personnage), déclenche les événements correspondants et met à jour les compteurs de tour.
    /// </para>
    /// </summary>
    void TurnHandler()
    {
        int attackTarget = currentTurn;
        if (characters.Count > 2)
        {
            while (attackTarget == currentTurn)
            {
                attackTarget = Random.Range(0, characters.Count - 1);
            }
        }
        else attackTarget = (currentTurn == 0) ? 1 : 0;
        int randomDmg = Calculations.Damage(characters[currentTurn], characters[attackTarget]);
        //Debug.Log("randomDmg: " + randomDmg);

        AddLine2BattleLog($"{characters[currentTurn].CharacterName} a infligé {randomDmg} dégats à {characters[attackTarget].CharacterName}");
        //Debug.Log($"HP of {characters[attackTarget].CharacterName} before randomDmg: {characters[attackTarget].CurrentHp}");
        characters[attackTarget].CurrentHp -= randomDmg;
        //Debug.Log($"HP of {characters[attackTarget].CharacterName} after randomDmg: {characters[attackTarget].CurrentHp}");
        HitEvent?.Invoke(characters[currentTurn], new[] { characters[attackTarget] });
        HurtEvent?.Invoke(characters[attackTarget], characters[currentTurn]);



        nextTurn = currentTurn + 1;
        if (nextTurn >= characters.Count)
        {
            nextTurn = 0;
            roundEnded = true;
        }
        turnEnded = true;
    }
    /// <summary>
    /// Enlève un personnage de la liste des personnages vivants et ajuste les compteurs de tour si nécessaire.
    /// </summary>
    /// <param name="character">Le personnage décédé</param>
    void MournCharacterList(CharacterObject character)
    {
        int deadCharacterIndex = characters.IndexOf(character);

        if (deadCharacterIndex < currentTurn)
        {
            currentTurn--;
            nextTurn--;
        }
        characters.RemoveAt(deadCharacterIndex);
    }
}
