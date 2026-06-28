using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterObject", menuName = "Scriptable Objects/CharacterObject")]
public class CharacterObject : ScriptableObject
{
    [Header("Identification")]
    public string CharacterName;

    [Header("Stats")]
    public int MaxHp;
    public int CurrentHp { get; private set; }
    public int Attack, Speed;

    public event Action<CharacterObject> DeadCharacterEvent;

    private void OnEnable()
    {
        CurrentHp = MaxHp;
    }


    /// <summary>
    /// Setter pour mettre à jour les pv actuels du personnage, en s'assurant qu'ils restent dans les limites de 0 et MaxHp.
    /// </summary>
    /// <param name="hp">La nouvelle valeur de pv</param>
    public void SetCurrentHp(int hp)
    {
        CurrentHp = Math.Clamp(hp, 0, MaxHp);
        if (CurrentHp == 0) Die();
    }
    public void Die()
    {
        DeadCharacterEvent?.Invoke(this);
    }
}
