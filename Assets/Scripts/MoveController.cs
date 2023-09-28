using System;
using DG.Tweening;
using UnityEngine;

public class MoveController : IDisposable
{
    private readonly GameObject _player;
    private readonly SwipeDetector _swipeDetector;
    private readonly Cell[,] _maze;
    private readonly MazeSpawner _mazeSpawner;
    private readonly float _speed;

    private Vector2Int _currentCell;
    private bool _isMoving;

    public MoveController(GameObject player, SwipeDetector swipeDetector, Cell[,] maze, MazeSpawner mazeSpawner,
        Vector2Int startCell, float speed)
    {
        _player = player;
        _swipeDetector = swipeDetector;
        _maze = maze;
        _mazeSpawner = mazeSpawner;
        _currentCell = startCell;
        _speed = speed;

        _swipeDetector.OnSwipe += OnSwipe;
    }

    public void Dispose()
    {
        _swipeDetector.OnSwipe -= OnSwipe;
    }

    private void OnSwipe(SwipeDetector.SwipeDirection direction)
    {
        if (_isMoving) return;

        var nextX = _currentCell.x;
        var nextY = _currentCell.y;
        var steps = 0;
        
        switch (direction)
        {
            case SwipeDetector.SwipeDirection.Left:
                for (var x = _currentCell.x; x > 0; x--)
                {
                    if (!_maze[x, nextY].wallLeft)
                    {
                        nextX = x - 1;
                        steps++;
                    }
                    else
                        break;
                }
                break;
            
            case SwipeDetector.SwipeDirection.Right:
                for (var x = _currentCell.x; x < _maze.GetLength(0); x++)
                {
                    if (!_maze[x + 1, nextY].wallLeft)
                    {
                        nextX = x + 1;
                        steps++;
                    }
                    else
                        break;
                }
                break;
            
            case SwipeDetector.SwipeDirection.Up:
                for (var y = _currentCell.y; y < _maze.GetLength(0) - 1; y++)
                {
                    if (!_maze[nextX, y + 1].wallBottom)
                    {
                        nextY = y + 1;
                        steps++;
                    }
                    else
                        break;
                }
                break;
            case SwipeDetector.SwipeDirection.Down:
                for (var y = _currentCell.y; y > 0; y--)
                {
                    if (!_maze[nextX, y].wallBottom)
                    {
                        nextY = y - 1;
                        steps++;
                    }
                    else
                        break;
                }
                break;
        }

        if (steps == 0) return;
        
        _isMoving = true;

        var toPos = _mazeSpawner.GetInCellCoordinates(nextX, nextY);

        // Debug.Log($"------------");
        // Debug.Log($"curXY[{_currentCell}], coord from { _mazeSpawner.GetInCellCoordinates(_currentCell)};");
        // Debug.Log($"nextXY[{nextX};{nextY}], coord to {toPos};");
        
        _player.transform.DOMove(toPos, steps * _speed).SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                _isMoving = false;
                _currentCell.x = nextX;
                _currentCell.y = nextY;
            });
    }
}