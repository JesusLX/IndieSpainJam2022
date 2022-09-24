using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    UnityEvent OnTurnChanged;
    Dictionary<ICharacter.CharacterType, int> actionsPerTurn;
    public ICharacter.CharacterType typeOfTheTurn;
    int actionsThisTurn = 0;

    private void Awake() {
        if (OnTurnChanged == null)
            OnTurnChanged = new UnityEvent();

        actionsPerTurn = new Dictionary<ICharacter.CharacterType, int>();
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }


    void Start() {
        
    }

    public void AddToTurns(ICharacter character) {
        this.AddToTurns(character.MyType, 
            character.OnTurnChanged);
    }

    public void AddToTurns(ICharacter.CharacterType character, UnityAction OnTurnChangedAction) {
        OnTurnChanged.AddListener(OnTurnChangedAction);
        if (!actionsPerTurn.ContainsKey(character)) {
            actionsPerTurn[character] = 1;
        } else {
            actionsPerTurn[character]++;
        }
    }

    public void RemoveToTurns(ICharacter character) {
        RemoveToTurns(character.MyType, character.OnTurnChanged);
    }

    public void RemoveToTurns(ICharacter.CharacterType character, UnityAction OnTurnChangedAction) {
        OnTurnChanged.RemoveListener(OnTurnChangedAction);
        if (actionsPerTurn.ContainsKey(character)) {
            actionsPerTurn[character]--;
        }
    }

    public void EndTurn(ICharacter.CharacterType character) {
        this.actionsThisTurn--;
        if(this.actionsThisTurn <= 0) {
            SetNextTurn();
            OnTurnChanged?.Invoke();
        }
    }

    private ICharacter.CharacterType  SetNextTurn() {
        int turnIndex = actionsPerTurn.Keys.ToList().IndexOf(typeOfTheTurn);
        if(turnIndex == actionsPerTurn.Count-1) {
            turnIndex = 0;
        } else {
            turnIndex++;
        }
        this.typeOfTheTurn = actionsPerTurn.Keys.ToList()[turnIndex];
        actionsPerTurn.TryGetValue(this.typeOfTheTurn, out actionsThisTurn);
        return this.typeOfTheTurn;
    }
}
