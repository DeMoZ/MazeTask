using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class MazeSpawner : IDisposable
{
    private readonly MazeCell _cellPrefab;

    public MazeSpawner(MazeCell cellPrefab)
    {
        _cellPrefab = cellPrefab;
    }

    public void Spawn(Cell[,] maze)
    {
        var xLenght = maze.GetLength(0);
        var yLenght = maze.GetLength(1);
        var xOffset = xLenght / 2;
        var yOffset = yLenght / 2;

        for (var x = 0; x < xLenght; x++)
        {
            for (var y = 0; y < yLenght; y++)
            {
                var cell = Object.Instantiate(_cellPrefab, new Vector2(x - xOffset, y - yOffset), Quaternion.identity);
                cell.wallLeft.SetActive(maze[x, y].wallLeft);
                cell.wallBottom.SetActive(maze[x, y].wallBottom);
                cell.start.SetActive(maze[x, y].start);
                cell.end.SetActive(maze[x, y].end);
                cell.decor.SetActive(maze[x, y].decor);
            }
        }
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}