using Placeholder;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public TextMeshProUGUI battleLog;

    public static event Action
        BattleStartEvent, BattleEndEvent,
        RoundStartEvent, RoundEndEvent,
        TurnStartEvent, TurnEndEvent;
    public static event Action<CharacterObject>
        HitEvent, HurtEvent;


    List<CharacterObject> characters = new();
    int roundCounter, turnCounter;
    string battleLogText = "";
    bool battleStarted, battleEnded,
        roundStarted, roundEnded,
        turnStarted, turnEnded;
    int currentTurn = 0, nextTurn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(Instance);

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
            throw new Exception("State used for battle processing can't be started and ended at the same time");
        }


        while (true)
        {
            if (characters.Count == 1)
            {
                //Debug.Log("Battle will end");
                battleEnded = true;
            }

            // checks pour états qui vont commencer
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

                    TurnStartEvent?.Invoke();

                    turnStarted = false;
                }

                break;
            }

            // progression tour (une fois tour commencé si pas terminé)
            if (!(turnStarted | turnEnded | roundEnded) && !battleEnded)
            {
                //Debug.Log("action personnage (progression tour)");

                RoundHandler();
                break;
            }

            // checks pour états qui vont terminer
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

                    TurnEndEvent?.Invoke();

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
    void RoundHandler()
    {
        //Debug.Log("progression tour normale");
        TurnHandler();
    }
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
        int randomDmg = (int)Random.Range(1, characters[currentTurn].Attack);

        AddLine2BattleLog($"{characters[currentTurn].CharacterName} a infligé {randomDmg} dégats à {characters[attackTarget].CharacterName}");
        HitEvent?.Invoke(characters[currentTurn]);
        HurtEvent?.Invoke(characters[attackTarget]);
        characters[attackTarget].CurrentHp -= randomDmg;




        nextTurn = currentTurn + 1;
        if (nextTurn >= characters.Count)
        {
            nextTurn = 0;
            roundEnded = true;
        }
        turnEnded = true;
    }

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
