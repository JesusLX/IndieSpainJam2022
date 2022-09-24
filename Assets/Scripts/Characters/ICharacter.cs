using UnityEngine.Events;

public interface ICharacter {
    enum Directions {
        Up, Down, Left, Right
    }
    public enum CharacterType {
        Player, Enemy, Neutral
    }
    public Cell CurrentCell { get; }
    public int HP { get; set; }
    public CharacterType MyType { get; }
    public void OnTurnChanged();
    void GetDamage(int damage);
    public bool TryWalk(Directions direction);
    public bool Walk(Cell nextCell);

    public void SetCurrentCell(Cell cell);
}