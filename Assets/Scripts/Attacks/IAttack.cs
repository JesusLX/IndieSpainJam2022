using System.Collections.Generic;
using UnityEngine;

public interface IAttack {
    List<Cell> affectedCells { get; set; }

    void Init(Cell tmpCell);
    void TryAttack();
    void PrepareAttack();
    void OnPrepareAttackDone();
    void Attack();
    void OnAttackDone();
    GameObject GetGameObject();
}