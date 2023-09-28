using System;
using System.Collections.Generic;
using UnityEngine;

public class Maze : IDisposable
{
    private readonly int[,] _maze;
    private readonly Vector2Int _start;
    private readonly Vector2Int _end;
    
    List<Vector2> visitedCells = new List<Vector2>();
    
    public Maze(Vector2Int size, Vector2Int start, Vector2Int end)
    {
        _maze = new int[size.x, size.y];
        _start = start;
        _end = end;

        Generate();
    }

    public void Generate()
    {
        for (int i = 0; i < _maze.GetLength(0); i++)
        {
            for (int j = 0; j < _maze.GetLength(1); j++)
            {
                if (_maze[i, j] == 0)
                {
                    // Клетка свободна, рисуем ее как проход
                    // Debug.DrawRect(new Rect(i * 10, j * 10, 10, 10), Color.White);
                }
                else
                {
                    // Клетка занята, рисуем ее как стену
                    // Debug.DrawRect(new Rect(i * 10, j * 10, 10, 10), Color.Black);
                }
            }
        }
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}