using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class MazeSpawner : IDisposable
{
    private readonly MazeCell _cellPrefab;
    private readonly Transform _mazeParent;

    private readonly float _xCellOffset;
    private readonly float _yCellOffset;

    private int _xMazeOffset;
    private int _yMazeOffset;

    public MazeSpawner(MazeCell cellPrefab, Transform mazeParent)
    {
        _cellPrefab = cellPrefab;
        _mazeParent = mazeParent;

        _xCellOffset = _cellPrefab.size.x;
        _yCellOffset = _cellPrefab.size.y;
    }

    public void Spawn(Cell[,] maze)
    {
        var xLenght = maze.GetLength(0);
        var yLenght = maze.GetLength(1);

        _xMazeOffset = xLenght / 2;
        _yMazeOffset = yLenght / 2;

        for (var x = 0; x < xLenght; x++)
        {
            for (var y = 0; y < yLenght; y++)
            {
                var cell = Object.Instantiate(_cellPrefab,
                    new Vector2(x - _xMazeOffset, y - _yMazeOffset), Quaternion.identity, _mazeParent);

                cell.wallLeft.SetActive(maze[x, y].wallLeft);
                cell.wallBottom.SetActive(maze[x, y].wallBottom);
                cell.start.SetActive(maze[x, y].start);
                cell.end.SetActive(maze[x, y].end);
                cell.decor.SetActive(maze[x, y].decor);
            }
        }
    }

    public Vector2 GetInCellCoordinates(int x, int y)
    {
        return new Vector2
        {
            x = x - _xMazeOffset + _xCellOffset,
            y = y - _yMazeOffset + _yCellOffset
        };
    }

    public Vector2 GetInCellCoordinates(Vector2Int coordinates)
    {
        return GetInCellCoordinates(coordinates.x, coordinates.y);
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}