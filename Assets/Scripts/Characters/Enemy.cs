using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter {
    public Cell _currentCell;
    public Transform pointer;
    public float speed;
    public float actionCD = .25f;
    public float actionCDRemaining = 0;
    public BaseAttack mainAttack;
    public int _hp = 1;
    public ICharacter.CharacterType _myType = ICharacter.CharacterType.Player;
    public Vector2Int spawnPoint;
    public List<GameObject> curringAttacks;


    public Cell CurrentCell { get => _currentCell; }

    public ICharacter.CharacterType MyType { get => _myType; }
    public int HP { get => _hp; set => _hp = value; }

    void Start() {
        curringAttacks = new List<GameObject>();
        DoInitPosition();
        StartCoroutine(FollowPointer());
        TurnManager.Instance.AddToTurns(this);
    }

    public void GetDamage(int damage) {
        HP -= damage;
    }

    public void OnTurnChanged() {
        if (TurnManager.Instance.typeOfTheTurn == MyType) {
            Debug.Log("Turno enemigo");
            curringAttacks.RemoveAll(c => c == null);
           
            TryDoAction();
        }
    }

    public bool CanDoActions() {
        return TurnManager.Instance.typeOfTheTurn == this.MyType;
    }

    public Vector2Int GetSearchedPostion(ICharacter.Directions direction) {
        Vector2Int searchPosition = Vector2Int.zero;
        switch (direction) {
            case ICharacter.Directions.Up:
                searchPosition = (this.CurrentCell.mapPosition + Vector2Int.up);
                break;
            case ICharacter.Directions.Down:
                searchPosition = (this.CurrentCell.mapPosition + Vector2Int.down);
                break;
            case ICharacter.Directions.Left:
                searchPosition = (this.CurrentCell.mapPosition + Vector2Int.left);
                break;
            case ICharacter.Directions.Right:
                searchPosition = (this.CurrentCell.mapPosition + Vector2Int.right);
                break;
            default:
                break;
        }
        return searchPosition;
    }
    public bool TryWalk(ICharacter.Directions direction) {
        bool canWalk = false;

        return canWalk;
    }
    public bool Walk(Cell nextCell) {
        bool canWalk = false;
        if (canWalk = (nextCell.obstacle == null || nextCell.obstacle.IsWalkable())) {
            pointer.position = nextCell.transform.position;
            SetCurrentCell(nextCell);
        }
        return canWalk;
    }

    public void OnActionDone() {
        actionCDRemaining = actionCD;
        TurnManager.Instance.EndTurn(MyType);
    }


    public IEnumerator FollowPointer() {
        Vector3 startingPos = transform.position;
        float elapsedTime = 0;
        while (true) {
            if (pointer != null && Vector2.Distance(transform.position, pointer.position) >= 0.01f) {
                while (Vector2.Distance(transform.position, pointer.position) < 0.01f) {
                    transform.position = Vector3.Lerp(transform.position, pointer.position, (elapsedTime / speed));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = pointer.position;
               
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    

    public void SetCurrentCell(Cell cell) {
        if (_currentCell != null) {
            _currentCell.OverMeCharacter = null;
        }
        _currentCell = cell;
        CurrentCell.OverMeCharacter = this;
        pointer.position = CurrentCell.transform.position;

    }
   
    public void DoInitPosition() {
        SetCurrentCell(MapController.Instance.GetCell(new Vector2Int(spawnPoint.x, spawnPoint.y)));
        //transform.position = currentCell.transform.position;
    }
    public void TryDoAction() {
        List<Cell> cells = CheckPosibleCells();
        bool attacks = false;
        foreach (Cell cell in cells) {
            if (this.curringAttacks.Count == 0 && (cell.OverMeCharacter != null && cell.OverMeCharacter.MyType == this.mainAttack.targetType)) {
                IAttack attack = Instantiate(mainAttack, cell.transform);
                curringAttacks.Add(attack.GetGameObject());
                attack.Init(cell);
                attacks = true;
                OnActionDone();
                break;
            }
        }
        if (!attacks && cells.Count > 0) {
            TryWalk(cells[UnityEngine.Random.Range(0, cells.Count)]);
        }
    }
    public bool TryWalk(Cell nextCell) {
        bool canWalk = false;
        if (canWalk = ((nextCell.obstacle == null || nextCell.obstacle.IsWalkable()) && nextCell.OverMeCharacter == null)) {
            pointer.position = nextCell.transform.position;
            SetCurrentCell(nextCell);
        }
        OnActionDone();
        return canWalk;
    }
    public List<Cell> CheckPosibleCells() {
        List<Cell> cells = new List<Cell>();
        foreach (ICharacter.Directions direction in (ICharacter.Directions[])Enum.GetValues(typeof(ICharacter.Directions))) {
            Vector2Int searchPosition = GetSearchedPostion(direction);
            Cell tmpCell = MapController.Instance.GetCell(searchPosition);
            if (tmpCell != null) {
                if (tmpCell.obstacle == null || tmpCell.obstacle.IsWalkable()) {
                    cells.Add(tmpCell);
                }
            }
        }
        return cells;
    }
}
