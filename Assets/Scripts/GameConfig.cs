using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfig), menuName = "Configs/" + nameof(GameConfig), order = 0)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private Vector2Int fieldSize = new (5, 6);
    [SerializeField] private Vector2Int startPoint = new (0, 0);
    [SerializeField] private Vector2Int endPoint = new (5, 6);

    public Vector2Int FieldSize => fieldSize;
    public Vector2Int StartPoint => startPoint;
    public Vector2Int EndPoint => endPoint;

    private void OnValidate()
    {
        if (fieldSize.x < 1) fieldSize.x = 1;
        if (fieldSize.y < 1) fieldSize.y = 1;

        startPoint.x = Mathf.Clamp(startPoint.x, 0, fieldSize.x);
        startPoint.y = Mathf.Clamp(startPoint.y, 0, fieldSize.y);
        
        endPoint.x = Mathf.Clamp(endPoint.x, 0, fieldSize.x);
        endPoint.y = Mathf.Clamp(endPoint.y, 0, fieldSize.y);
    }
}