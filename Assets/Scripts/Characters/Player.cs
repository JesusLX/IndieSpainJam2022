using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ICharacter {
    public Cell _currentCell;
    public Transform pointer;
    public float speed;
    public float actionCD = .25f;
    public float actionCDRemaining = 0;
    public BaseAttack mainAttack;
    public int _hp = 1;
    public ICharacter.CharacterType _myType = ICharacter.CharacterType.Player;
    public int turns = 100;
    public Dictionary<IItem.Type, int> inventory;

    public UnityEvent<IItem.Type> OnItemAdded;
    public UnityEvent<IItem.Type> OnItemRemoved;
    public UnityEvent OnTurnPassed;

    public Animator animator;


    public Cell CurrentCell { get => _currentCell; }

    public ICharacter.CharacterType MyType { get => _myType; }
    public int HP { get => _hp; set => _hp = value; }

    void Start() {
        inventory = new Dictionary<IItem.Type, int>();
        StartCoroutine(FollowPointer());
        DoInitPosition();
        TurnManager.Instance.AddToTurns(this);
        if(animator == null) {
            animator = GetComponent<Animator>();
        }
    }

    void Update() {
        if (this.CanDoActions() && this.actionCDRemaining <= 0) {
            if (Input.GetAxisRaw("Vertical") == 1) {
                TryWalk(ICharacter.Directions.Up);
            }else
            if (Input.GetAxisRaw("Vertical") == -1) {
                TryWalk(ICharacter.Directions.Down);
            }else
            if (Input.GetAxisRaw("Horizontal") == 1) {
                TryWalk(ICharacter.Directions.Right);
            }else
            if (Input.GetAxisRaw("Horizontal") == -1) {
                TryWalk(ICharacter.Directions.Left);
            }else
            if (Input.GetKeyDown(KeyCode.Space)) {
                TryDoAction();
            }
        }


        if (actionCDRemaining > 0) {
            actionCDRemaining -= Time.deltaTime;
        }
    }

    public void GetDamage(int damage) {
        HP -= damage;
        if (HP <= 0) {
            TryDie();
        }
    }

    public void OnTurnChanged() {
        if (TurnManager.Instance.typeOfTheTurn == MyType) {

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
        Cell tmpCell;
        bool canWalk = false;
        Vector2Int searchPosition = GetSearchedPostion(direction);
        tmpCell = MapController.Instance.GetCell(searchPosition);
        if (tmpCell != null) {
            Walk(tmpCell);
        }

        return canWalk;
    }
    public bool Walk(Cell nextCell) {
        bool canWalk = false;
        if (canWalk = ((nextCell.obstacle == null || nextCell.obstacle.IsWalkable()) && nextCell.OverMeCharacter == null)) {
            Debug.Log(nextCell);
            pointer.position = nextCell.transform.position;
            SetCurrentCell(nextCell);
            OnActionDone();
        }
        return canWalk;
    }

    public void OnActionDone() {
        turns--;
        actionCDRemaining = actionCD;
        OnTurnPassed?.Invoke();
        TurnManager.Instance.EndTurn(MyType);
    }

    [ContextMenu("DoInitPosition")]
    public void DoInitPosition() {
        Vector2Int entrancePosition = MapController.Instance.EntrancePosition;
        SetCurrentCell(MapController.Instance.GetCell(new Vector2Int(entrancePosition.x, entrancePosition.y)));
        //transform.position = currentCell.transform.position;
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

    public void TryDoAction() {
        foreach (ICharacter.Directions direction in (ICharacter.Directions[])Enum.GetValues(typeof(ICharacter.Directions))) {
            Vector2Int searchPosition = GetSearchedPostion(direction);
            Cell tmpCell = MapController.Instance.GetCell(searchPosition);
            if (tmpCell != null) {
                    Debug.Log(tmpCell.obstacle);
                if (tmpCell.obstacle != null && tmpCell.obstacle.HasAction()) {
                    tmpCell.obstacle.DoAction();
                } else if (tmpCell.obstacle == null || tmpCell.obstacle.IsWalkable()) {
                    Instantiate(mainAttack, tmpCell.transform).GetComponent<BaseAttack>().Init(tmpCell);
                }
            }
        }
        OnActionDone();
    }

    public void SetCurrentCell(Cell cell) {
        if (_currentCell != null) {
            _currentCell.OverMeCharacter = null;
        }
        _currentCell = cell;
        CurrentCell.OverMeCharacter = this;
        pointer.position = CurrentCell.transform.position;

    }

    public void AddItem(IItem.Type itemType) {
        if (inventory.ContainsKey(itemType)) {
            inventory[itemType]++;
        } else {
            inventory[itemType] = 1;
        }
        OnItemAdded?.Invoke(itemType);
    }
    public bool SubstractItem(IItem.Type itemType) {
        if (inventory.ContainsKey(itemType)) {
            inventory[itemType]--;
            OnItemRemoved?.Invoke(itemType);
            return true;
        } else {
            return false;
        }
    }

    public int GetItemCount(IItem.Type itemType) {
        if (inventory.ContainsKey(itemType)) {
            return inventory[itemType];
        } else {
           return 0;
        }
    }

    public void TryDie() {
        animator.Play("Die");
    }

    public void Die() {
        GameManager.Instance.Reset();
    }
}
