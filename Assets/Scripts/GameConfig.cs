using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfig), menuName = "Configs/" + nameof(GameConfig), order = 0)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private Vector2Int fieldSize = new(5, 6);
    [SerializeField] private Vector2Int startPoint = new(0, 0);
    [SerializeField] private Vector2Int endPoint = new(5, 6);
    [SerializeField] private float ballSpeed = 0.3f;
    [SerializeField] private MazeCell cellPrefab;
    [SerializeField] private GameObject playerPrefab;

    public Vector2Int FieldSize => fieldSize;
    public Vector2Int StartPoint => startPoint;
    public Vector2Int EndPoint => endPoint;
    public MazeCell CellPrefab => cellPrefab;
    public GameObject PlayerPrefab => playerPrefab;
    public float BallSpeed => ballSpeed;

    private void OnValidate()
    {
        if (fieldSize.x < 1) fieldSize.x = 1;
        if (fieldSize.y < 1) fieldSize.y = 1;

        startPoint.x = Mathf.Clamp(startPoint.x, 0, fieldSize.x - 1);
        startPoint.y = Mathf.Clamp(startPoint.y, 0, fieldSize.y - 1);

        endPoint.x = Mathf.Clamp(endPoint.x, 0, fieldSize.x - 1);
        endPoint.y = Mathf.Clamp(endPoint.y, 0, fieldSize.y - 1);
    }
}