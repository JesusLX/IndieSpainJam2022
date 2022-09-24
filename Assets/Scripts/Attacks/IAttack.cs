using System.Collections.Generic;

public interface IAttack {
    List<Cell> affectedCells { get; set; }

    void Init(Cell tmpCell);
    void TryAttack();
    void PrepareAttack();
    void OnPrepareAttackDone();
    void Attack();
    void OnAttackDone();
}