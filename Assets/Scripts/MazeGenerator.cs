using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cell
{
    public int x;
    public int y;

    public bool wallLeft = true;
    public bool wallBottom = true;

    public bool decor = true;
    public bool visited;
    public bool start;
    public bool end;
}

public class MazeGenerator : IDisposable
{
    private readonly Vector2Int _size;
    private readonly Vector2Int _start;
    private readonly Vector2Int _end;

    private Cell[,] _maze;
    public Cell[,] Maze => _maze;

    public MazeGenerator(Vector2Int size, Vector2Int start, Vector2Int end)
    {
        _size = size + Vector2Int.one;
        
        _start = start;
        _end = end;
    }

    public void Generate()
    {
        _maze = new Cell[_size.x, _size.y];

        for (var x = 0; x < _maze.GetLength(0); x++)
        {
            for (var y = 0; y < _maze.GetLength(1); y++)
            {
                _maze[x, y] = new Cell { x = x, y = y };
            }
        }

        for (var x = 0; x < _maze.GetLength(0); x++)
        {
            _maze[x, _size.y - 1].wallLeft = false;
            _maze[x, _size.y - 1].decor = false;
        }

        for (var y = 0; y < _maze.GetLength(1); y++)
        {
            _maze[_size.x - 1, y].wallBottom = false;
            _maze[_size.x - 1, y].decor = false;
        }

        RemoveWallsWithBackTracker();
        PlaceStartEnd();
    }

    private void PlaceStartEnd()
    {
        _maze[_start.x, _start.y].start = true;
        _maze[_end.x, _end.y].end = true;
    }

    private void RemoveWallsWithBackTracker()
    {
        var current = _maze[0, 0];
        current.visited = true;
        var stack = new Stack<Cell>();

        do
        {
            var unvisitedNeighbours = new List<Cell>();
            var x = current.x;
            var y = current.y;

            if (x > 0 && !_maze[x - 1, y].visited) unvisitedNeighbours.Add(_maze[x - 1, y]);
            if (y > 0 && !_maze[x, y - 1].visited) unvisitedNeighbours.Add(_maze[x, y - 1]);
            if (x < _size.x - 2 && !_maze[x + 1, y].visited) unvisitedNeighbours.Add(_maze[x + 1, y]);
            if (y < _size.y - 2 && !_maze[x, y + 1].visited) unvisitedNeighbours.Add(_maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                var chosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, chosen);

                chosen.visited = true;
                stack.Push(chosen);
                current = chosen;
            }
            else
            {
                if(stack.Count > 0)
                    current = stack.Pop();
            }
            
        } while (stack.Count > 0);
    }

    private void RemoveWall(Cell a, Cell b)
    {
        if (a.x == b.x)
        {
            if (a.y > b.y) a.wallBottom = false;
            else b.wallBottom = false;
        }
        else
        {
            if (a.x > b.x) a.wallLeft = false;
            else b.wallLeft = false;
        }
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}