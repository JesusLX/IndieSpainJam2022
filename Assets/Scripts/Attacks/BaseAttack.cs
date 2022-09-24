using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : MonoBehaviour, IAttack {
    List<Cell> _affectedCells;
    public int constTurnsToAttack;
    private int turnsToAttack;
    public int constRepeats;
    private int repeats;
    public ICharacter.CharacterType targetType;
    public ICharacter.CharacterType invockerType;
    public int damage;
    public Animator visual;
    public List<Cell> affectedCells { get => _affectedCells; set => _affectedCells = value; }

    public void PrepareAttack() {
        if(this.turnsToAttack == this.constTurnsToAttack) {
            visual.SetTrigger("PrepareAttackInit");
        } else {
            Debug.Log("Prepare attack");
            visual.SetTrigger("PrepareAttack");
        }
    }

    public void OnPrepareAttackDone() {
        this.turnsToAttack--;

        if (this.turnsToAttack < 0) {
            Attack();
        } else {
            TurnManager.Instance.EndTurn(invockerType);
        }
    }

    public void Attack() {
        visual.SetTrigger("DoAttack");
    }

    public void OnAttackDone() {
        affectedCells.ForEach(c => c.DoDamage(targetType, damage));
        repeats--;
        if(repeats <= 0) {
            TurnManager.Instance.RemoveToTurns(this.invockerType, TryAttack);
            Destroy(gameObject);
        }
        TurnManager.Instance.EndTurn(invockerType);

    }

    public void TryAttack() {
        PrepareAttack();
    }

    public void Init(Cell tmpCell) {
        this.affectedCells = new List<Cell>();
        this.affectedCells.Add(tmpCell);
        this.turnsToAttack = this.constTurnsToAttack;
        TurnManager.Instance.AddToTurns(this.invockerType, TryAttack);
        TryAttack();
    }

    public GameObject GetGameObject() {
        return this.gameObject;
    }
}
